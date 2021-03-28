using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides a simple control that displays a single line item.
    /// </summary>
    public class LineItem : ContentControl
    {
        #region Private
        private const double MinItemDisplayWidth = 40.0;
        private const double DefaultItemDisplayWidth = 120.0;
        #endregion
        
        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LineItem"/> class.
        /// </summary>
        public LineItem()
        {
        }

        static LineItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineItem), new FrameworkPropertyMetadata(typeof(LineItem)));
        }
        #endregion

        /************************************************************************/

        #region ValueChanged routed event
        /// <summary>
        /// Occurs when the <see cref="Value"/> property changes.
        /// </summary>
        public event RoutedEventHandler ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        /// <summary>
        /// Identifies the <see cref="ValueChanged"/> routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent
            (
                nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LineItem)
            );

        #endregion

        /************************************************************************/

        #region Item
        /// <summary>
        /// Gets or sets the control header.
        /// </summary>
        public object Item
        {
            get => GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that displays the control header.
        /// </summary>
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register
            (
                nameof(Item), typeof(object), typeof(LineItem), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnItemChanged,
                }
            );

        private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LineItem control)
            {
                if (e.NewValue is string text)
                {
                    control.Item = new TextBlock() 
                    { 
                        Text = text,
                        Foreground = control.ItemForeground,
                        FontSize = control.ItemFontSize,
                    };
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the display slot for <see cref="Item"/>.
        /// </summary>
        public double ItemDisplayWidth
        {
            get => (double)GetValue(ItemDisplayWidthProperty);
            set => SetValue(ItemDisplayWidthProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that controls the width of <see cref="Item"/>.
        /// </summary>
        public static readonly DependencyProperty ItemDisplayWidthProperty = DependencyProperty.Register
            (
                nameof(ItemDisplayWidth), typeof(double), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = DefaultItemDisplayWidth,
                    PropertyChangedCallback = OnItemDisplayWidthChanged,
                    CoerceValueCallback = OnCoerceItemWidth
                }
            );

        private static void OnItemDisplayWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LineItem control)
            {
                control.ItemGridWidth = new GridLength((double)e.NewValue);
            }
        }

        private static object OnCoerceItemWidth(DependencyObject d, object baseValue)
        {
            if (baseValue is double w)
            {
                return Math.Max(w, MinItemDisplayWidth);
            }
            return baseValue;
        }

        /// <summary>
        /// Gets the grid width for the display slot of <see cref="Item"/>.
        /// </summary>
        public GridLength ItemGridWidth
        {
            get => (GridLength)GetValue(ItemGridWidthProperty);
            private set => SetValue(ItemGridWidthPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ItemGridWidthPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ItemGridWidth), typeof(GridLength), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = new GridLength(DefaultItemDisplayWidth)
                }
            );

        /// <summary>
        /// Identifies the <see cref="ItemGridWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemGridWidthProperty = ItemGridWidthPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the margin for the item
        /// </summary>
        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            private set => SetValue(ItemMarginPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ItemMarginPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ItemMargin), typeof(Thickness), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = new Thickness(0)
                }
            );

        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = ItemMarginPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets the foregound of <see cref="Item"/>.
        /// This property is only used when Item is set to a string value.
        /// </summary>
        public Brush ItemForeground
        {
            get => (Brush)GetValue(ItemForegroundProperty);
            set => SetValue(ItemForegroundProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the foreground of <see cref="Item"/>.
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty = DependencyProperty.Register
            (
                nameof(ItemForeground), typeof(Brush), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = Brushes.Black,
                    PropertyChangedCallback = OnItemTextPropertyChanged,
                }
            );

        /// <summary>
        /// Gets or sets the font size of <see cref="Item"/>.
        /// This property is only used when Item is set to a string value.
        /// </summary>
        public double ItemFontSize
        {
            get => (double)GetValue(ItemFontSizeProperty);
            set => SetValue(ItemFontSizeProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the font size of <see cref="Item"/>.
        /// </summary>
        public static readonly DependencyProperty ItemFontSizeProperty = DependencyProperty.Register
            (
                nameof(ItemFontSize), typeof(double), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = 11.0,
                    PropertyChangedCallback = OnItemTextPropertyChanged
                }
            );

        private static void OnItemTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LineItem control && control.Item is TextBlock text)
            {
                text.Foreground = control.ItemForeground;
                text.FontSize = control.ItemFontSize;
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment for <see cref="Item"/>
        /// </summary>
        public VerticalAlignment ItemVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(ItemVerticalAlignmentProperty);
            set => SetValue(ItemVerticalAlignmentProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the vertical alignment of the header.
        /// </summary>
        public static readonly DependencyProperty ItemVerticalAlignmentProperty = DependencyProperty.Register
            (
                nameof(ItemVerticalAlignment), typeof(VerticalAlignment), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = VerticalAlignment.Center
                }
            );

        /// <summary>
        /// Gets or sets the horizontal alignment for <see cref="Item"/>
        /// </summary>
        public HorizontalAlignment ItemHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(ItemHorizontalAlignmentProperty);
            set => SetValue(ItemHorizontalAlignmentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ItemHorizontalAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemHorizontalAlignmentProperty = DependencyProperty.Register
            (
                nameof(ItemHorizontalAlignment), typeof(HorizontalAlignment), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = HorizontalAlignment.Left
                }
            );
        #endregion

        /************************************************************************/

        #region Value
        /// <summary>
        /// Gets or sets the display value.
        /// </summary>
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that displays the control value.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
            (
                nameof(Value), typeof(object), typeof(LineItem), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = OnValueChanged
                }
            );

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LineItem control)
            {
                if (e.NewValue is string text)
                {
                    control.Value = new TextBlock() 
                    { 
                        Text = text, 
                        Foreground = control.ValueForeground,
                        FontSize = control.ValueFontSize,
                    };
                    return;
                }
                control.OnValueChanged(new RoutedEventArgs(ValueChangedEvent));
            }
        }

        /// <summary>
        /// Gets or sets the foregound of <see cref="Value"/>.
        /// This property is only used when Value is set to a string value.
        /// </summary>
        public Brush ValueForeground
        {
            get => (Brush)GetValue(ValueForegroundProperty);
            set => SetValue(ValueForegroundProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the foreground of <see cref="Value"/>.
        /// </summary>
        public static readonly DependencyProperty ValueForegroundProperty = DependencyProperty.Register
            (
                nameof(ValueForeground), typeof(Brush), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = Brushes.Black,
                    PropertyChangedCallback = OnValueTextPropertyChanged
                }
            );

        /// <summary>
        /// Gets or sets the font size of <see cref="Value"/>.
        /// This property is only used when Value is set to a string value.
        /// </summary>
        public double ValueFontSize
        {
            get => (double)GetValue(ValueFontSizeProperty);
            set => SetValue(ValueFontSizeProperty, value);
        }

        /// <summary>
        /// Defines a dependency property for the value font size.
        /// </summary>
        public static readonly DependencyProperty ValueFontSizeProperty = DependencyProperty.Register
            (
                nameof(ValueFontSize), typeof(double), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = 11.0,
                    PropertyChangedCallback = OnValueTextPropertyChanged
                }
            );

        private static void OnValueTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LineItem control && control.Value is TextBlock text)
            {
                text.Foreground = control.ValueForeground;
                text.FontSize = control.ValueFontSize;
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment for the value
        /// </summary>
        public VerticalAlignment ValueVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(ValueVerticalAlignmentProperty);
            set => SetValue(ValueVerticalAlignmentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ValueVerticalAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueVerticalAlignmentProperty = DependencyProperty.Register
            (
                nameof(ValueVerticalAlignment), typeof(VerticalAlignment), typeof(LineItem), new PropertyMetadata(VerticalAlignment.Center)
                {
                    DefaultValue = VerticalAlignment.Center
                }
            );

        /// <summary>
        /// Gets or sets the horizontal alignment for the value
        /// </summary>
        public HorizontalAlignment ValueHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(ValueHorizontalAlignmentProperty);
            set => SetValue(ValueHorizontalAlignmentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ValueHorizontalAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueHorizontalAlignmentProperty = DependencyProperty.Register
            (
                nameof(ValueHorizontalAlignment), typeof(HorizontalAlignment), typeof(LineItem), new PropertyMetadata()
                {
                    DefaultValue = HorizontalAlignment.Left
                }
            );
        #endregion

        /************************************************************************/

        #region DisplayLevel
        /// <summary>
        /// Gets or sets the indent level of <see cref="Item"/>.
        /// This property is clamped between zero and 4.
        /// </summary>
        public int IndentLevel
        {
            get => (int)GetValue(IndentLevelProperty);
            set => SetValue(IndentLevelProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the display level.
        /// </summary>
        public static readonly DependencyProperty IndentLevelProperty = DependencyProperty.Register
            (
                nameof(IndentLevel), typeof(int), typeof(LineItem), new PropertyMetadata(0, OnIndentLevelChanged, OnCoerceIndentLevel)
                {
                    DefaultValue = 0,
                    PropertyChangedCallback = OnIndentLevelChanged,
                    CoerceValueCallback = OnCoerceIndentLevel,
                }
            );

        private static void OnIndentLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LineItem control)
            {
                int indentLevel = (int)e.NewValue;
                double left = indentLevel * 12;
                control.ItemMargin = new Thickness(left, 0, 0, 0);
            }
        }

        private static object OnCoerceIndentLevel(DependencyObject d, object baseValue)
        {
            if (baseValue is int level)
            {
                return Math.Max(0, Math.Min(level, 4));
            }
            return baseValue;
        }

        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Raises the <see cref="ValueChangedEvent"/>.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnValueChanged(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }
        #endregion
    }
}
