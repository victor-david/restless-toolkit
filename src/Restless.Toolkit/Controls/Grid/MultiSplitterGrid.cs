using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides a specialized control that contains a grid
    /// that can be expanded / contracted to show or hide detail
    /// </summary>
    public class MultiSplitterGrid : Control
    {
        #region Private
        private bool sizeUpdateInProgress;
        #endregion

        /************************************************************************/

        #region Public fields
        /// <summary>
        /// The minimum value for <see cref="MinDetailSize"/>
        /// </summary>
        public const double MinMinDetailSize = 60;
        /// <summary>
        /// The default value for <see cref="MinDetailSize"/>
        /// </summary>
        public const double DefaultMinDetailSize = 100;
        /// <summary>
        /// The minimum value for <see cref="MaxDetailSize"/>
        /// </summary>
        public const double MinMaxDetailSize = 140;
        /// <summary>
        /// The default value for <see cref="MaxDetailSize"/>
        /// </summary>
        public const double DefaultMaxDetailSize = 460;
        /// <summary>
        /// The default value for <see cref="DetailSize"/>
        /// </summary>
        public const double DefaultDetailSize = 360;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSplitterGrid"/> class
        /// </summary>
        public MultiSplitterGrid()
        {
        }

        static MultiSplitterGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSplitterGrid), new FrameworkPropertyMetadata(typeof(MultiSplitterGrid)));
        }
        #endregion

        /************************************************************************/

        #region State
        /// <summary>
        /// Gets or sets the orientation
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register
            (
                nameof(Orientation), typeof(Orientation), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Orientation.Vertical
                }
            );

        /// <summary>
        /// Gets or sets a boolean value that determines if the detail panel may be collapsed
        /// </summary>
        public bool CanCollapseDetail
        {
            get => (bool)GetValue(CanCollapseDetailProperty);
            set => SetValue(CanCollapseDetailProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CanCollapseDetail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanCollapseDetailProperty = DependencyProperty.Register
            (
                nameof(CanCollapseDetail), typeof(bool), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = true,
                    PropertyChangedCallback = OnCanCollapseDetailChanged
                }
            );

        private static void OnCanCollapseDetailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiSplitterGrid control)
            {
                control.ExpanderVisibility = control.CanCollapseDetail ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if the detail panel is expanded.
        /// </summary>
        public bool IsDetailExpanded
        {
            get => (bool)GetValue(IsDetailExpandedProperty);
            set => SetValue(IsDetailExpandedProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsDetailExpanded"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDetailExpandedProperty = DependencyProperty.Register
            (
                nameof(IsDetailExpanded), typeof(bool), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = true,
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnIsDetailExpandedChanged
                }
            );

        private static void OnIsDetailExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MultiSplitterGrid)?.UpdateActualDetailSize();
        }

        /// <summary>
        /// Gets the visibility for the expander
        /// </summary>
        public Visibility ExpanderVisibility
        {
            get => (Visibility)GetValue(ExpanderVisibilityProperty);
            private set => SetValue(ExpanderVisibilityPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ExpanderVisibilityPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ExpanderVisibility), typeof(Visibility), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Visibility.Visible
                }
            );

        /// <summary>
        /// Identifies the <see cref="ExpanderVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpanderVisibilityProperty = ExpanderVisibilityPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the visibility for the splitter
        /// </summary>
        public Visibility SplitterVisibility
        {
            get => (Visibility)GetValue(SplitterVisibilityProperty);
            private set => SetValue(SplitterVisibilityPropertyKey, value);
        }

        private static readonly DependencyPropertyKey SplitterVisibilityPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(SplitterVisibility), typeof(Visibility), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Visibility.Visible
                }
            );

        /// <summary>
        /// Identifies the <see cref="SplitterVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SplitterVisibilityProperty = SplitterVisibilityPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Dimensions
        /// <summary>
        /// Gets or sets the minimum detail size, height or width depending on orientation
        /// </summary>
        public double MinDetailSize
        {
            get => (double)GetValue(MinDetailSizeProperty);
            set => SetValue(MinDetailSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MinDetailSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinDetailSizeProperty = DependencyProperty.Register
            (
                nameof(MinDetailSize), typeof(double), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultMinDetailSize,
                    CoerceValueCallback = OnCoerceMinDetailSize,
                    PropertyChangedCallback = OnMinDetailSizeChanged
                }
            );

        private static object OnCoerceMinDetailSize(DependencyObject d, object baseValue)
        {
            return Math.Max((double)baseValue, MinMinDetailSize);
        }

        private static void OnMinDetailSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MultiSplitterGrid)?.HandleMinDetailSizeChanged();
        }

        /// <summary>
        /// Gets the actual min detail size
        /// </summary>
        public double ActualMinDetailSize
        {
            get => (double)GetValue(ActualMinDetailSizeProperty);
            private set => SetValue(ActualMinDetailSizePropertyKey, value);
        }

        private static readonly DependencyPropertyKey ActualMinDetailSizePropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ActualMinDetailSize), typeof(double), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultMinDetailSize
                }
            );

        /// <summary>
        /// Identifies the <see cref="ActualMinDetailSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualMinDetailSizeProperty = ActualMinDetailSizePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets the maximum detail size, height or width depending on orientation
        /// </summary>
        public double MaxDetailSize
        {
            get => (double)GetValue(MaxDetailSizeProperty);
            set => SetValue(MaxDetailSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MaxDetailSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxDetailSizeProperty = DependencyProperty.Register
            (
                nameof(MaxDetailSize), typeof(double), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultMaxDetailSize,
                    CoerceValueCallback = OnCoerceMaxDetailSize
                }
            );

        private static object OnCoerceMaxDetailSize(DependencyObject d, object baseValue)
        {
            return Math.Max((double)baseValue, MinMaxDetailSize);
        }

        /// <summary>
        /// Gets or sets the detail size, height or width depending on orientation
        /// </summary>
        public double DetailSize
        {
            get => (double)GetValue(DetailSizeProperty);
            set => SetValue(DetailSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DetailSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DetailSizeProperty = DependencyProperty.Register
            (
                nameof(DetailSize), typeof(double), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultDetailSize,
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnDetailSizeChanged
                }
            );

        private static void OnDetailSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MultiSplitterGrid)?.HandleDetaiSizeChanged();
        }

        /// <summary>
        /// Gets or sets the actual detail size, height or width depending on orientation
        /// </summary>
        internal GridLength ActualDetailSize
        {
            get => (GridLength)GetValue(ActualDetailSizeProperty);
            set => SetValue(ActualDetailSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ActualDetailSize"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty ActualDetailSizeProperty = DependencyProperty.Register
            (
                nameof(ActualDetailSize), typeof(GridLength), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new GridLength(DefaultDetailSize),
                    PropertyChangedCallback = OnActualDetailSizeChanged
                }
            );

        private static void OnActualDetailSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MultiSplitterGrid)?.HandleActualDetailSizeChanged();
        }
        #endregion

        /************************************************************************/

        #region Margins
        /// <summary>
        /// Gets or sets the margin for the main header
        /// </summary>
        public Thickness MainHeaderMargin
        {
            get => (Thickness)GetValue(MainHeaderMarginProperty);
            set => SetValue(MainHeaderMarginProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MainHeaderMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MainHeaderMarginProperty = DependencyProperty.Register
            (
                nameof(MainHeaderMargin), typeof(Thickness), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new Thickness()
                }
            );

        /// <summary>
        /// Gets or sets the margin for the detail header
        /// </summary>
        public Thickness DetailHeaderMargin
        {
            get => (Thickness)GetValue(DetailHeaderMarginProperty);
            set => SetValue(DetailHeaderMarginProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DetailHeaderMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DetailHeaderMarginProperty = DependencyProperty.Register
            (
                nameof(DetailHeaderMargin), typeof(Thickness), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new Thickness()
                }
            );

        /// <summary>
        /// Gets or sets the margin for the main content
        /// </summary>
        public Thickness MainContentMargin
        {
            get => (Thickness)GetValue(MainContentMarginProperty);
            set => SetValue(MainContentMarginProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MainContentMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MainContentMarginProperty = DependencyProperty.Register
            (
                nameof(MainContentMargin), typeof(Thickness), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new Thickness()
                }
            );

        /// <summary>
        /// Gets or sets the margin for the detail content
        /// </summary>
        public Thickness DetailContentMargin
        {
            get => (Thickness)GetValue(DetailContentMarginProperty);
            set => SetValue(DetailContentMarginProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DetailContentMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DetailContentMarginProperty = DependencyProperty.Register
            (
                nameof(DetailContentMargin), typeof(Thickness), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new Thickness()
                }
            );
        #endregion

        /************************************************************************/

        #region Header / content
        /// <summary>
        /// Gets or sets the main header content
        /// </summary>
        public object MainHeader
        {
            get => GetValue(MainHeaderProperty);
            set => SetValue(MainHeaderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MainHeader"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MainHeaderProperty = DependencyProperty.Register
            (
                nameof(MainHeader), typeof(object), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets detail header content
        /// </summary>
        public object DetailHeader
        {
            get => GetValue(DetailHeaderProperty);
            set => SetValue(DetailHeaderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DetailHeader"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DetailHeaderProperty = DependencyProperty.Register
            (
                nameof(DetailHeader), typeof(object), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the main content
        /// </summary>
        public object MainContent
        {
            get => GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MainContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MainContentProperty = DependencyProperty.Register
            (
                nameof(MainContent), typeof(object), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the detail content
        /// </summary>
        public object DetailContent
        {
            get => GetValue(DetailContentProperty);
            set => SetValue(DetailContentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="DetailContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DetailContentProperty = DependencyProperty.Register
            (
                nameof(DetailContent), typeof(object), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
            );


        /// <summary>
        /// Gets or sets the text that is displayed next to the button when in horizontal mode
        /// </summary>
        public string ButtonSideText
        {
            get => (string)GetValue(ButtonSideTextProperty);
            set => SetValue(ButtonSideTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ButtonSideText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonSideTextProperty = DependencyProperty.Register
            (
                nameof(ButtonSideText), typeof(string), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
            );


        /// <summary>
        /// Gets or sets the brush to use for <see cref="ButtonSideText"/>
        /// </summary>
        public Brush ButtonSideTextBrush
        {
            get => (Brush)GetValue(ButtonSideTextBrushProperty);
            set => SetValue(ButtonSideTextBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ButtonSideTextBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonSideTextBrushProperty = DependencyProperty.Register
            (
                nameof(ButtonSideTextBrush), typeof(Brush), typeof(MultiSplitterGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Brushes.Gray
                }
            );

        #endregion

        /************************************************************************/

        #region Private methods
        private void HandleMinDetailSizeChanged()
        {
            if (IsDetailExpanded)
            {
                ActualMinDetailSize = MinDetailSize;
            }
        }

        private void HandleDetaiSizeChanged()
        {
            if (!sizeUpdateInProgress)
            {
                UpdateActualDetailSize();
            }
        }

        private void HandleActualDetailSizeChanged()
        {
            if (IsDetailExpanded)
            {
                sizeUpdateInProgress = true;
                DetailSize = ActualDetailSize.Value;
                sizeUpdateInProgress = false;
            }
        }

        private void UpdateActualDetailSize()
        {
            if (IsDetailExpanded)
            {
                ActualDetailSize = new GridLength(DetailSize);
                ActualMinDetailSize = MinDetailSize;
                SplitterVisibility = Visibility.Visible;
            }
            else
            {
                ActualMinDetailSize = 0;
                ActualDetailSize = new GridLength(0);
                SplitterVisibility = Visibility.Collapsed;
            }
        }
        #endregion
    }
}