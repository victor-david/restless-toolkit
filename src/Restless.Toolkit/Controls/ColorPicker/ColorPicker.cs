using System;
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
        private bool mainPickerEventInProgress;
        #endregion

        /************************************************************************/

        #region Public constant values
        /// <summary>
        /// Gets the minimum allowed value of <see cref="CanvasWidth"/>.
        /// </summary>
        public const double MinCanvasWidth = 148;
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

        #region Properties (sliders)
        /// <summary>
        /// Gets or sets the configuration of the sliders
        /// </summary>
        public ColorSliderConfig Sliders
        {
            get => (ColorSliderConfig)GetValue(SlidersProperty);
            set => SetValue(SlidersProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Sliders"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SlidersProperty = DependencyProperty.Register
            (
                nameof(Sliders), typeof(ColorSliderConfig), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = ColorSliderConfig.Default,
                    PropertyChangedCallback = OnSlidersPropertyChanged,
                }
            );

        private static void OnSlidersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker control)
            {
                control.DisplayAlpha = control.Sliders.HasFlag(ColorSliderConfig.Alpha);
                control.DisplayRgb = control.Sliders.HasFlag(ColorSliderConfig.Rgb);
            }
        }

        /// <summary>
        /// Gets a boolean value that determines if the alpha slider is displayed.
        /// </summary>
        public bool DisplayAlpha
        {
            get => (bool)GetValue(DisplayAlphaProperty);
            private set => SetValue(DisplayAlphaPropertyKey, value);
        }

        private static readonly DependencyPropertyKey DisplayAlphaPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(DisplayAlpha), typeof(bool), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = true,
                }
            );

        /// <summary>
        /// Identifies the <see cref="DisplayAlpha"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayAlphaProperty = DisplayAlphaPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a boolean value that deterneins if the RGB sliders are displayed.
        /// </summary>
        public bool DisplayRgb
        {
            get => (bool)GetValue(DisplayRgbProperty);
            private set => SetValue(DisplayRgbPropertyKey, value);
        }

        private static readonly DependencyPropertyKey DisplayRgbPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(DisplayRgb), typeof(bool), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = true,
                }
            );

        /// <summary>
        /// Identifies the <see cref="DisplayRgb"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayRgbProperty = DisplayRgbPropertyKey.DependencyProperty;

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
        #endregion

        /************************************************************************/

        #region Properties (canvas)
        /// <summary>
        /// Gets or sets the width for the main picker canvas.
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
        /// Gets the canvas height.
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
        #endregion

        /************************************************************************/

        #region Properties (color components)
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
                    DefaultValue = ColorValues.DefaultAlpha,
                    CoerceValueCallback = OnCoerceAlpha,
                    PropertyChangedCallback = OnAlphaPropertyChanged
                }
            );

        private static object OnCoerceAlpha(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Min(Math.Max(Math.Round(value), ColorValues.MinRgbaComponent), ColorValues.MaxRgbaComponent);
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
                    DefaultValue = ColorValues.DefaultColor,
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

        /// <summary>
        /// From this assenbly, gets or sets the color hex code.
        /// </summary>
        internal string ColorHexCode
        {
            get => (string)GetValue(ColorHexCodeProperty);
            set => SetValue(ColorHexCodeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColorHexCode"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty ColorHexCodeProperty = DependencyProperty.Register
            (
                nameof(ColorHexCode), typeof(string), typeof(ColorPicker), new PropertyMetadata()
                {
                    DefaultValue = ColorValues.DefaultHexCode
                }
            );
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
            SyncAlpha();
            UpdateAlphaPanelBrush();
            UpdateSelectedColorBrush();
            UpdateColorHexCode();
            SyncToMainPicker();
        }

        private void MainPickerColorComponentChanged(object sender, ColorComponents e)
        {
            mainPickerEventInProgress = true;
            SelectedColor = ColorHelper.ColorFromHSB(e.Hue, e.Saturation, e.Brightness);
            mainPickerEventInProgress = false;
        }

        private void UpdateSelectedColorBrush()
        {
            SelectedColorBrush.Color = SelectedColor;
        }

        private void UpdateColorHexCode()
        {
            ColorHexCode = SelectedColor.ToString();
        }

        private void SyncToMainPicker()
        {
            if (!mainPickerEventInProgress)
            {
                mainPicker?.UpdateColorComponents(SelectedColor);
            }
        }

        /// <summary>
        /// Synchonizes the alpha value by either applying it to the selected color,
        /// or taking the alpha value from the selected color and applying it
        /// to the <see cref="Alpha"/> property, depending on the origin of the color change.
        /// </summary>
        private void SyncAlpha()
        {
            if (mainPickerEventInProgress)
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
            SelectedColorBrush = new SolidColorBrush(ColorValues.DefaultColor);
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
                    EndPoint = new Point(1,0)
                };
                alphaPicker.PanelBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
                alphaPicker.PanelBrush.GradientStops.Add(new GradientStop(ColorValues.DefaultColor, 1));
            }
        }
        #endregion
    }
}