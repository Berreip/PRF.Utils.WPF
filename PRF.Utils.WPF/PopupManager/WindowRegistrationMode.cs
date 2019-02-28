namespace PRF.Utils.WPF.PopupManager
{
    /// <summary>
    /// Les modes d'enregistrement d'une fenêtre
    /// </summary>
    public enum WindowRegistrationMode
    {
        /// <summary>
        /// Enregistrement où chaque appel renvoie une nouvelle fenêtre différente
        /// </summary>
        Transient,

        /// <summary>
        /// Enregistrement où chaque appel renvoie la fenêtre ouverte si une fenêtre de ce type existe ET SINON, en créer une nouvelle instance
        /// </summary>
        TransientSingle,
    }
}
