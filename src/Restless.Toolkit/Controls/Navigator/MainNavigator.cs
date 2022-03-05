using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a navigator control that presents <see cref="NavigatorItem"/> objects.
    /// </summary>
    public class MainNavigator : System.Windows.Controls.ListBox
    {
        #region Private
        private const double DefaultLeftMargin = 10;
        private const double ItemIndent = 22;
        private Thickness navItemMargin;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainNavigator"/> class.
        /// </summary>
        public MainNavigator()
        {
            navItemMargin = new Thickness(DefaultLeftMargin + ItemIndent, 0, 0, 0);
        }

        static MainNavigator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainNavigator), new FrameworkPropertyMetadata(typeof(MainNavigator)));
            MarginProperty.OverrideMetadata(typeof(MainNavigator), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnMarginPropertyChanged
            });
        }

        private static void OnMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MainNavigator)?.AdjustItemMargin();
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the header text for the navigator
        /// </summary>
        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HeaderText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register
            (
                nameof(HeaderText), typeof(string), typeof(MainNavigator), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets a boolean value that determines if the navigator is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsExpanded"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register
            (
                nameof(IsExpanded), typeof(bool), typeof(MainNavigator), new FrameworkPropertyMetadata()
                {
                    DefaultValue = false,
                    BindsTwoWayByDefault = true,
                }
            );

        /// <summary>
        /// Gets or sets the icon brush.
        /// </summary>
        public Brush IconBrush
        {
            get => (Brush)GetValue(IconBrushProperty);
            set => SetValue(IconBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IconBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register
            (
                nameof(IconBrush), typeof(Brush), typeof(MainNavigator), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Brushes.LightGray
                }
            );

        /// <summary>
        /// Gets or sets a boolean value that determines if the control hides itself
        /// when none of its <see cref="NavigatorItem"/> objects are visible.
        /// </summary>
        public bool HideWhenEmpty
        {
            get => (bool)GetValue(HideWhenEmptyProperty);
            set => SetValue(HideWhenEmptyProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HideWhenEmpty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HideWhenEmptyProperty = DependencyProperty.Register
            (
                nameof(HideWhenEmpty), typeof(bool), typeof(MainNavigator), new FrameworkPropertyMetadata()
                {
                    DefaultValue = true,
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnHideWhenEmptyChanged
                }
            );

        private static void OnHideWhenEmptyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MainNavigator)?.EvaluateVisibility();
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <inheritdoc/>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (item is NavigatorItem navItem)
            {
                navItem.IconBrush = IconBrush;
                navItem.InternalGridMargin = navItemMargin;
            }

            if (element is ListBoxItem listBoxItem)
            {
                listBoxItem.Margin = new Thickness(-Margin.Left, 0, 0, 0);
            }
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            EvaluateVisibility();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void EvaluateVisibility()
        {
            Visibility =
                Items.OfType<NavigatorItem>().Any((n) => n.IsItemVisible) ?
                Visibility.Visible :
                HideWhenEmpty ? Visibility.Collapsed : Visibility.Visible;
        }

        private void AdjustItemMargin()
        {
            // TODO
        }
        #endregion
    }
}