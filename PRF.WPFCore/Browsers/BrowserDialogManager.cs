using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using PRF.Utils.CoreComponents.Extensions;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.WPFCore.Browsers
{
    /// <summary>
    /// Classe de gestion des fenêtres de choix de fichiers et dossiers
    /// </summary>
    public static class BrowserDialogManager
    {
        /// <summary>
        /// Ouvre une fenêtre de choix de fichier et renvoie null si l'on a pas de fichier choisi 
        /// </summary>
        /// <param name="filter">le filtre sur le choix de fichier</param>
        /// <param name="title">le titre de la fenetre ('Choose File' par défaut)</param>
        /// <param name="initialDirectory">le dossier initial à ouvrir (par défaut, il s'agit du dernier dossier ouvert)</param>
        /// <returns>le fichier choisi ou null si aucun choix</returns>
        public static FileInfo OpenFileBrowser(string filter, string title = "Choose File", string initialDirectory = null)
        {
            var ofd = initialDirectory != null
                ? new OpenFileDialog
                {
                    Filter = filter,
                    Title = title,
                    InitialDirectory = initialDirectory,
                    Multiselect = false
                }
                : new OpenFileDialog
                {
                    Filter = filter,
                    Title = title,
                    Multiselect = false
                };

            return ofd.ShowDialog() == true && File.Exists(ofd.FileName)
                ? new FileInfo(ofd.FileName)
                : null;
        }

        /// <summary>
        /// Ouvre une fenêtre de choix de dossier et renvoie null si l'on a pas de dossier choisi 
        /// </summary>
        /// <param name="description">la description de la fenetre </param>
        /// <param name="originalPath">le chemin source</param>
        /// <returns></returns>
        public static DirectoryInfo OpenDirectoryBrowser(string description, string originalPath = "")
        {
            using (var fbd = new FolderBrowserDialog { SelectedPath = originalPath, Description = description })
            {
                return fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath) && Directory.Exists(fbd.SelectedPath)
                    ? new DirectoryInfo(fbd.SelectedPath)
                    : null;
            }
        }

        /// <summary>
        /// Ouvre le dossier dans l'explorateur Windows
        /// </summary>
        public static void OpenFolderInExplorer(this DirectoryInfo dir)
        {
            if (dir == null || !dir.ExistsExplicit()) return;
            using var _ = Process.Start(new ProcessStartInfo("explorer.exe", dir.FullName));
        }

        /// <summary>
        /// Ouvre le fichier avec le visualiseur de fichier par défaut
        /// </summary>
        public static void OpenFileInExplorer(this FileInfo file)
        {
            if (file == null || !file.ExistsExplicit()) return;
            using var _ = Process.Start(file.FullName);
        }
    }
}
