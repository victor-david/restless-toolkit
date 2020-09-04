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
            HorizontalAlignment = HorizontalAlignment.Left;
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
            int hideIdxThreshold = 0;

            MeasureResult result;

            do
            {
                result = PerformMeasure(constraint, hideIdxThreshold++);
            } while (result.IsSelectedHidden || hideIdxThreshold == InternalChildren.Count - 1);

            contentSize.Width = result.TotalWidth;

            return contentSize;
        }

        private MeasureResult PerformMeasure(Size constraint, int autoHideIdxThreshold)
        {
            int childIdx = 0;
            double totalWidth = 0.0;
            bool isSelectedHidden = false;

            foreach (TabItem child in InternalChildren.OfType<TabItem>().Where((item) => item.IsVisible))
            {
                child.Measure(constraint);
                child.IsItemVisible = childIdx >= autoHideIdxThreshold && totalWidth + child.DesiredSize.Width <= constraint.Width;

                if (child.IsItemVisible)
                {
                    totalWidth += child.DesiredSize.Width;
                }

                if (child.IsSelected)
                {
                    isSelectedHidden = !child.IsItemVisible;
                }
                childIdx++;
            }
            return new MeasureResult(totalWidth, isSelectedHidden);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            double xOffset = 0.0;
            double border = parent.BorderThickness.Left;
            double rowHeight = parent.TabHeight + parent.TabHeightIncrease;

            foreach (TabItem child in InternalChildren.OfType<TabItem>().Where((item) => item.IsVisible))
            {
                double yOffset = child.IsSelected ? 0.0 : parent.TabHeightIncrease;
                double totalHeight = child.IsSelected ? rowHeight + border : rowHeight - yOffset;
                double renderWidth = child.IsItemVisible ? child.DesiredSize.Width : 0.0;

                Rect rect = new Rect(xOffset, yOffset, renderWidth, totalHeight);
                child.Arrange(rect);

                xOffset += Math.Max(renderWidth - border, 0.0);
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

        #region Private helper class
        private class MeasureResult
        {
            public double TotalWidth { get; }
            public bool IsSelectedHidden { get; }
            public MeasureResult(double totalWidth, bool isSelectedHidden)
            {
                TotalWidth = totalWidth;
                IsSelectedHidden = isSelectedHidden;
            }
        }
        #endregion
    }
}