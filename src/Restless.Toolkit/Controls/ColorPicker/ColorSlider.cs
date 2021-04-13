using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Extends <see cref="Slider"/> to provide a control used for color adjustments
    /// </summary>
    public class ColorSlider : Slider
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSlider"/> class.
        /// </summary>
        public ColorSlider()
        {
        }

        static ColorSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSlider), new FrameworkPropertyMetadata(typeof(ColorSlider)));
        }
        #endregion

        /************************************************************************/

        #region Public constant values
        /// <summary>
        /// Gets the minimum allowed value of <see cref="SliderSize"/>.
        /// </summary>
        public const double MinSliderSize = 12;
        /// <summary>
        /// Gets the maximum allowed value of <see cref="SliderSize"/>.
        /// </summary>
        public const double MaxSliderSize = 58;
        /// <summary>
        /// Gets the default value of <see cref="SliderSize"/>.
        /// </summary>
        public const double DefaultSliderSize = 22;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the dimension (width or height, depending on layout) of the thumb.
        /// </summary>
        public double SliderSize
        {
            get => (double)GetValue(SliderSizeProperty);
            set => SetValue(SliderSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SliderSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SliderSizeProperty = DependencyProperty.Register
            (
                nameof(SliderSize), typeof(double), typeof(ColorSlider), new PropertyMetadata()
                {
                    DefaultValue = DefaultSliderSize,
                    CoerceValueCallback = OnCoerceSliderSize,
                }
            );

        private static object OnCoerceSliderSize(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Min(Math.Max(value, MinSliderSize), MaxSliderSize);
        }

        /// <summary>
        /// Gets or sets the brush to use on the thumb.
        /// </summary>
        public Brush ThumbBrush
        {
            get => (Brush)GetValue(ThumbBrushProperty);
            set => SetValue(ThumbBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ThumbBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ThumbBrushProperty = DependencyProperty.Register
            (
                nameof(ThumbBrush), typeof(Brush), typeof(ColorSlider), new PropertyMetadata()
                {
                    DefaultValue = Brushes.Blue
                }
            );

        /// <summary>
        /// Gets or sets the secondary background brush.
        /// </summary>
        public LinearGradientBrush PanelBrush
        {
            get => (LinearGradientBrush)GetValue(PanelBrushProperty);
            set => SetValue(PanelBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="PanelBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PanelBrushProperty = DependencyProperty.Register
            (
                nameof(PanelBrush), typeof(LinearGradientBrush), typeof(ColorSlider), new PropertyMetadata()
            );
        #endregion
    }
}