using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a button that also contains an icon
    /// </summary>
    public class IconButton : Button
    {
        #region Private
        private const double DefaultPadding = 4;
        private const double DefaultHorizontalPressedMovement = 1.0;
        private const double DefaultVerticalPressedMovement = 0.0;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Creates a new instance of the <see cref="IconButton"/> class.
        /// </summary>
        public IconButton()
        {
        }

        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
            PaddingProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata()
            {
                DefaultValue = new Thickness(DefaultPadding),
                PropertyChangedCallback = OnPaddingChanged
            });
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets an icon to display with the button.
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
                nameof(Icon), typeof(object), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = null
                }
            );

        /// <summary>
        /// Gets or sets the padding used on the icon
        /// </summary>
        public Thickness IconPadding
        {
            get => (Thickness)GetValue(IconPaddingProperty);
            set => SetValue(IconPaddingProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IconPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconPaddingProperty = DependencyProperty.Register
            (
                nameof(IconPadding), typeof(Thickness), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = new Thickness(DefaultPadding),
                    PropertyChangedCallback = OnPaddingChanged
                }
            );

        /// <summary>
        /// Gets or sets the horizonatal alignment for the icon
        /// </summary>
        public HorizontalAlignment HorizontalIconAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalIconAlignmentProperty);
            set => SetValue(HorizontalIconAlignmentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HorizontalIconAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalIconAlignmentProperty = DependencyProperty.Register
            (
                nameof(HorizontalIconAlignment), typeof(HorizontalAlignment), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = HorizontalAlignment.Center
                }
            );

        /// <summary>
        /// Gets or sets the vertical alignment for the icon
        /// </summary>
        public VerticalAlignment VerticalIconAlignment
        {
            get => (VerticalAlignment)GetValue(VerticalIconAlignmentProperty);
            set => SetValue(VerticalIconAlignmentProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="VerticalIconAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalIconAlignmentProperty = DependencyProperty.Register
            (
                nameof(VerticalIconAlignment), typeof(VerticalAlignment), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = VerticalAlignment.Center
                }
            );

        /// <summary>
        /// Gets or sets a brush to use on button rollover
        /// </summary>
        public Brush RolloverBrush
        {
            get => (Brush)GetValue(RolloverBrushProperty);
            set => SetValue(RolloverBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="RolloverBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RolloverBrushProperty = DependencyProperty.Register
            (
                nameof(RolloverBrush), typeof(Brush), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = Brushes.Transparent,
                    CoerceValueCallback = OnCoerceRolloverBrush
                }
            );

        private static object OnCoerceRolloverBrush(DependencyObject d, object baseValue)
        {
            return baseValue ?? Brushes.Transparent;
        }

        /// <summary>
        /// Gets or sets the corner radius for the button.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            (
                nameof(CornerRadius), typeof(CornerRadius), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = new CornerRadius(2)
                }
            );

        /// <summary>
        /// Gets or sets the horizontal movement when the button is pressed.
        /// </summary>
        public double HorizontalPressedMovement
        {
            get => (double)GetValue(HorizontalPressedMovementProperty);
            set => SetValue(HorizontalPressedMovementProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HorizontalPressedMovement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalPressedMovementProperty = DependencyProperty.Register
            (
                nameof(HorizontalPressedMovement), typeof(double), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = DefaultHorizontalPressedMovement,
                    CoerceValueCallback = OnCoercePressedMovement,
                    PropertyChangedCallback = OnPressedMovementChanged
                }
            );

        /// <summary>
        /// Gets or sets the vertical movement when the button is pressed.
        /// </summary>
        public double VerticalPressedMovement
        {
            get => (double)GetValue(VerticalPressedMovementProperty);
            set => SetValue(VerticalPressedMovementProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="VerticalPressedMovement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalPressedMovementProperty = DependencyProperty.Register
            (
                nameof(VerticalPressedMovement), typeof(double), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = DefaultVerticalPressedMovement,
                    CoerceValueCallback = OnCoercePressedMovement,
                    PropertyChangedCallback = OnPressedMovementChanged
                }
            );

        private static object OnCoercePressedMovement(DependencyObject d, object baseValue)
        {
            return baseValue is double value ? Math.Max(Math.Min(5.0, value), -5.0) : baseValue;
        }

        private static void OnPressedMovementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconButton button)
            {
                button.SetValue(PressedPaddingPropertyKey, GetPressedPadding(button, button.Padding));
                button.SetValue(IconPressedPaddingPropertyKey, GetPressedPadding(button, button.IconPadding));
            }
        }

        /// <summary>
        /// Gets the icon padding when pressed.
        /// </summary>
        public Thickness IconPressedPadding
        {
            get => (Thickness)GetValue(IconPressedPaddingProperty);
            private set => SetValue(IconPressedPaddingPropertyKey, value);
        }

        private static readonly DependencyPropertyKey IconPressedPaddingPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(IconPressedPadding), typeof(Thickness), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = new Thickness()
                    {
                        Left = DefaultPadding + DefaultHorizontalPressedMovement,
                        Right = DefaultPadding - DefaultHorizontalPressedMovement,
                        Top = DefaultPadding + DefaultVerticalPressedMovement,
                        Bottom = DefaultPadding - DefaultVerticalPressedMovement
                    }
                }
            );

        /// <summary>
        /// Identifies the <see cref="IconPressedPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconPressedPaddingProperty = IconPressedPaddingPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the padding when pressed.
        /// </summary>
        public Thickness PressedPadding
        {
            get => (Thickness)GetValue(PressedPaddingProperty);
            private set => SetValue(PressedPaddingPropertyKey, value);
        }

        private static readonly DependencyPropertyKey PressedPaddingPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(PressedPadding), typeof(Thickness), typeof(IconButton), new PropertyMetadata()
                {
                    DefaultValue = new Thickness()
                    {
                        Left = DefaultPadding + DefaultHorizontalPressedMovement,
                        Right = DefaultPadding - DefaultHorizontalPressedMovement,
                        Top = DefaultPadding + DefaultVerticalPressedMovement,
                        Bottom = DefaultPadding - DefaultVerticalPressedMovement
                    }
                }
            );

        /// <summary>
        /// Identifies the <see cref="PressedPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PressedPaddingProperty = PressedPaddingPropertyKey.DependencyProperty;

        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconButton button)
            {
                if (e.Property.Name == nameof(Padding))
                {
                    button.SetValue(PressedPaddingPropertyKey, GetPressedPadding(button, (Thickness)e.NewValue));
                }
                if (e.Property.Name == nameof(IconPadding))
                {
                    button.SetValue(IconPressedPaddingPropertyKey, GetPressedPadding(button, (Thickness)e.NewValue));
                }
            }
        }

        private static Thickness GetPressedPadding(IconButton button, Thickness thickness)
        {
            return new Thickness()
            {
                Left = thickness.Left + button.HorizontalPressedMovement,
                Right = thickness.Right - button.HorizontalPressedMovement,
                Top = thickness.Top + button.VerticalPressedMovement,
                Bottom = thickness.Bottom - button.VerticalPressedMovement
            };
        }
        #endregion
    }
}