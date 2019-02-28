namespace PRF.Utils.WPF.PopupManager
{
    /// <summary>
    /// Interface qui identifie un viewModel de popup, chaque fenêtre de popup doit l'implémenter
    /// </summary>
    public interface IPopupWindowsViewModel
    {
        /// <summary>
        /// Méthode appelée lors de la fermeture de la fenêtre
        /// </summary>
        void OnClose();
    }
}
