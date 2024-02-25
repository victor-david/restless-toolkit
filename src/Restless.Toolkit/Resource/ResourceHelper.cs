using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Restless.Toolkit.Resource
{
    /// <summary>
    /// Provides static methods to obtain resources.
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Gets the resource specified by <paramref name="resourceId"/> if it exists
        /// </summary>
        /// <typeparam name="T">The type of resource</typeparam>
        /// <param name="resourceId">The resource id</param>
        /// <returns>The resource as <typeparamref name="T"/>, or null if it doesn't exist</returns>
        public static T Get<T>(object resourceId) where T : class
        {
            return Application.Current.TryFindResource(resourceId) as T;
        }

        /// <summary>
        /// Gets the resource specified by <paramref name="resourceId"/> if it exists
        /// </summary>
        /// <param name="resourceId">The resource id</param>
        /// <returns>The resource as an object, or null if it doesn't exist.</returns>
        public static object Get(object resourceId)
        {
            return Application.Current.TryFindResource(resourceId);
        }

        /// <summary>
        /// Gets the style with key <see cref="ResourceKeys.DataGridCellEditTextBoxStyleKey"/>
        /// </summary>
        /// <param name="maxLength">The max length allowed in the TextBox. Pass zero to not set the max.</param>
        /// <returns>A Style for a TextBox</returns>
        public static Style CreateDataGridCellEditTextBoxStyle(int maxLength)
        {
            Style result = new Style(typeof(TextBox), Get<Style>(ResourceKeys.DataGridCellEditTextBoxStyleKey));
            if (maxLength > 0)
            {
                result.Setters.Add(new Setter(TextBox.MaxLengthProperty, maxLength));
            }
            return result;
        }

        /// <summary>
        /// Injects <see cref="ResourceDictionary"/> into the application
        /// that contains the default styles
        /// </summary>
        public static void InjectDefaultStyles()
        {
            ResourceDictionary res = Application.Current.Resources;
            ResourceDictionary styles = new ResourceDictionary();
            styles.BeginInit();
            styles.AddStyle(ResourceKeys.DefaultButtonStyleKey);
            styles.AddStyle(ResourceKeys.DefaultCheckBoxStyleKey);
            styles.AddStyle(ResourceKeys.DefaultComboBoxStyleKey);
            styles.AddStyle(ResourceKeys.DefaultDataGridStyleKey);
            styles.AddStyle(ResourceKeys.DefaultDataGridStyleKey, typeof(Controls.DataGrid));
            styles.AddStyle(ResourceKeys.DefaultListBoxItemStyleKey);
            styles.AddStyle(ResourceKeys.DefaultPathStyleKey);
            styles.AddStyle(ResourceKeys.DefaultStatusBarStyleKey);
            styles.AddStyle(ResourceKeys.DefaultSliderStyleKey);
            styles.AddStyle(ResourceKeys.DefaultTextBoxStyleKey);
            styles.EndInit();
            res.MergedDictionaries.Insert(0, styles);
        }

        private static void AddStyle(this ResourceDictionary res, ComponentResourceKey key, Type targetTypeOverride = null)
        {
            if (Get<Style>(key) is Style style)
            {
                Type targetType = targetTypeOverride ?? style.TargetType;
                Style newStyle = new Style(targetType, style);
                newStyle.Seal();
                res.Add(targetType, newStyle);
            }

        }

        internal static ComponentResourceKey CreateKey<T>([CallerMemberName] string resourceId = null)
        {
            return new ComponentResourceKey(typeof(T), resourceId);
        }
    }
}