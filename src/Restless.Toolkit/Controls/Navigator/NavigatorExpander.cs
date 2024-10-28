using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents an expander used in navigators. Expands down only.
    /// </summary>
    public class NavigatorExpander : HeaderedContentControl
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatorExpander"/> class.
        /// </summary>
        public NavigatorExpander()
        {
        }

        static NavigatorExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigatorExpander), new FrameworkPropertyMetadata(typeof(NavigatorExpander)));
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets whether the control is expanded
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
                nameof(IsExpanded), typeof(bool), typeof(NavigatorExpander), new FrameworkPropertyMetadata()
                {
                    DefaultValue = false,
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnIconStateChanged
                }
            );

        /// <summary>
        /// Gets or sets the icon when not expanded.
        /// </summary>
        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Icon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register
            (
                nameof(Icon), typeof(object), typeof(NavigatorExpander), new FrameworkPropertyMetadata()
                {
                    PropertyChangedCallback = OnIconStateChanged
                }
            );

        /// <summary>
        /// Gets or sets the icon when expanded
        /// </summary>
        public object ExpandedIcon
        {
            get => GetValue(ExpandedIconProperty);
            set => SetValue(ExpandedIconProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ExpandedIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpandedIconProperty = DependencyProperty.Register
            (
                nameof(ExpandedIcon), typeof(object), typeof(NavigatorExpander), new FrameworkPropertyMetadata()
                {
                    PropertyChangedCallback = OnIconStateChanged
                }
            );

        /// <summary>
        /// Gets or sets the margin for the expander button
        /// </summary>
        public Thickness ButtonMargin
        {
            get => (Thickness)GetValue(ButtonMarginProperty);
            set => SetValue(ButtonMarginProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ButtonMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonMarginProperty = DependencyProperty.Register
            (
                nameof(ButtonMargin), typeof(Thickness), typeof(NavigatorExpander), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new Thickness(0, 0, 10, 0)
                }
            );

        /// <summary>
        /// Gets or sets the cursor for the expander button
        /// </summary>
        public Cursor ButtonCursor
        {
            get => (Cursor)GetValue(ButtonCursorProperty);
            set => SetValue(ButtonCursorProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ButtonCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonCursorProperty = DependencyProperty.Register
            (
                nameof(ButtonCursor), typeof(Cursor), typeof(NavigatorExpander), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Cursors.Hand
                }
            );
        #endregion

        /************************************************************************/

        #region Properties (internal)
        /// <summary>
        /// Gets or sets the active icon.
        /// </summary>
        internal object ActiveIcon
        {
            get => GetValue(ActiveIconProperty);
            set => SetValue(ActiveIconProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ActiveIcon"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty ActiveIconProperty = DependencyProperty.Register
            (
                nameof(ActiveIcon), typeof(object), typeof(NavigatorExpander), new FrameworkPropertyMetadata()
            );
        #endregion

        /************************************************************************/

        #region Private
        private static void OnIconStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NavigatorExpander)?.HandleIconStateChanged();
        }

        private void HandleIconStateChanged()
        {
            ActiveIcon = IsExpanded ? ExpandedIcon : Icon;
        }
        #endregion
    }
}