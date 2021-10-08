using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;

namespace PRF.Utils.WPF.BootStrappers
{
    /// <summary>
    /// Bootstrapper de base pour les IHM, les type de la vue et de son view model SERONT ENREGISTRÉS EN SINGLETON
    /// </summary>
    /// <example>
    /// Pour l'utiliser, il faut:
    ///  - supprimer la balise 'StartUri' de l'App.xaml de votre application
    ///  - faire une implémentation de BootStrapperPresentation
    ///  - créer une classe 'Startup.cs' qui ressemble à ça:
    /// <code>
    ///     static class Startup
    ///     {
    ///         [STAThread]
    ///         static void Main()
    ///         {
    ///         try
    ///         {
    ///             var container = new MiscWpfBoot();
    ///             var app = new App();
    ///
    ///             app.Exit += container.OnExit;
    ///             app.InitializeComponent();
    ///             app.Run(container.Run());
    ///         }
    ///         catch (Exception ex)
    ///         {
    ///             // gère exception, log, etc...
    ///         }
    ///     }
    /// </code>
    ///  - et enfin la mettre comme classe de démarrage dans les propriétés du projet
    /// </example>
    /// <typeparam name="TMainWindow">le type de la fenetre principale</typeparam>
    /// <typeparam name="TMainViewModel">le type du viewModel lié à la fenetre principale</typeparam>
    public abstract class BootStrapperPresentation<TMainWindow, TMainViewModel>
        where TMainWindow : Window
        where TMainViewModel : NotifierBase
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly InjectionContainerSimpleInjector _container = new InjectionContainerSimpleInjector();
        private readonly object _key = new object();
        private bool _hasbeenRun;

        /// <summary>
        /// Démarre le bootStrapper en créant un nouveau container d'injection
        /// </summary>
        public TMainWindow Run()
        {
            lock (_key)
            {
                if (_hasbeenRun)
                {
                    return _container.Resolve<TMainWindow>();
                }
                _hasbeenRun = true;

                _container.ResolveUnregisteredType += ResolveUnregisteredType;

                // enregistre cette instance en accès statique
                ContainerHolder.Container = _container;

                //enregistre la vue, le viewModel et le traceur en singleton:
                _container.RegisterType<TMainWindow>(LifeTime.Singleton);
                _container.RegisterType<TMainViewModel>(LifeTime.Singleton);

                //fait les enregistrements des autres composants
                Register(_container);

                // on initialise les composants
                Initialize(_container);

                // Il peut y avoir des données à charger au démarrage, on s'en occupe maintenant que les initializations sont terminées.
                // ces données seront chargée en arrière plan
                LoadingTask = LoadDataAsync(_cts.Token, GetLoaderMethods(_container));

                // et enfin la vue principale
                return _container.Resolve<TMainWindow>();
            }
        }

        /// <summary>
        /// Do a full Verification of the container by reslving every registered type. Do this only in UnitTest
        /// </summary>
        public string Verify()
        {
            Run();
            return _container.Verify();
        }

        /// <summary>
        /// Méthode appelée lors d'une demande de résolution sur un type non enregistré
        /// </summary>
        protected abstract void ResolveUnregisteredType(object sender, Type type);

        /// <summary>
        /// Liste des méthodes async à lancer en arrière plan au démarrage (après le register et en parallèle de l'affichage de la fenêtre principale)
        ///  => pour les résolutions synchrones, privilégiez le Initialize().
        /// </summary>
        /// <returns>la liste de méthodes (async évidemment)</returns>
        protected virtual IEnumerable<Func<CancellationToken, Task>> GetLoaderMethods(IInjectionContainer container)
        {
            return new Func<CancellationToken, Task>[0];
        }

        /// <summary>
        /// tache de chargement des données. à la fin du programme, on peut éventuellement annuler ce chargement. Dans tt les cas, 
        /// il faut attendre cette tache, au moins pour récupérer les éventuelles exception qu'elle aurait stockée
        /// </summary>
        private Task LoadingTask { get; set; }

        private async Task LoadDataAsync(CancellationToken ctsToken, IEnumerable<Func<CancellationToken, Task>> loadingAsyncMethods)
        {
            try
            {
                await Task.WhenAll(loadingAsyncMethods.Select(async t => await t.Invoke(ctsToken).ConfigureAwait(false))).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (!ManageExceptionInLoading(e))
                {
                    throw;
                }
            }
            finally
            {
                // dispose le CancellationTokenSource à la fin du traitement il doit être disposé.
                _cts.Dispose();
            }
        }

        /// <summary>
        /// Méthode de gestion des exceptions de chargement de données au démarrage
        /// </summary>
        /// <param name="exception">exceptions</param>
        /// <returns>si l'on souhaite relancer l'exception (attention car dans ce cas, c'est la tache d'initialisation qui la stockera)</returns>
        protected virtual bool ManageExceptionInLoading(Exception exception)
        {
            MessageBox.Show($"Critical error in loading background data: {exception}");
            return true; // pas de rethrow par défaut au cas où personne n'attends la LoadingTask qui stockera l'exception
        }

        /// <summary>
        /// résout certains composant essentiels au fonctionnement de l'IHM
        /// </summary>
        /// <param name="container">le container d'injection</param>
        protected virtual void Initialize(IInjectionContainer container) { }

        /// <summary>
        /// Enregistre les composants dans le container d'injection
        /// </summary>
        /// <param name="container">le container d'injection</param>
        protected abstract void Register(IInjectionContainerRegister container);

        /// <summary>
        /// Wait for any loading task on Exit
        /// </summary>
        public virtual void OnExit(object sender, ExitEventArgs e)
        {
            try
            {
                if (!LoadingTask.IsCompleted)
                {
                    _cts?.Cancel();
                    // le cts sera disposé dans le finally du chargement

                    // la Loading Task gère les exception, donc l'attente ici ne devrait pas en générer
                    LoadingTask.Wait(TimeSpan.FromSeconds(5));
                }
            }
            finally
            {
                if (_container != null)
                {
                    _container.ResolveUnregisteredType -= ResolveUnregisteredType;
                    //Les objets Disposables enregistrés dans le container sont diposés à la fermeture de l'application:
                    _container.Dispose();
                }
            }
        }
    }
}
