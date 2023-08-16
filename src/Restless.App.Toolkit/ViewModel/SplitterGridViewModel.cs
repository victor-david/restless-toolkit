using Restless.Toolkit.Mvvm;
using System.Windows.Controls;

namespace Restless.App.Toolkit
{
    public class SplitterGridViewModel : ViewModelBase
    {
        private Orientation orientation;
        private double minDetailSize;

        public Orientation Orientation
        {
            get => orientation;
            private set => SetProperty(ref orientation, value);
        }

        public double MinDetailSize
        {
            get => minDetailSize;
            private set => SetProperty(ref minDetailSize, value);
        }

        public SplitterGridViewModel()
        {
            DisplayName = "Splitter";
            Commands.Add("Toggle", RunToggleOrientationCommand);
            Orientation = Orientation.Horizontal;
            MinDetailSize = 120;
        }

        private void RunToggleOrientationCommand(object parm)
        {
            Orientation = Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
            MinDetailSize = Orientation == Orientation.Horizontal ? 120 : 220;
        }
    }
}