using System.Windows.Input;

namespace PRF.Utils.WPF.Commands
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable InheritdocConsiderUsage
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable ClassNeverInstantiated.Global

    /// <summary>
    /// Interface des commandes WPF version light (l'appel des RaiseCanExecute est manuel)
    /// </summary>
    public interface IDelegateCommandLight : ICommand
    {
        /// <summary>
        /// Demande une réévaluation de la commande et dispatche dans le thread UI (en asynchrone) si l'on n'est pas déjà dedans.
        /// </summary>
        void RaiseCanExecuteChanged();

        /// <summary>
        /// Exécute la commande
        /// </summary>
        void Execute();

        /// <summary>
        /// Défini la condition d'éxécution de la commande
        /// </summary>
        /// <returns>Renvoi true si la commande peut s'éxécuter, false sinon</returns>
        bool CanExecute();
    }
}