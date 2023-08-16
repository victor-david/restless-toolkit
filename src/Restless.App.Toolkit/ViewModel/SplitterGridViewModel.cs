using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm;
using System.Windows.Controls;

namespace Restless.App.Toolkit
{
    public class SplitterGridViewModel : ViewModelBase
    {
        private SplitterPosition splitterPosition;
        private double minDetailSize;

        public SplitterPosition SplitterPosition
        {
            get => splitterPosition;
            private set => SetProperty(ref splitterPosition, value);
        }

        public double MinDetailSize
        {
            get => minDetailSize;
            private set => SetProperty(ref minDetailSize, value);
        }

        public SplitterGridViewModel()
        {
            DisplayName = "Splitter";
            Commands.Add("ChangePosition", p => SplitterPosition = GetSplitterPosition());
            SplitterPosition = MultiSplitterGrid.DefaultSplitterPosition;
            MinDetailSize = 120;
        }

        private SplitterPosition GetSplitterPosition()
        {
            return SplitterPosition switch
            {
                SplitterPosition.Right => SplitterPosition.Bottom,
                SplitterPosition.Bottom => SplitterPosition.Left,
                SplitterPosition.Left => SplitterPosition.Top,
                SplitterPosition.Top => SplitterPosition.Right,
                _ => SplitterPosition.Right,
            };
        }
    }
}