using System;
using System.Windows;

namespace Restless.Toolkit.Utility
{
    /// <summary>
    /// Provides static utility methods to display messages.
    /// </summary>
    [Obsolete("Use MessageWindow instead")]
    public static class Messages
    {
        /// <summary>
        /// Displays a dialog box with Yes / No buttons.
        /// </summary>
        /// <param name="message">The message to display in the dialog box.</param>
        /// <param name="caption">The caption, or null to use the default</param>
        /// <returns>true if Yes if selected; otherwise, false.</returns>
        public static bool ShowYesNo(string message, string caption = null)
        {
            caption = string.IsNullOrWhiteSpace(caption) ? "Confirm" : caption;
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return (result == MessageBoxResult.Yes);
        }

        /// <summary>
        /// Display a dialog box with an error icon.
        /// </summary>
        /// <param name="message">The message to display in the dialog box.</param>
        /// <param name="caption">The caption, or null to use the default</param>
        public static void ShowError(string message, string caption = null)
        {
            caption = string.IsNullOrWhiteSpace(caption) ? "Operation Not Available" : caption;
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Display a dialog box with an information icon.
        /// </summary>
        /// <param name="message">The message to display in the dialog box.</param>
        /// <param name="caption">The caption, or null to use the default</param>
        public static void Show(string message, string caption = null)
        {
            caption = string.IsNullOrWhiteSpace(caption) ? "Information" : caption;
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
