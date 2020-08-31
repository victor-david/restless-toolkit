using Restless.Toolkit.Core;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Restless.Toolkit.Controls
{
    public class TabPanel : Panel
    {
        private TabControl parent;
        private double rowHeight;

        private int measure;
        private int arrange;

        public TabPanel()
        {
            SetZIndex(this, 1);
            VerticalAlignment = VerticalAlignment.Bottom;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            parent = CoreHelper.GetVisualParent<TabControl>(this);
            if (parent == null) throw new ArgumentNullException(nameof(parent));
        }


        protected override Size MeasureOverride(Size constraint)
        {
            measure++;
            Debug.WriteLine($"Measure #{measure}");

            rowHeight = parent.TabHeight + parent.TabHeightIncrease;
            Size contentSize = new Size(0, rowHeight);
            int childIdx = 0;
            foreach (TabItem child in InternalChildren.OfType<TabItem>())
            {
                if (child.Visibility != Visibility.Collapsed)
                {
                    child.Measure(constraint);
                    child.IsLeftmost = childIdx == 0;
                    
                    if (child.DesiredSize.Width + contentSize.Width < constraint.Width)
                    {
                        contentSize.Width += child.DesiredSize.Width;
                    }

                    childIdx++;
                }
            }

            //contentSize.Height = rowHeight;

            Debug.WriteLine($"Measure: {contentSize}");
            Debug.WriteLine("-----------------");

            return contentSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            arrange++;
            Debug.WriteLine($"Arrange #{arrange}");
            Vector childOffset = new Vector();
            double border = parent.BorderThickness.Left;
            int childIdx = 0;
            foreach (TabItem child in InternalChildren.OfType<TabItem>())
            {
                if (child.Visibility != Visibility.Collapsed)
                {
                    double leftOffset = child.IsSelected && !child.IsLeftmost ? border : 0.0;
                    double rightOffset = child.IsSelected ? border : 0.0;
                    double totalWidth = child.DesiredSize.Width + leftOffset + rightOffset;
                    double yOffset = child.IsSelected ? 0.0 : parent.TabHeightIncrease;
                    double totalHeight = child.IsSelected ? rowHeight + border : rowHeight - yOffset;

                    Rect rect = new Rect(childOffset.X - leftOffset, childOffset.Y + yOffset, totalWidth, totalHeight);

                    Debug.WriteLine($"Arrange child# {childIdx} (Selected: {child.IsSelected}) to rect: {rect}");

                    child.Arrange(rect);
                    childOffset.X += child.DesiredSize.Width;
                    childIdx++;
                }
            }

            Debug.WriteLine("-----------------");
            // Debug.WriteLine($"Arrange: {finalSize}");

            return finalSize;
        }
    }
}