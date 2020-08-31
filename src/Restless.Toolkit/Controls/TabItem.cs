using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a tab item that is used with <see cref="TabControl"/>
    /// </summary>
    public class TabItem : System.Windows.Controls.TabItem
    {
        #region Private
        private TabControl parent;
        //private double standardHeight;
        //private double tabHeightIncrease;
        private ContentPresenter contentPresenter;
        #endregion

        /************************************************************************/

        #region Constructors
        public TabItem()
        {
            Panel.SetZIndex(this, 1);
            VerticalAlignment = VerticalAlignment.Bottom;

            Debug.WriteLine(this.FontFamily);
            Debug.WriteLine(this.FontSize);

        }

        static TabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(typeof(TabItem)));
        }
        #endregion

        /************************************************************************/

        #region Properties
        internal bool IsLeftmost { get;  set; }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when the item is selected.
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            Height = parent.TabHeight + parent.TabHeightIncrease + BorderThickness.Top;
            Background = parent.Background;
            Opacity = 1.0;
            Panel.SetZIndex(this, 2);
        }

        /// <summary>
        /// Called when the item is unselected.
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            Height = parent.TabHeight;
            Background = parent.InactiveTabBackground;
            Opacity = parent.InactiveTabOpacity;
            Panel.SetZIndex(this, 1);
        }
        #endregion

        /************************************************************************/

        #region Internal methods
        internal ContentPresenter GetContentPresenter()
        {
            if (contentPresenter == null)
            {
                contentPresenter = new ContentPresenter
                {
                    Content = Content,
                    ContentTemplate = ContentTemplate,
                    ContentTemplateSelector = ContentTemplateSelector,
                    ContentStringFormat = ContentStringFormat,
                };
            }
            return contentPresenter;
        }

        internal void SyncToParent(TabControl parent)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            SyncToParentBorder(parent);

            Height = parent.TabHeight;
            MinWidth = parent.MinTabWidth;
            Background = parent.InactiveTabBackground;
            Opacity = parent.InactiveTabOpacity;
        }

        internal void SyncToParentBorder(TabControl parent)
        {
            double value = parent.BorderThickness.Left;
            BorderThickness = new Thickness(value, value, value, 0);
            BorderBrush = parent.BorderBrush;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        #endregion
    }
}
