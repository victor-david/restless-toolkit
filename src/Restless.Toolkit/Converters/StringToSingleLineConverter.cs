using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Restless.Toolkit.Converters
{
    /// <summary>
    /// Provides a converter that accepts a string value and returns a string value without line breaks.
    /// </summary>
    public class StringToSingleLineConverter : MarkupExtension, IValueConverter
    {
        #region Public methods
        /// <summary>
        /// Cleans up a string according to various options. At minumum, leading and ending white space is removed.
        /// </summary>
        /// <param name="value">The string to be cleaned.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns>The cleaned string</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string str)
            {
                return str.Trim().Replace(Environment.NewLine, ".");
            }
            return value;
        }

        /// <summary>
        /// This method is not used. It throws a <see cref="NotImplementedException"/>
        /// </summary>
        /// <param name="value">n/a</param>
        /// <param name="targetType">n/a</param>
        /// <param name="parameter">n/a</param>
        /// <param name="culture">n/a</param>
        /// <returns>n/a</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the object that is set as the value of the target property for this markup extension. 
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>This object.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        #endregion
    }
}
