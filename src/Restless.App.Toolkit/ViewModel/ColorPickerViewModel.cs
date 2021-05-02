using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm;
using System.Windows.Media;

namespace Restless.App.Toolkit
{
    public class ColorPickerViewModel : ViewModelBase
    {
        private double canvasWidth;
        private double sliderSize;
        private ColorSliderConfig sliderConfig;
        private bool displayRgbSliders;
        private bool displayAlphaSlider;
        private ColorDemoConfig demoConfig;
        private bool displayDemoCanvas;
        private bool displayDemoHex;
        private Color selectedColor;

        public ColorPickerViewModel()
        {
            DisplayName = "Color Picker";
            Commands.Add("SetColor", RunSetColorCommand);
            CanvasWidth = ColorPicker.DefaultCanvasWidth;
            SliderSize = ColorSlider.DefaultSliderSize;
            SelectedColor = ColorValues.DefaultColor;
            SliderConfig = ColorSliderConfig.Default;
            DisplayRgbSliders = true;
            DisplayAlphaSlider = true;
            DemoConfig = ColorDemoConfig.Default;
            DisplayDemoCanvas = true;
            DisplayDemoHex = true;
        }

        /// <summary>
        /// Gets or sets the canvas width for color picker.
        /// </summary>
        public double CanvasWidth
        {
            get => canvasWidth;
            set => SetProperty(ref canvasWidth, value);
        }

        /// <summary>
        /// Gets or sets the slider size for the color picker.
        /// </summary>
        public double SliderSize
        {
            get => sliderSize;
            set => SetProperty(ref sliderSize, value);
        }

        /// <summary>
        /// Gets the value that configures the sliders for the color picker control.
        /// </summary>
        public ColorSliderConfig SliderConfig
        {
            get => sliderConfig;
            private set => SetProperty(ref sliderConfig, value);
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if the RGB sliders are displayed.
        /// </summary>
        public bool DisplayRgbSliders
        {
            get => displayRgbSliders;
            set
            {
                SetProperty(ref displayRgbSliders, value);
                UpdateSliderConfig();
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if the alpha slider is displayed.
        /// </summary>
        public bool DisplayAlphaSlider
        {
            get => displayAlphaSlider;
            set
            {
                SetProperty(ref displayAlphaSlider, value);
                UpdateSliderConfig();
            }
        }

        /// <summary>
        /// Gets the value that configures the demo components for the color picker control.
        /// </summary>
        public ColorDemoConfig DemoConfig
        {
            get => demoConfig;
            private set => SetProperty(ref demoConfig, value);
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if the demo canvas is displayed.
        /// </summary>
        public bool DisplayDemoCanvas
        {
            get => displayDemoCanvas;
            set
            {
                SetProperty(ref displayDemoCanvas, value);
                UpdateDemoConfig();
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if the demo hex code is displayed.
        /// </summary>
        public bool DisplayDemoHex
        {
            get => displayDemoHex;
            set
            {
                SetProperty(ref displayDemoHex, value);
                UpdateDemoConfig();
            }
        }

        /// <summary>
        /// Gets or sets the color for the view.
        /// </summary>
        public Color SelectedColor
        {
            get => selectedColor;
            set => SetProperty(ref selectedColor, value);
        }

        private void UpdateSliderConfig()
        {
            ColorSliderConfig value = ColorSliderConfig.None;
            if (DisplayRgbSliders) value |= ColorSliderConfig.Rgb;
            if (DisplayAlphaSlider) value |= ColorSliderConfig.Alpha;
            SliderConfig = value;
        }

        private void UpdateDemoConfig()
        {
            ColorDemoConfig value = ColorDemoConfig.None;
            if (DisplayDemoCanvas) value |= ColorDemoConfig.Canvas;
            if (DisplayDemoHex) value |= ColorDemoConfig.HexCode;
            DemoConfig = value;
        }

        private void RunSetColorCommand(object parm)
        {
            if (parm is Color color)
            {
                SelectedColor = color;
            }
        }
    }
}