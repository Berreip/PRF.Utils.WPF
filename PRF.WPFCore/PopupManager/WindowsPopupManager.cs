using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using PRF.WPFCore.UiWorkerThread;

namespace PRF.WPFCore.PopupManager
{
    /// <summary>
    /// Module de gestion des fenêtres
    /// </summary>
    public interface IWindowsPopupManager<in TWindowKey>
    {
        /// <summary>
        /// Demande la fermeture de la fenêtre assossié au viewModel
        /// </summary>
        Task CloseWindowsAsync(IPopupWindowsViewModel viewModelReference);

        /// <summary>
        /// Demande la fermeture de toutes les fenêtres de popup (principalement les fenêtres de détails des puits)
        /// </summary>
        Task CloseAllWindows();
        
        /// <summary>
        /// Génère une fenêtre du type demandé puis affiche la fenêtre en mode ShowDialog() (affichage par dessus 
        /// et sans accès aux autres fenêtres)
        /// </summary>
        void ShowDialog(TWindowKey name);

        /// <summary>
        /// Génère une fenêtre du type demandé  la cast en la classe spécifié par T puis affiche la fenêtre en mode ShowDialog()
        ///  (affichage par dessus et sans accès aux autres fenêtres) ET appelle une action de chargement sur ce type
        /// </summary>
        void ShowDialog<T>(TWindowKey name, Action<T> loadAction)
            where T : class, IPopupWindowsViewModel;

        /// <summary>
        /// Génère une fenêtre du type demandé puis affiche la fenêtre en mode Show() (affichage en parallèle aux autres fenêtres)
        /// </summary>
        void Show(TWindowKey name);

        /// <summary>
        /// Génère une fenêtre du type demandé la cast en la classe spécifié par T puis affiche la fenêtre en mode Show()
        ///  (affichage en parallèle aux autres fenêtres) ET appelle une action de chargement sur ce type
        /// </summary>
        void Show<T>(TWindowKey name, Action<T> loadAction) where T : class, IPopupWindowsViewModel;
    }

    /// <inheritdoc />
    public sealed class WindowsPopupManager<TWindowKey> : IWindowsPopupManager<TWindowKey>
    {
        private readonly IInjectionContainer _container;
        private readonly Dictionary<IPopupWindowsViewModel, WindowWrapper> _refWindowByViewModel
            = new Dictionary<IPopupWindowsViewModel, WindowWrapper>();

        /// <summary>
        /// Dictionnaire des fenêtre unique (si une de ces fenêtre est déjà affiché, on la réaffiche mais on n'en créer pas une seconde
        /// </summary>
        private readonly Dictionary<TWindowKey, Window> _alreadyCreatedSingleWindows
            = new Dictionary<TWindowKey, Window>();


        private readonly Dictionary<TWindowKey, RegistrationWindowsReference> _windowReference = new Dictionary<TWindowKey, RegistrationWindowsReference>();
        private readonly object _key = new object();

