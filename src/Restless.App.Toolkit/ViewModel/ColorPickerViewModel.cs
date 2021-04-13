using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm;
using System.Windows.Media;

namespace Restless.App.Toolkit
{
    public class ColorPickerViewModel : ViewModelBase
    {
        private double canvasWidth;
        private double sliderSize;
        private Color selectedColor;

        public ColorPickerViewModel()
        {
            DisplayName = "Color Picker";
            Commands.Add("SetColor", RunSetColorCommand);
            CanvasWidth = ColorPicker.DefaultCanvasWidth;
            SliderSize = ColorSlider.DefaultSliderSize;
            SelectedColor = Colors.Green;
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
        /// Gets or sets the color for the view.
        /// </summary>
        public Color SelectedColor
        {
            get => selectedColor;
            set => SetProperty(ref selectedColor, value);
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