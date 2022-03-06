using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a single item for a <see cref="MainNavigator"/>.
    /// </summary>
    public class NavigatorItem : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatorItem"/> class.
        /// </summary>
        /// <param name="groupIdx">The group index</param>
        /// <param name="targetType">The target type of the item.</param>
        /// <param name="id">The associated id</param>
        public NavigatorItem(int groupIdx, Type targetType, long id)
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            GroupIndex = groupIdx;
            Id = id;
        }

        static NavigatorItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigatorItem), new FrameworkPropertyMetadata(typeof(NavigatorItem)));
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the title for the navigaor item
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            (
                nameof(Title), typeof(string), typeof(NavigatorItem), new PropertyMetadata()
                {

                }
            );

        /// <summary>
        /// Gets the target type for this navigator.
        /// </summary>
        public Type TargetType
        {
            get;
        }

        /// <summary>
        /// Gets the group index for this item.
        /// </summary>
        public int GroupIndex
        {
            get;
        }

        /// <summary>
        /// Gets the id associated with this navigation item.
        /// </summary>
        public long Id
        {
            get;
        }

        /// <summary>
        /// Gets or sets the geometry for the navigator item's icon.
        /// </summary>
        public Geometry IconGeometry
        {
            get => (Geometry)GetValue(IconGeometryProperty);
            set => SetValue(IconGeometryProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IconGeometry"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconGeometryProperty = DependencyProperty.Register
            (
                nameof(IconGeometry), typeof(Geometry), typeof(NavigatorItem), new PropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the icon brush
        /// </summary>
        internal Brush IconBrush
        {
            get => (Brush)GetValue(IconBrushProperty);
            set => SetValue(IconBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IconBrush"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register
            (
                nameof(IconBrush), typeof(Brush), typeof(NavigatorItem), new PropertyMetadata()
                {
                    DefaultValue = Brushes.Black
                }
            );

        /// <summary>
        /// Gets or sets a boolean value that determines if the item is visible.
        /// </summary>
        public bool IsItemVisible
        {
            get => (bool)GetValue(IsItemVisibleProperty);
            set => SetValue(IsItemVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsItemVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsItemVisibleProperty = DependencyProperty.Register
            (
                nameof(IsItemVisible), typeof(bool), typeof(NavigatorItem), new PropertyMetadata()
                {
                    DefaultValue = true
                }
            );

        /// <summary>
        /// Gets or sets the margin used internally for the template grid
        /// </summary>
        internal Thickness InternalGridMargin
        {
            get => (Thickness)GetValue(InternalGridMarginProperty);
            set => SetValue(InternalGridMarginProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="InternalGridMargin"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty InternalGridMarginProperty = DependencyProperty.Register
            (
                nameof(InternalGridMargin), typeof(Thickness), typeof(NavigatorItem), new PropertyMetadata()
                {
                    DefaultValue = new Thickness()
                }
            );
        #endregion

        /************************************************************************/

        #region Public methods
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{nameof(NavigatorItem)}: {Title}";
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when the right mouse button is pressed.
        /// </summary>
        /// <param name="e">The event args</param>
        /// <remarks>
        /// This method sets e.Handled = true to prevent a right-click
        /// from selecting the item.
        /// </remarks>
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.OnMouseRightButtonDown(e);
        }
        #endregion
    }
}