using System;
using System.Windows;
using System.Windows.Markup;

namespace Restless.Toolkit.Resource
{
    /// <summary>
    /// Provides a markup extension that enables the merging of two styles.
    /// </summary>
    [MarkupExtensionReturnType(typeof(Style))]
    public class MergedStyle : MarkupExtension
    {
        #region Public properties
        /// <summary>
        /// Gets or sets the first style.
        /// </summary>
        public Style Style1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second style.
        /// </summary>
        public Style Style2
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets the value for the markup extension.
        /// </summary>
        /// <param name="serviceProvider">The provider</param>
        /// <returns>The merged style of <see cref="Style1"/> and <see cref="Style2"/>.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Style1 == null && Style2 == null)
            {
                return null;
            }

            if (Style1 == null)
            {
                return Style2;
            }

            if (Style2 == null)
            {
                return Style1;
            }

            Style newStyle = new Style(Style1.TargetType, Style1);
            MergeWithStyle(newStyle, Style2);
            return newStyle;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private static void MergeWithStyle(Style style, Style mergeStyle)
        {
            // Recursively merge with any Styles this Style might be BasedOn.
            if (mergeStyle.BasedOn != null)
            {
                MergeWithStyle(style, mergeStyle.BasedOn);
            }

            // Merge setters
            foreach (SetterBase setter in mergeStyle.Setters)
            {
                style.Setters.Add(setter);
            }

            // Merge triggers...
            foreach (TriggerBase trigger in mergeStyle.Triggers)
            {
                style.Triggers.Add(trigger);
            }
        }
        #endregion
    }
}