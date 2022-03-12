using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a control that may be in one of three states
    /// </summary>
    public class ThreeWay : Control
    {
        #region Public fields
        /// <summary>
        /// Minimum value for <see cref="SelectorHeight"/>
        /// </summary>
        public const double MinSelectorHeight = 16;

        /// <summary>
        /// Maximum value for <see cref="SelectorHeight"/>
        /// </summary>
        public const double MaxSelectorHeight = 64;

        /// <summary>
        /// Default value for <see cref="SelectorHeight"/>
        /// </summary>
        public const double DefaultSelectorHeight = 24;

        /// <summary>
        /// Mimimum value for <see cref="MinSelectorAreaWidth"/>
        /// </summary>
        public const double MinMinSelectorAreaWidth = 48;

        /// <summary>
        /// Default value for <see cref="MinSelectorAreaWidth"/>
        /// </summary>
        public const double DefaultMinSelectorAreaWidth = 96;

        /// <summary>
        /// Default value for <see cref="SelectorRadius"/>
        /// </summary>
        public const double DefaultSelectorRadius = 0;

        /// <summary>
        /// Identifies the default resource key for <see cref="HeaderStyle"/>
        /// </summary>
        public static readonly ComponentResourceKey HeaderStyleKey = new ComponentResourceKey(typeof(ThreeWay), nameof(HeaderStyleKey));

        /// <summary>
        /// Identifies the default resource key for <see cref="OnOffStyle"/>
        /// </summary>
        public static readonly ComponentResourceKey OnOffStyleKey = new ComponentResourceKey(typeof(ThreeWay), nameof(OnOffStyleKey));
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initialzies a new instance of the <see cref="ThreeWay"/> class.
        /// </summary>
        public ThreeWay()
        {
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ClickEventHandler));
        }

        static ThreeWay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThreeWay), new FrameworkPropertyMetadata(typeof(ThreeWay)));
        }
        #endregion

        /************************************************************************/

        #region State
        /// <summary>
        /// Gets or sets the state of the control
        /// </summary>
        public ThreeWayState State
        {
            get => (ThreeWayState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="State"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register
            (
                nameof(State), typeof(ThreeWayState), typeof(ThreeWay), new FrameworkPropertyMetadata()
                {
                    DefaultValue = ThreeWayState.Neutral,
                    BindsTwoWayByDefault = true
                }
            );
        #endregion

        /************************************************************************/

        #region Labels
        /// <summary>
        /// Gets or sets the control title
        /// </summary>
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register
            (
                nameof(Header), typeof(string), typeof(ThreeWay), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the style to apply to <see cref="Header"/>
        /// </summary>
        public Style HeaderStyle
        {
            get => (Style)GetValue(HeaderStyleProperty);
            set => SetValue(HeaderStyleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HeaderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register
            (
                nameof(HeaderStyle), typeof(Style), typeof(ThreeWay), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the text used for the on position
        /// </summary>
        public string OnText
        {
            get => (string)GetValue(OnTextProperty);
            set => SetValue(OnTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OnText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OnTextProperty = DependencyProperty.Register
            (
                nameof(OnText), typeof(string), typeof(ThreeWay), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the text used for the off position
        /// </summary>
        public string OffText
        {
            get => (string)GetValue(OffTextProperty);
            set => SetValue(OffTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OffText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffTextProperty = DependencyProperty.Register
            (
                nameof(OffText), typeof(string), typeof(ThreeWay), new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the style to apply to <see cref="OnText"/> and <see cref="OffText"/>
        /// </summary>
        public Style OnOffStyle
        {
            get => (Style)GetValue(OnOffStyleProperty);
            set => SetValue(OnOffStyleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OnOffStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OnOffStyleProperty = DependencyProperty.Register
            (
                nameof(OnOffStyle), typeof(Style), typeof(ThreeWay), new FrameworkPropertyMetadata()
            );
        #endregion

        /************************************************************************/

        #region Dimensions
        /// <summary>
        /// Gets or sets the selector height
        /// </summary>
        public double SelectorHeight
        {
            get => (double)GetValue(SelectorHeightProperty);
            set => SetValue(SelectorHeightProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectorHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectorHeightProperty = DependencyProperty.Register
            (
                nameof(SelectorHeight), typeof(double), typeof(ThreeWay), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultSelectorHeight,
                    CoerceValueCallback = OnCoerceSelectorHeight
                }
            );

        private static object OnCoerceSelectorHeight(DependencyObject d, object baseValue)
        {
            return Math.Max(Math.Min((double)baseValue, MaxSelectorHeight), MinSelectorHeight);
        }


        /// <summary>
        /// Gets or sets the minimum width for the selector area
        /// </summary>
        public double MinSelectorAreaWidth
        {
            get => (double)GetValue(MinSelectorAreaWidthProperty);
            set => SetValue(MinSelectorAreaWidthProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MinSelectorAreaWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinSelectorAreaWidthProperty = DependencyProperty.Register
            (
                nameof(MinSelectorAreaWidth), typeof(double), typeof(ThreeWay), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultMinSelectorAreaWidth,
                    CoerceValueCallback = OnCoerceMinSelectorAreaWidth
                }
            );

        private static object OnCoerceMinSelectorAreaWidth(DependencyObject d, object baseValue)
        {
            return Math.Max((double)baseValue, MinMinSelectorAreaWidth);
        }


        /// <summary>
        /// Gets or sets the selector radius
        /// </summary>
        public double SelectorRadius
        {
            get => (double)GetValue(SelectorRadiusProperty);
            set => SetValue(SelectorRadiusProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectorRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectorRadiusProperty = DependencyProperty.Register
            (
                nameof(SelectorRadius), typeof(double), typeof(ThreeWay), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultSelectorRadius,
                    CoerceValueCallback = OnCoerceSelectorRadius,
                    PropertyChangedCallback = OnSelectorRadiusPropertyChanged
                }
            );

        private static object OnCoerceSelectorRadius(DependencyObject d, object baseValue)
        {
            return Math.Max((double)baseValue, 0);
        }

        private static void OnSelectorRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ThreeWay control)
            {
                double value = (double)e.NewValue;
                control.SelectorAreaCornerRadius = new CornerRadius(value);
                control.OnCornerRadius = new CornerRadius(0, value, value, 0);
                control.OffCornerRadius = new CornerRadius(value, 0, 0, value);
            }
        }

        /// <summary>
        /// Gets the corner radius used for the selector area
        /// </summary>
        public CornerRadius SelectorAreaCornerRadius
        {
            get => (CornerRadius)GetValue(SelectorAreaCornerRadiusProperty);
            private set => SetValue(SelectorAreaCornerRadiusPropertyKey, value);
        }

        private static readonly DependencyPropertyKey SelectorAreaCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(SelectorAreaCornerRadius), typeof(CornerRadius), typeof(ThreeWay), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new CornerRadius(DefaultSelectorRadius)
                }
            );

        /// <summary>
        /// Identifies the <see cref="SelectorAreaCornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectorAreaCornerRadiusProperty = SelectorAreaCornerRadiusPropertyKey.DependencyProperty;




        /// <summary>
        /// Gets the...
        /// </summary>
        public CornerRadius OnCornerRadius
        {
            get => (CornerRadius)GetValue(OnCornerRadiusProperty);
            private set => SetValue(OnCornerRadiusPropertyKey, value);
        }

        private static readonly DependencyPropertyKey OnCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(OnCornerRadius), typeof(CornerRadius), typeof(ThreeWay), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new CornerRadius(0, DefaultSelectorRadius, DefaultSelectorRadius, 0)
                }
            );

        /// <summary>
        /// Identifies the <see cref="OnCornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OnCornerRadiusProperty = OnCornerRadiusPropertyKey.DependencyProperty;


        /// <summary>
        /// Gets the...
        /// </summary>
        public CornerRadius OffCornerRadius
        {
            get => (CornerRadius)GetValue(OffCornerRadiusProperty);
            private set => SetValue(OffCornerRadiusPropertyKey, value);
        }

        private static readonly DependencyPropertyKey OffCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(OffCornerRadius), typeof(CornerRadius), typeof(ThreeWay), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new CornerRadius(DefaultSelectorRadius, 0 ,0, DefaultSelectorRadius)
                }
            );

        /// <summary>
        /// Identifies the <see cref="OffCornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffCornerRadiusProperty = OffCornerRadiusPropertyKey.DependencyProperty;


        #endregion

        /************************************************************************/

        #region Brushes
        /// <summary>
        /// Gets or sets the brush used when the control is in the neutral state
        /// </summary>
        public Brush NeutralBrush
        {
            get => (Brush)GetValue(NeutralBrushProperty);
            set => SetValue(NeutralBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="NeutralBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NeutralBrushProperty = DependencyProperty.Register
            (
                nameof(NeutralBrush), typeof(Brush), typeof(ThreeWay), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Brushes.Transparent
                }
            );

        /// <summary>
        /// Gets or sets the brush used when the control is in the off state
        /// </summary>
        public Brush OffBrush
        {
            get => (Brush)GetValue(OffBrushProperty);
            set => SetValue(OffBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OffBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffBrushProperty = DependencyProperty.Register
            (
                nameof(OffBrush), typeof(Brush), typeof(ThreeWay), new FrameworkPropertyMetadata()
                { 
                    DefaultValue = Brushes.Transparent
                }
            );

        /// <summary>
        /// Gets or sets the brush used when the control is in the on state
        /// </summary>
        public Brush OnBrush
        {
            get => (Brush)GetValue(OnBrushProperty);
            set => SetValue(OnBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OnBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OnBrushProperty = DependencyProperty.Register
            (
                nameof(OnBrush), typeof(Brush), typeof(ThreeWay), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Brushes.Transparent
                }
            );
        #endregion

        /************************************************************************/

        #region Private methods
        private void ClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is ThreeWayButton button)
            {
                State = button.State;
                e.Handled = true;
            }
        }
        #endregion
    }
}