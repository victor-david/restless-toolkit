using System;
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
        private double tabHeightIncrease;
        private ContentPresenter contentPresenter;
        #endregion

        /************************************************************************/

        #region Constructors
        public TabItem()
        {
            Panel.SetZIndex(this, 1);
            TabHeightIncrease = TabControl.DefaultTabHeightIncrease;
        }

        static TabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(typeof(TabItem)));
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the amount to increase the height of the tab when it is selected.
        /// </summary>
        public double TabHeightIncrease 
        {
            get => tabHeightIncrease; 
            internal set
            {
                tabHeightIncrease = value;
                Margin = new Thickness(0, tabHeightIncrease, 0, 0);
            }
        }
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
            if (GetParent() is TabControl parent)
            {
                double parentVal = -parent.BorderThickness.Left;
                double left = parentVal;
                if (parent.ItemContainerGenerator.ContainerFromIndex(0) == this)
                {
                    left = 0;
                }

                Margin = new Thickness(left, 0, parentVal, parentVal - 1);
                Panel.SetZIndex(this, 2);
            }
        }

        /// <summary>
        /// Called when the item is unselected.
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            Margin = new Thickness(0, tabHeightIncrease, 0, 0);
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
        #endregion

        /************************************************************************/

        #region Private methods
        private TabControl GetParent()
        {
            return ItemsControl.ItemsControlFromItemContainer(this) as TabControl;
        }
        #endregion
    }
}
