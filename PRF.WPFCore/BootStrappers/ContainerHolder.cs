using PRF.Utils.Injection.Containers;

namespace PRF.WPFCore.BootStrappers
{
    /// <summary>
    /// Classe static qui va juste servir à garder une référence statique vers le conteneur d'injection afin de pouvoir y accéder dans certains éléments 
    /// où l'on ne peut pas l'injecter
    /// </summary>
    public static class ContainerHolder
    {
        /// <summary>
        /// Stockage du container en static: pas terrible mais idispensable pour certaines techniques (dependency properties, ...)
        /// </summary>
        public static IInjectionContainer Container { get; set; }
    }
}
