namespace PRF.Utils.WPF.Navigation
{
    /// <summary>
    /// Interface qui signale une vue navigable. toutes les vues utilisant le système de navigation doivent l'implémenter
    /// </summary>
    public interface INavigableView
    {
        /// <summary>
        /// Méthode appelé lorsque l'on souhaite naviguer vers la page en cours.
        /// </summary>
        void NavigateToCurrentRequested();

        /// <summary>
        /// Méthode appelé lorsque l'on souhaite naviguer depuis la page en cours vers une autre page.
        /// </summary>
        void NavigateFromCurrentRequested();

        /// <summary>
        /// Méthode appelée lorsque l'on souhaite naviguer vers la même page alors que celle ci est déjà affichée.
        /// </summary>
        void NavigateToItSelfRequested();
    }
}