        /// <summary>
        /// Constructeur du gestionnaire de fenêtre. On lui donne le container d'injection à utiliser
        /// </summary>
        /// <param name="container"></param>
        public WindowsPopupManager(IInjectionContainer container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public async Task CloseWindowsAsync(IPopupWindowsViewModel viewModelReference)
        {
            WindowWrapper view;
            lock (_key)
            {
                // retire le viewModel de la référence afin de ne pas pouvoir appeler cette méthode N fois le temps que la fenêtre se ferme
                if (viewModelReference == null || !_refWindowByViewModel.TryGetValue(viewModelReference, out view)) return;
            }

            await UiThreadDispatcher.ExecuteOnUIAsync(() =>
            {
                // déclenche la fermeture de la fenêtre (qui lèvera l'evènement OnWindowClosed qui appelera
                // à son tour la méthode OnClose() du ViewModel)
                view.Window.Close();
            }).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task CloseAllWindows()
        {
            while (_refWindowByViewModel.Count != 0)
            {
                await CloseWindowsAsync(_refWindowByViewModel.Keys.FirstOrDefault()).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Enregistre un type de fenêtre (view + viewModel) avec un nom en Transient (une instance pour chaque) pour affichage ultérieur
        /// On exige un constructeur vide pour la vue car le viewModel est setté via la propriété DataContext après la résolution
        /// </summary>
        /// <typeparam name="TViewModel">le type du viewModel</typeparam>
        /// <typeparam name="TWindow">le type de la vue</typeparam>
        /// <param name="name">le nom avec lequel on retrouvera cette fenêtre</param>
        /// <param name="registrationMode">le mode d'enregistrement de cette fenêtre</param>
        public void RegisterWindow<TViewModel, TWindow>(TWindowKey name, WindowRegistrationMode registrationMode = WindowRegistrationMode.TransientSingle)
            where TViewModel : class, IPopupWindowsViewModel
            where TWindow : Window, new()
        {
            lock (_key)
            {
                _container.RegisterType<TViewModel>(LifeTime.Transient);
                _container.RegisterType<TWindow>(LifeTime.Transient);
                // stocke le type d'enregistrement par nom
                _windowReference.Add(name, new RegistrationWindowsReference(typeof(TViewModel), typeof(TWindow), registrationMode));
            }

        }

        /// <inheritdoc />
        public void Show(TWindowKey name)
        {
            Show<IPopupWindowsViewModel>(name, null);
        }

        /// <inheritdoc />
        public void Show<T>(TWindowKey name, Action<T> loadAction)
            where T : class, IPopupWindowsViewModel
        {
            if (TryGetType(name, out var refWindow)) return;
            WrapperCoreUI.DispatchAsync(() => CreateViewAndViewModel(name, refWindow, loadAction).Show()).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void ShowDialog(TWindowKey name)
        {
            ShowDialog<IPopupWindowsViewModel>(name, null);
        }

        /// <inheritdoc />
        public void ShowDialog<T>(TWindowKey name, Action<T> loadAction)
            where T : class, IPopupWindowsViewModel
        {
            if (TryGetType(name, out var refWindow)) return;
            WrapperCoreUI.Dispatch(() => CreateViewAndViewModel(name, refWindow, loadAction).ShowDialog());
        }

        private bool TryGetType(TWindowKey name, out RegistrationWindowsReference refWindow)
        {
            if (_windowReference.TryGetValue(name, out refWindow)) return false;

            MessageBox.Show($"@Erreur: impossible de trouver une fenêtre enregistrée avec le nom {name}");
            return true;

        }

        private Window CreateViewAndViewModel<T>(TWindowKey name, RegistrationWindowsReference refWindow,
            Action<T> loadAction)
            where T : class, IPopupWindowsViewModel
        {
            lock (_key)
            {
                // tout d'abord, on regarde si la fenêtre existe déjà si l'on est en mode TransientSingle
                if (refWindow.RegistrationMode == WindowRegistrationMode.TransientSingle
                    && _alreadyCreatedSingleWindows.TryGetValue(name, out var alreadyCreatedWindow))
                {
                    // passage au premier plan:
                    if (!alreadyCreatedWindow.IsActive)
                    {
                        alreadyCreatedWindow.Activate();
                    }
                    return alreadyCreatedWindow;
                }
                // SINON, on créer le viewModel:
                var viewModel = _container.Resolve<T>(refWindow.ViewModelType);
                // invoke la méthode de chargement (optionnel) du ViewModel
                loadAction?.Invoke(viewModel);
                // résout la fenêtre et lui attribue un ViewModel en DataContext
                var window = _container.Resolve<Window>(refWindow.WindowsType);
                window.DataContext = viewModel;
                // s'abonne aux fermetures des fenêtres
                window.Closed += OnWindowClosed;
                // enfin, enregistre le mapping view/viewModel pour une éventuelle demande de fermeture
                _refWindowByViewModel.Add(viewModel, new WindowWrapper(name, window));
                if (refWindow.RegistrationMode == WindowRegistrationMode.TransientSingle)
                {
                    _alreadyCreatedSingleWindows.Add(name, window);
                }
                return window;
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            try
            {
                lock (_key)
                {
                    // sur fermeture de la fenêtre, on appelle la méthode OnClose du viewModel et on désabonne
                    var window = (Window)sender;
                    if (window.DataContext is IPopupWindowsViewModel viewModel)
                    {
                        // si on a pas déjà retiré le view model du dictionnaire de référence, on le fait et on désabonne
                        if (!_refWindowByViewModel.TryGetValue(viewModel, out var wrapper)) return;

                        _refWindowByViewModel.Remove(viewModel);
                        // retire éventuellement la fenêtre si elle est en mode TransientSingle
                        _alreadyCreatedSingleWindows.Remove(wrapper.Name);

                        // on désabonne
                        window.Closed -= OnWindowClosed;

                        // Appelle la méthode OnClose du viewModel
                        viewModel.OnClose();
                        // important: Memoryleak: détache manuellement le datacontext
                        window.DataContext = null;
                    }
                    else
                    {
                        throw new InvalidOperationException($"la fenêtre {window.GetType().Name} a un ViewModel qui n'est pas un {nameof(IPopupWindowsViewModel)}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex}");
            }
        }
        private sealed class WindowWrapper
        {
            public Window Window { get; }
            public TWindowKey Name { get; }

            public WindowWrapper(TWindowKey name, Window window)
            {
                Window = window;
                Name = name;
            }
        }

        private sealed class RegistrationWindowsReference
        {
            public Type ViewModelType { get; }
            public Type WindowsType { get; }
            public WindowRegistrationMode RegistrationMode { get; }

            public RegistrationWindowsReference(Type viewModelType, Type windowsType, WindowRegistrationMode registrationMode)
            {
                if (!windowsType.IsSubclassOf(typeof(Window)))
                {
                    throw new ArgumentException($"@Erreur: la fenêtre enregistrée du type '{windowsType}' ne dérive pas du type 'Window'");
                }
                ViewModelType = viewModelType;
                WindowsType = windowsType;
                RegistrationMode = registrationMode;
            }
        }
    }
}
