using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using PRF.Utils.CoreComponents.Extensions;
using PRF.Utils.CoreComponents.IO;
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
        public static FileInfo? OpenFileBrowser(string filter, string title = "Choose File", string? initialDirectory = null)
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
        [Obsolete("this Open file browser use the WindowsForm legacy Dialog. Use the BrowserDialogManager.OpenFolderBrowser instead")]
        public static DirectoryInfo? OpenDirectoryBrowser(string description, string originalPath = "")
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = originalPath;
                fbd.Description = description;
                return fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath) && Directory.Exists(fbd.SelectedPath)
                    ? new DirectoryInfo(fbd.SelectedPath)
                    : null;
            }
        }

        /// <summary>
        /// Open a folder browser
        /// </summary>
        /// <param name="description">The browser description</param>
        /// <param name="originalPath">the initial path when opening the browser</param>
        public static IDirectoryInfo? OpenFolderBrowser(string description, string originalPath = "")
        {
            var folderDialog = new OpenFolderDialog
            {
                Title = description,
                InitialDirectory = originalPath,
            };
            if (folderDialog.ShowDialog() == true &&
                !string.IsNullOrWhiteSpace(folderDialog.FolderName) &&
                Directory.Exists(folderDialog.FolderName))
            {
                return new DirectoryInfoWrapper(folderDialog.FolderName);
            }

            return null;
        }

        /// <summary>
        /// Ouvre le dossier dans l'explorateur Windows
        /// </summary>
        public static void OpenFolderInExplorer(this DirectoryInfo? dir)
        {
            if (dir == null || !dir.ExistsExplicit()) return;
            using var _ = Process.Start(new ProcessStartInfo("explorer.exe", dir.FullName));
        }

        /// <summary>
        /// Ouvre le dossier dans l'explorateur Windows
        /// </summary>
        public static void OpenFolderInExplorer(this IDirectoryInfo? dir)
        {
            if (dir == null || !dir.ExistsExplicit) return;
            using var _ = Process.Start(new ProcessStartInfo("explorer.exe", dir.FullName));
        }

        /// <summary>
        /// Ouvre le fichier avec le visualiseur de fichier par défaut
        /// </summary>
        public static void OpenFileInExplorer(this FileInfo? file)
        {
            if (file == null || !file.ExistsExplicit()) return;
            using var _ = Process.Start(file.FullName);
        }

        /// <summary>
        /// Ouvre le fichier avec le visualiseur de fichier par défaut
        /// </summary>
        public static void OpenFileInExplorer(this IFileInfo? file)
        {
            if (file == null || !file.ExistsExplicit) return;
            using var _ = Process.Start(file.FullName);
        }
    }
}