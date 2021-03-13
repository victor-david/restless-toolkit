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
        private TabControl parent;
        private ContentPresenter contentPresenter;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TabItem"/> class.
        /// </summary>
        public TabItem()
        {
            Panel.SetZIndex(this, 1);
            VerticalAlignment = VerticalAlignment.Bottom;
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        static TabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(typeof(TabItem)));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata()
            {
                DefaultValue = HorizontalAlignment.Stretch,
                CoerceValueCallback = OnCoerceHorizontalAlignment
            });

            VerticalAlignmentProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata()
            {
                DefaultValue = VerticalAlignment.Bottom,
                CoerceValueCallback = OnCoerceVerticalAlignment,
            });
        }
        #endregion

        /************************************************************************/

        #region Properties
        internal bool IsItemVisible { get; set; }
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
        private static object OnCoerceHorizontalAlignment(DependencyObject d, object baseValue)
        {
            return HorizontalAlignment.Stretch;
        }

        private static object OnCoerceVerticalAlignment(DependencyObject d, object baseValue)
        {
            return VerticalAlignment.Bottom;
        }
        #endregion
    }
}