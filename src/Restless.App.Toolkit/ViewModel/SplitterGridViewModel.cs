using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm;

namespace Restless.App.Toolkit
{
    public class SplitterGridViewModel : ViewModelBase
    {
        private SplitterPosition splitterPosition;
        private bool canCollapseDetail;

        public SplitterPosition SplitterPosition
        {
            get => splitterPosition;
            private set => SetProperty(ref splitterPosition, value);
        }

        public bool CanCollapseDetail
        {
            get => canCollapseDetail;
            set => SetProperty(ref canCollapseDetail, value);
        }

        public SplitterGridViewModel()
        {
            DisplayName = "Splitter";
            Commands.Add("ChangePosition", p => SplitterPosition = GetSplitterPosition());
            SplitterPosition = MultiSplitterGrid.DefaultSplitterPosition;
            CanCollapseDetail = true;
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