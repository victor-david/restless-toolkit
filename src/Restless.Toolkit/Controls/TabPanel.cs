using Restless.Toolkit.Core;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    public class TabPanel : Panel
    {
        private TabControl parent;

        public TabPanel()
        {
            SetZIndex(this, 1);
            VerticalAlignment = VerticalAlignment.Bottom;
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            parent = CoreHelper.GetVisualParent<TabControl>(this);
            if (parent == null) throw new ArgumentNullException(nameof(parent));
        }


        protected override Size MeasureOverride(Size constraint)
        {
            Size contentSize = new Size(0, parent.TabHeight + parent.TabHeightIncrease);
            int childIdx = 0;
            foreach (TabItem child in InternalChildren.OfType<TabItem>().Where((item) => item.Visibility != Visibility.Collapsed))
            {
                child.IsLeftmost = childIdx == 0;
                child.Measure(constraint);

                if (child.DesiredSize.Width + contentSize.Width < constraint.Width)
                {
                    child.Visibility = Visibility.Visible;
                    contentSize.Width += child.DesiredSize.Width;
                }
                else
                {
                    child.Visibility = Visibility.Hidden;
                }
                childIdx++;
            }

            return contentSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Vector childOffset = new Vector();
            double border = parent.BorderThickness.Left;
            double rowHeight = parent.TabHeight + parent.TabHeightIncrease;
            int childIdx = 0;
            foreach (TabItem child in InternalChildren.OfType<TabItem>().Where((item) => item.Visibility == Visibility.Visible))
            {
                double leftOffset = child.IsSelected && !child.IsLeftmost ? border : 0.0;
                double rightOffset = child.IsSelected ? border : 0.0;
                double totalWidth = child.DesiredSize.Width + leftOffset + rightOffset;
                double yOffset = child.IsSelected ? 0.0 : parent.TabHeightIncrease;
                double totalHeight = child.IsSelected ? rowHeight + border : rowHeight - yOffset;

                Rect rect = new Rect(childOffset.X - leftOffset, childOffset.Y + yOffset, totalWidth, totalHeight);
                child.Arrange(rect);
                childOffset.X += child.DesiredSize.Width;
                childIdx++;
            }

            return finalSize;
        }

        /// <summary>
        /// Override of <see cref="UIElement.GetLayoutClip"/>.
        /// </summary>
        /// <returns>Geometry to use as additional clip in case when element is larger then available space</returns>
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return null;
        }
    }
}