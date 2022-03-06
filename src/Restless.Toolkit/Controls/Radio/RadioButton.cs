using System;
using System.Windows;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a custom radio button. See <see cref="RadioButtonPanel"/> also.
    /// </summary>
    public class RadioButton : System.Windows.Controls.RadioButton
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        public RadioButton()
        {
        }

        static RadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButton), new FrameworkPropertyMetadata(typeof(RadioButton)));
        }
        #endregion

        /************************************************************************/

        #region Value
        /// <summary>
        /// Gets or sets an abitrary integer value to associate with the <see cref="RadioButton"/>
        /// </summary>
        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
            (
                nameof(Value), typeof(int), typeof(RadioButton), new PropertyMetadata(0)
            );

        #endregion

        /************************************************************************/

        #region TemplateStyle
        /// <summary>
        /// Gets or sets the template style
        /// </summary>
        public RadioButtonTemplateStyle TemplateStyle
        {
            get => (RadioButtonTemplateStyle)GetValue(TemplateStyleProperty);
            set => SetValue(TemplateStyleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TemplateStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TemplateStyleProperty = DependencyProperty.Register
            (
                nameof(TemplateStyle), typeof(RadioButtonTemplateStyle), typeof(RadioButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = RadioButtonTemplateStyle.Default
                }
            );
        #endregion


        /************************************************************************/

        #region UnderlineHeight
        /// <summary>
        /// Gets or sets a value that determines the height of the underline when <see cref="TemplateStyle"/> is underline.
        /// </summary>
        public double UnderlineHeight
        {
            get => (double)GetValue(UnderlineHeightProperty);
            set => SetValue(UnderlineHeightProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="UnderlineHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UnderlineHeightProperty = DependencyProperty.Register
            (
                nameof(UnderlineHeight), typeof(double), typeof(RadioButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = 2.0,
                    CoerceValueCallback = OnCoerceUnderlineHeight
                }
            );

        private static object OnCoerceUnderlineHeight(DependencyObject d, object baseValue)
        {
            return baseValue is double doubleValue ? Math.Max(Math.Min(16, doubleValue), 1.0) : baseValue;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets a string representation of this object.
        /// </summary>
        /// <returns>A descriptive string.</returns>
        public override string ToString()
        {
            return $"{nameof(RadioButton)} Value: {Value} IsChecked: {IsChecked}";
        }
        #endregion
    }
}