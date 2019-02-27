using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Controls;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.Utils.WPF.ValidationRules
{
    /// <summary>
    /// Règle de validation pour un string en tant que nom de fichier (non null ni vide et sans
    /// charactères invalides)
    /// </summary>
    public class StringValidForFileNamValidationRule : ValidationRule
    {
        /// <summary>
        /// Valide la donnée s'il s'agit d'un string non vide, non null et qui ne contient pas de
        /// charactères invalides pour un nom de fichier
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value?.ToString();
            return string.IsNullOrWhiteSpace(str)
                ? new ValidationResult(false, "this field could not be empty")
                : Path.GetInvalidFileNameChars().Any(str.Contains)
                    ? new ValidationResult(false, "field contains invalid file name chars")
                    : new ValidationResult(true, null);
        }
    }
}
