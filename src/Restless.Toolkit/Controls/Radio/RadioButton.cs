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
        /// Gets the template style
        /// </summary>
        public RadioButtonTemplateStyle TemplateStyle
        {
            get => (RadioButtonTemplateStyle)GetValue(TemplateStyleProperty);
            internal set => SetValue(TemplateStylePropertyKey, value);
        }

        private static readonly DependencyPropertyKey TemplateStylePropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(TemplateStyle), typeof(RadioButtonTemplateStyle), typeof(RadioButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = RadioButtonTemplateStyle.Default
                }
            );

        /// <summary>
        /// Identifies the <see cref="TemplateStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TemplateStyleProperty = TemplateStylePropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region ButtonRadius
        /// <summary>
        /// Gets the corner radius to use when <see cref="TemplateStyle"/> is button.
        /// </summary>
        public CornerRadius ButtonRadius
        {
            get => (CornerRadius)GetValue(ButtonRadiusProperty);
            internal set => SetValue(ButtonRadiusPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ButtonRadiusPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ButtonRadius), typeof(CornerRadius), typeof(RadioButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new CornerRadius(RadioButtonPanel.DefaultCornerRadius)
                }
            );

        /// <summary>
        /// Identifies the <see cref="ButtonRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonRadiusProperty = ButtonRadiusPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region UnderlineHeight
        /// <summary>
        /// Gets the height of the underline when <see cref="TemplateStyle"/> is underline.
        /// </summary>
        public double UnderlineHeight
        {
            get => (double)GetValue(UnderlineHeightProperty);
            internal set => SetValue(UnderlineHeightPropertyKey, value);
        }

        private static readonly DependencyPropertyKey UnderlineHeightPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(UnderlineHeight), typeof(double), typeof(RadioButton), new PropertyMetadata()
                {
                    DefaultValue = RadioButtonPanel.DefaultUnderlineHeight,
                }
            );

        /// <summary>
        /// Identifies the <see cref="UnderlineHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UnderlineHeightProperty = UnderlineHeightPropertyKey.DependencyProperty;
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