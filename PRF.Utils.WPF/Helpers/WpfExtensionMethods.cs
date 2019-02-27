using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace PRF.Utils.WPF.Helpers
{
    /// <summary>
    /// Regroupe les extensions liées à la manipulation de contrôle WPF
    /// </summary>
    public static class WpfExtensionMethods
    {
        /// <summary>
        /// Récupère la fenêtre parente d'un DependencyObject
        /// </summary>
        public static Window GetWindowsParent(this DependencyObject canvas)
        {
            return RecursiveFindParent<Window>(canvas);
        }

        /// <summary>
        /// Recursively try to find the first parent of type T in the hierarchy.
        /// This method combines Logical et and Visual tree search in order to be sure to find the target even if the initial object is not a visual object.
        /// </summary>
        /// <param name="d">Initial object</param>
        /// <typeparam name="T">Type of parent to find</typeparam>
        /// <returns>The parent dependency object or null</returns>
        public static T RecursiveFindParent<T>(this DependencyObject d) where T : class
        {
            var current = d;
            var result = d;

            while (current != null)
            {

                result = current;
                if (result is T)
                {
                    break;
                }
                if (current is Visual || current is Visual3D)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
                else
                {
                    // If we're in Logical Land then we must walk 
                    // up the logical tree until we find a 
                    // Visual/Visual3D to get us back to Visual Land.
                    current = LogicalTreeHelper.GetParent(current);
                }


            }

            return result as T;
        }


        /// <summary>
        /// Find all children of the a given type for a target parent control. The look up is made both in visual and logical tree.
        /// </summary>
        /// <param name="source">Source control</param>
        /// <typeparam name="T">Type of children we are looking for</typeparam>
        /// <returns>List of all recursively found children of the given type</returns>
        public static IEnumerable<T> FindChildren<T>(this DependencyObject source) where T : DependencyObject
        {
            if (source == null) yield break;

            var childs = GetChildObjects(source);
            foreach (var child in childs)
            {
                //analyze if children match the requested type
                if (child is T variable)
                {
                    yield return variable;
                }

                //recurse tree
                foreach (var descendant in FindChildren<T>(child))
                {
                    yield return descendant;
                }
            }
        }


        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetChild"/> method, which also
        /// supports content elements. Do note, that for content elements,
        /// this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="parent">The item to be processed.</param>
        /// <returns>The submitted item's child elements, if available.</returns>
        private static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject parent)
        {
            switch (parent)
            {
                case null:
                    yield break;
                case ContentElement _:
                case FrameworkElement _:
                {
                    //use the logical tree for content / framework elements
                    foreach (var obj in LogicalTreeHelper.GetChildren(parent))
                    {
                        if (obj is DependencyObject depObj)
                        {
                            yield return depObj;
                        }
                    }

                    break;
                }
                default:
                {
                    //use the visual tree per default
                    var count = VisualTreeHelper.GetChildrenCount(parent);
                    for (var i = 0; i < count; i++)
                    {
                        yield return VisualTreeHelper.GetChild(parent, i);
                    }

                    break;
                }
            }
        }
    }


}
