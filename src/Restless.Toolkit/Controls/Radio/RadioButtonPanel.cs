using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides a panel to hold <see cref="RadioButton"/> objects
    /// and enable one central binding for values assigned to each.
    /// </summary>
    public class RadioButtonPanel : Grid
    {
        #region Public fields
        /// <summary>
        /// Gets the minimum value for <see cref="UnderlineHeight"/>.
        /// </summary>
        public const double MinUnderlineHeight = 1.0;

        /// <summary>
        /// Gets the maximum value for <see cref="UnderlineHeight"/>.
        /// </summary>
        public const double MaxUnderlineHeight = 16.0;

        /// <summary>
        /// Gets the default value for <see cref="UnderlineHeight"/>.
        /// </summary>
        public const double DefaultUnderlineHeight = 3.0;

        /// <summary>
        /// Gets the default uniform value for <see cref="ButtonRadius"/>
        /// </summary>
        public const double DefaultCornerRadius = 0.0;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButtonPanel"/> class.
        /// </summary>
        public RadioButtonPanel()
        {
            AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(CheckedEventHandler));
        }
        #endregion

        /************************************************************************/

        #region SelectedValue
        /// <summary>
        /// Gets or sets the selected value
        /// </summary>
        public int SelectedValue
        {
            get => (int)GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }
        
        /// <summary>
        /// Identifies the <see cref="SelectedValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty =  DependencyProperty.Register
            (
                nameof(SelectedValue), typeof(int), typeof(RadioButtonPanel), new FrameworkPropertyMetadata()
                {
                    DefaultValue = 0,
                    PropertyChangedCallback = OnSelectedValueChanged,
                    BindsTwoWayByDefault = true,
                }
            );

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RadioButtonPanel)?.UpdateSelectedChild();
        }
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
                nameof(TemplateStyle), typeof(RadioButtonTemplateStyle), typeof(RadioButtonPanel), new FrameworkPropertyMetadata()
                {
                    DefaultValue = RadioButtonTemplateStyle.Default,
                    PropertyChangedCallback = OnTemplatePropertyChanged
                }
            );
        #endregion

        /************************************************************************/

        #region ButtonRadius
        /// <summary>
        /// Gets or sets the corner radius to use when <see cref="TemplateStyle"/> is button.
        /// </summary>
        public CornerRadius ButtonRadius
        {
            get => (CornerRadius)GetValue(ButtonRadiusProperty);
            set => SetValue(ButtonRadiusProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ButtonRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonRadiusProperty = DependencyProperty.Register
            (
                nameof(ButtonRadius), typeof(CornerRadius), typeof(RadioButtonPanel), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new CornerRadius(DefaultCornerRadius),
                    PropertyChangedCallback = OnTemplatePropertyChanged
                }
            );
        #endregion

        /************************************************************************/

        #region UnderlineHeight
        /// <summary>
        /// Gets or sets the underline height when <see cref="TemplateStyle"/> is underline.
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
                nameof(UnderlineHeight), typeof(double), typeof(RadioButtonPanel), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultUnderlineHeight,
                    CoerceValueCallback = OnCoerceUnderlineHeight,
                    PropertyChangedCallback = OnTemplatePropertyChanged
                }
            );

        private static object OnCoerceUnderlineHeight(DependencyObject d, object baseValue)
        {
            return baseValue is double doubleValue ? Math.Max(Math.Min(MaxUnderlineHeight, doubleValue), MinUnderlineHeight) : baseValue;
        }

        private static void OnTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RadioButtonPanel)?.UpdateChildTemplateProperties();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Called when the initialization process is complete
        /// </summary>
        public override void EndInit()
        {
            UpdateSelectedChild();
            UpdateChildTemplateProperties();
            base.EndInit();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void CheckedEventHandler(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is RadioButton button)
            {
                SelectedValue = button.Value;
                e.Handled = true;
            }
        }

        private void UpdateChildTemplateProperties()
        {
            foreach (RadioButton child in Children.OfType<RadioButton>())
            {
                child.TemplateStyle = TemplateStyle;
                child.ButtonRadius = ButtonRadius;
                child.UnderlineHeight = UnderlineHeight;
            }
        }

        private void UpdateSelectedChild()
        {
            foreach (RadioButton child in Children.OfType<RadioButton>())
            {
                child.IsChecked = child.Value == SelectedValue;
            }
        }
        #endregion
    }
}