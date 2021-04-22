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
        private Color selectedColor;

        public ColorPickerViewModel()
        {
            DisplayName = "Color Picker";
            Commands.Add("SetColor", RunSetColorCommand);
            CanvasWidth = ColorPicker.DefaultCanvasWidth;
            SliderSize = ColorSlider.DefaultSliderSize;
            SelectedColor = ColorValues.DefaultColor;
            DisplayRgbSliders = true;
            DisplayAlphaSlider = true;
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

        private void RunSetColorCommand(object parm)
        {
            if (parm is Color color)
            {
                SelectedColor = color;
            }
        }
    }
}