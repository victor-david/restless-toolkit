using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides a composite control for color selection.
    /// </summary>
    [TemplatePart(Name = PartMainPicker, Type = typeof(MainPicker))]
    [TemplatePart(Name = PartAlphaPicker, Type = typeof(ColorSlider))]
    public class ColorPicker : Control
    {
        #region Private
        private const string PartMainPicker = "PART_MainPicker";
        private const string PartAlphaPicker = "PART_AlphaPicker";
        private const double WidthHeightRatio = 1.0 / 1.618;
        private MainPicker mainPicker;
        private ColorSlider alphaPicker;
        private ColorUpdateOrign selectedColorUpdateOrign;
        #endregion

        /************************************************************************/

        #region Private helper enum
        private enum ColorUpdateOrign
        {
            /// <summary>
            /// Color is being upated via the control consumer.
            /// </summary>
            Consumer,
            /// <summary>
            /// Color is being updated from within the control itself.
            /// </summary>
            ControlInternal
        }
        #endregion

        /************************************************************************/

        #region Public constant values
        /// <summary>
        /// Gets the minimum allowed value of <see cref="CanvasWidth"/>.
        /// </summary>
        public const double MinCanvasWidth = 112;
        /// <summary>
        /// Gets the maximum allowed value of <see cref="CanvasWidth"/>.
        /// </summary>
        public const double MaxCanvasWidth = 250;
        /// <summary>
        /// Gets the default value of <see cref="CanvasWidth"/>.
        /// </summary>
        public const double DefaultCanvasWidth = 192;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker()
        {
            Loaded += ColorPickerLoaded;
        }

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the margin for the hue and alpha pickers.
        /// </summary>
        public Thickness PickerMargin
        {
            get => (Thickness)GetValue(PickerMarginProperty);
            set => SetValue(PickerMarginProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="PickerMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PickerMarginProperty = DependencyProperty.Register
            (
                nameof(PickerMargin), typeof(Thickness), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = new Thickness(2)
                }
            );

        /// <summary>
        /// Gets or sets the dimension (both width and height) for the main picker canvas
        /// </summary>
        public double CanvasWidth
        {
            get => (double)GetValue(CanvasWidthProperty);
            set => SetValue(CanvasWidthProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CanvasWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanvasWidthProperty = DependencyProperty.Register
            (
                nameof(CanvasWidth), typeof(double), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = DefaultCanvasWidth,
                    CoerceValueCallback = OnCoerceCanvasWidth,
                    PropertyChangedCallback = OnCanvasWidthPropertyChanged
                }
            );

        private static object OnCoerceCanvasWidth(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Min(Math.Max(value, MinCanvasWidth), MaxCanvasWidth);
        }

        private static void OnCanvasWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker control)
            {
                control.CanvasHeight = control.CanvasWidth * WidthHeightRatio;
                control.DemoCanvasWidth = control.CanvasWidth / 2.0;
            }
        }

        /// <summary>
        /// Gets the canvas width
        /// </summary>
        public double CanvasHeight
        {
            get => (double)GetValue(CanvasWidthProperty);
            private set => SetValue(CanvasHeightPropertyKey, value);
        }

        private static readonly DependencyPropertyKey CanvasHeightPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(CanvasHeight), typeof(double), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = DefaultCanvasWidth * WidthHeightRatio
                }
            );

        /// <summary>
        /// Identifies the <see cref="CanvasHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanvasHeightProperty = CanvasHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the height of the demo canvas.
        /// </summary>
        public double DemoCanvasWidth
        {
            get => (double)GetValue(DemoCanvasWidthProperty);
            private set => SetValue(DemoCanvasWidthPropertyKey, value);
        }

        private static readonly DependencyPropertyKey DemoCanvasWidthPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(DemoCanvasWidth), typeof(double), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = DefaultCanvasWidth / 2.0,
                    PropertyChangedCallback = OnDemoCanvasWidthPropertyChanged
                }
            );

        /// <summary>
        /// Identifies the <see cref="DemoCanvasWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DemoCanvasWidthProperty = DemoCanvasWidthPropertyKey.DependencyProperty;

        private static void OnDemoCanvasWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker control)
            {
                control.DemoCanvasHeight = control.DemoCanvasWidth * WidthHeightRatio;
            }
        }

        /// <summary>
        /// Gets the height of the demo canvas
        /// </summary>
        public double DemoCanvasHeight
        {
            get => (double)GetValue(DemoCanvasHeightProperty);
            private set => SetValue(DemoCanvasHeightPropertyKey, value);
        }

        private static readonly DependencyPropertyKey DemoCanvasHeightPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(DemoCanvasHeight), typeof(double), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = DefaultCanvasWidth / 2.0 * WidthHeightRatio
                }
            );

        /// <summary>
        /// Identifies the <see cref="DemoCanvasHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DemoCanvasHeightProperty = DemoCanvasHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets the slider size.
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
                nameof(SliderSize), typeof(double), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = ColorSlider.DefaultSliderSize,
                    CoerceValueCallback = OnCoerceSliderSize,
                }
            );

        private static object OnCoerceSliderSize(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Min(Math.Max(value, ColorSlider.MinSliderSize), ColorSlider.MaxSliderSize);
        }

        /// <summary>
        /// From this assembly, gets or sets the alpha value.
        /// </summary>
        internal double Alpha
        {
            get => (double)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Alpha"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty AlphaProperty = DependencyProperty.Register
            (
                nameof(Alpha), typeof(double), typeof(ColorPicker), new FrameworkPropertyMetadata()
                {
                    DefaultValue = ColorValues.MaxAlpha,
                    CoerceValueCallback = OnCoerceAlphaProperty,
                    PropertyChangedCallback = OnAlphaPropertyChanged
                }
            );

        private static object OnCoerceAlphaProperty(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Min(Math.Max(value, ColorValues.MinAlpha), ColorValues.MaxAlpha);
        }

        private static void OnAlphaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ColorPicker)?.ApplyAlphaToSelectedColor();
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectedColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register
            (
                nameof(SelectedColor), typeof(Color), typeof(ColorPicker), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Colors.Red,
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnSelectedColorPropertyChanged,
                }
            );

        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ColorPicker)?.OnSelectedColorChanged();
        }

        /// <summary>
        /// Gets the brush used to display the selected color
        /// </summary>
        public SolidColorBrush SelectedColorBrush
        {
            get => (SolidColorBrush)GetValue(SelectedColorBrushProperty);
            private set => SetValue(SelectedColorBrushPropertyKey, value);
        }

        private static readonly DependencyPropertyKey SelectedColorBrushPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(SelectedColorBrush), typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata()
                {
                }
            );

        /// <summary>
        /// Identifies the <see cref="SelectedColorBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorBrushProperty = SelectedColorBrushPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (mainPicker != null) mainPicker.ColorComponentChanged -= MainPickerColorComponentChanged;

            mainPicker = GetTemplateChild(PartMainPicker) as MainPicker;
            alphaPicker = GetTemplateChild(PartAlphaPicker) as ColorSlider;

            if (mainPicker == null || alphaPicker == null)
            {
                throw new NotImplementedException("Control template not implemented correctly");
            }

            mainPicker.ColorComponentChanged += MainPickerColorComponentChanged;

            InitializeSelectedColorBrush();
            InitializeAlphaPanelBrush();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void ColorPickerLoaded(object sender, RoutedEventArgs e)
        {
            SyncToMainPicker();
        }

        private void OnSelectedColorChanged()
        {
            SyncAlpa();
            UpdateAlphaPanelBrush();
            UpdateSelectedColorBrush();
            SyncToMainPicker();
        }

        private void MainPickerColorComponentChanged(object sender, ColorComponents e)
        {
            selectedColorUpdateOrign = ColorUpdateOrign.ControlInternal;
            SelectedColor = ColorHelper.ColorFromHSB(e.Hue, e.Saturation, e.Brightness);
            selectedColorUpdateOrign = ColorUpdateOrign.Consumer;
        }

        private void UpdateSelectedColorBrush()
        {
            SelectedColorBrush.Color = SelectedColor;
        }

        private void SyncToMainPicker()
        {
            if (selectedColorUpdateOrign == ColorUpdateOrign.Consumer)
            {
                mainPicker?.UpdateColorComponents(SelectedColor);
            }
        }

        /// <summary>
        /// Synchonizes the alpha value by either applying it to the selected color,
        /// or taking the alpha value from the selected color and applying it
        /// to the <see cref="Alpha"/> property, depending on the origin of the color change.
        /// </summary>
        private void SyncAlpa()
        {
            if (selectedColorUpdateOrign == ColorUpdateOrign.ControlInternal)
            {
                ApplyAlphaToSelectedColor();
            }
            else
            {
                Alpha = SelectedColor.A;
            }
        }

        private void ApplyAlphaToSelectedColor()
        {
            SelectedColor = SelectedColor.ToSpecifiedAlpha((byte)Alpha);
        }

        /// <summary>
        /// Updates the alpha picker's PanelBrush using the value of <see cref="SelectedColor"/> 
        /// (with full opacity) as the brush's gradient stop.
        /// </summary>
        private void UpdateAlphaPanelBrush()
        {
            if (alphaPicker != null && alphaPicker.PanelBrush != null)
            {
                alphaPicker.PanelBrush.GradientStops[1].Color = SelectedColor.ToMaxAlpha();
            }
        }

        /// <summary>
        /// Initializes the <see cref="SelectedColorBrush"/> property.
        /// </summary>
        /// <remarks>
        /// This method is called from <see cref="OnApplyTemplate"/> to set the initial
        /// value of the selected color brush. The brush is not given a default value
        /// in the dependency property declaration because that causes the brush to be
        /// frozen. When it's time to update the brush, we want to just update its Color.
        /// </remarks>
        private void InitializeSelectedColorBrush()
        {
            SelectedColorBrush = new SolidColorBrush(Colors.Red);
        }

        /// <summary>
        /// Initialzes the alpha picker's PanelBrush.
        /// </summary>
        /// <remarks>
        /// Same idea as <see cref="InitializeSelectedColorBrush"/>
        /// </remarks>
        private void InitializeAlphaPanelBrush()
        {
            if (alphaPicker != null)
            {
                alphaPicker.PanelBrush = new LinearGradientBrush()
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0,1)
                };
                alphaPicker.PanelBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
                alphaPicker.PanelBrush.GradientStops.Add(new GradientStop(Colors.Red, 1));
            }
        }
        #endregion
    }
}