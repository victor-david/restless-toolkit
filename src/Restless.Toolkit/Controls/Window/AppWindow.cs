﻿using Restless.Toolkit.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a custom window
    /// </summary>
    public class AppWindow : Window
    {
        #region Private
        private const double MinMenuItemHeight = 26.0;
        private const double MaxMenuItemHeight = 34.0;
        private const double DefMenuItemHeight = 32.0;
        #endregion

        /************************************************************************/

        #region Public consts
        /// <summary>
        /// Gets the minimum value for title bar height
        /// </summary>
        public const double MinTitleBarHeight = 26.0;
        /// <summary>
        /// Gets the maximum value for title bar height
        /// </summary>
        public const double MaxTitleBarHeight = 56.0;
        /// <summary>
        /// Gets the default value for title bar height
        /// </summary>
        public const double DefaultTitleBarHeight = 34.0;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AppWindow"/> class.
        /// </summary>
        public AppWindow()
        {
            MinimizeCommand = RelayCommand.Create((p) => WindowState = WindowState.Minimized);
            ChangeStateCommand = RelayCommand.Create(RunChangeStateCommand);
            CloseCommand = RelayCommand.Create((p) => Close());
            /* see comments in this method */
            SetMaxHeight();
            /* see comments in this method */
            SystemParameters.StaticPropertyChanged += SystemParametersStaticPropertyChanged;
            Loaded += (s, e) => OnLoaded(s, e);
            UseLayoutRounding = true;
            //RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            // Default for the following is: Auto, Ideal, Auto
            TextOptions.SetTextRenderingMode(this, TextRenderingMode.ClearType);
            TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);
            TextOptions.SetTextHintingMode(this, TextHintingMode.Fixed);
        }

        static AppWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppWindow), new FrameworkPropertyMetadata(typeof(AppWindow)));
        }
        #endregion

        /************************************************************************/

        #region Window menu properties
        /// <summary>
        /// Gets or sets the title bar menu.
        /// </summary>
        public Menu Menu
        {
            get => (Menu)GetValue(MenuProperty);
            set => SetValue(MenuProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Menu"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register
            (
                nameof(Menu), typeof(Menu), typeof(AppWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets the border brush for the title bar menu.
        /// </summary>
        public Brush MenuBorderBrush
        {
            get => (Brush)GetValue(MenuBorderBrushProperty);
            set => SetValue(MenuBorderBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MenuBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MenuBorderBrushProperty = DependencyProperty.Register
            (
                nameof(MenuBorderBrush), typeof(Brush), typeof(AppWindow), new PropertyMetadata(Brushes.Gray)
            );

        /// <summary>
        /// Gets or sets the background brush for the title bar menu.
        /// </summary>
        public Brush MenuBackgroundBrush
        {
            get => (Brush)GetValue(MenuBackgroundBrushProperty);
            set => SetValue(MenuBackgroundBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MenuBackgroundBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MenuBackgroundBrushProperty = DependencyProperty.Register
            (
                nameof(MenuBackgroundBrush), typeof(Brush), typeof(AppWindow), new PropertyMetadata(Brushes.LightGray)
            );

        /// <summary>
        /// Gets or sets the brush to use for a menu item that is checked.
        /// </summary>
        public Brush MenuHighlightBrush
        {
            get => (Brush)GetValue(MenuHighlightBrushProperty);
            set => SetValue(MenuHighlightBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MenuHighlightBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MenuHighlightBrushProperty = DependencyProperty.Register
            (
                nameof(MenuHighlightBrush), typeof(Brush), typeof(AppWindow), new PropertyMetadata(Brushes.Firebrick)
            );

        /// <summary>
        /// Gets or sets the opacity for the title bar menu.
        /// </summary>
        public double MenuOpacity
        {
            get => (double)GetValue(MenuOpacityProperty);
            set => SetValue(MenuOpacityProperty, value);
        }
        /// <summary>
        /// Identifies the <see cref="MenuOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MenuOpacityProperty = DependencyProperty.Register
            (
                nameof(MenuOpacity), typeof(double), typeof(AppWindow), new PropertyMetadata(1.0)
            );

        /// <summary>
        /// Gets or sets the height of menu items
        /// </summary>
        public double MenuItemHeight
        {
            get => (double)GetValue(MenuItemHeightProperty);
            set => SetValue(MenuItemHeightProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MenuItemHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MenuItemHeightProperty = DependencyProperty.Register
            (
                nameof(MenuItemHeight), typeof(double), typeof(AppWindow), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefMenuItemHeight,
                    CoerceValueCallback = (d, b) => Math.Max(Math.Min((double)b, MaxMenuItemHeight), MinMenuItemHeight)
                }
            );
        #endregion

        /************************************************************************/

        #region Window properties
        /// <summary>
        /// Gets or sets the tile bar height
        /// </summary>
        public double TitleBarHeight
        {
            get => (double)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register
            (
                nameof(TitleBarHeight), typeof(double), typeof(AppWindow), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultTitleBarHeight,
                    CoerceValueCallback = (d, b) => Math.Max(Math.Min((double)b, MaxTitleBarHeight), MinTitleBarHeight),
                    PropertyChangedCallback = (d, e) => (d as AppWindow)?.HandleTitleBarHeightChanged()
                }
            );

        /// <summary>
        /// Gets or sets the brush used for the background of the title bar.
        /// </summary>
        public Brush TitleBarBackground
        {
            get => (Brush)GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register
            (
                nameof(TitleBarBackground), typeof(Brush), typeof(AppWindow), new PropertyMetadata(Brushes.Black)
            );


        /// <summary>
        /// Gets or sets the brush used for the foreground of the title bar
        /// </summary>
        public Brush TitleBarForeground
        {
            get => (Brush)GetValue(TitleBarForegroundProperty);
            set => SetValue(TitleBarForegroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarForeground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBarForegroundProperty = DependencyProperty.Register
            (
                nameof(TitleBarForeground), typeof(Brush), typeof(AppWindow), new PropertyMetadata(Brushes.White)
            );

        /// <summary>
        /// Gets or sets the brush used for the buttons in the title bar.
        /// </summary>
        public Brush TitleBarButtonBrush
        {
            get => (Brush)GetValue(TitleBarButtonBrushProperty);
            set => SetValue(TitleBarButtonBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarButtonBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBarButtonBrushProperty = DependencyProperty.Register
            (
                nameof(TitleBarButtonBrush), typeof(Brush), typeof(AppWindow), new PropertyMetadata(Brushes.White)
            );

        /// <summary>
        /// Gets or sets the brush used for the bottom border of the title bar.
        /// </summary>
        public Brush TitleBarBorderBrush
        {
            get => (Brush)GetValue(TitleBarBorderBrushProperty);
            set => SetValue(TitleBarBorderBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TitleBarBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleBarBorderBrushProperty = DependencyProperty.Register
            (
                nameof(TitleBarBorderBrush), typeof(Brush), typeof(AppWindow), new PropertyMetadata(Brushes.Black)
            );

        /// <summary>
        /// Gets or sets a boolean value that determines if the title bar back button is visible.
        /// </summary>
        public bool IsBackButtonVisible
        {
            get => (bool)GetValue(IsBackButtonVisibleProperty);
            set => SetValue(IsBackButtonVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsBackButtonVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBackButtonVisibleProperty = DependencyProperty.Register
            (
                nameof(IsBackButtonVisible), typeof(bool), typeof(AppWindow), new PropertyMetadata(false)
            );

        /// <summary>
        /// Gets or sets a boolean value that determines if the title bar back button is enabled.
        /// </summary>
        public bool IsBackButtonEnabled
        {
            get => (bool)GetValue(IsBackButtonEnabledProperty);
            set => SetValue(IsBackButtonEnabledProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsBackButtonEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBackButtonEnabledProperty = DependencyProperty.Register
            (
                nameof(IsBackButtonEnabled), typeof(bool), typeof(AppWindow), new PropertyMetadata(false)
            );

        /// <summary>
        /// Gets or sets the title bar back button icon brush
        /// </summary>
        public Brush BackButtonIconBrush
        {
            get => (Brush)GetValue(BackButtonIconBrushProperty);
            set => SetValue(BackButtonIconBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="BackButtonIconBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BackButtonIconBrushProperty = DependencyProperty.Register
            (
                nameof(BackButtonIconBrush), typeof(Brush), typeof(AppWindow), new PropertyMetadata(Brushes.OrangeRed)
            );

        /// <summary>
        /// Gets or sets the command to associate with the back button.
        /// </summary>
        public ICommand BackButtonCommand
        {
            get => (ICommand)GetValue(BackButtonCommandProperty);
            set => SetValue(BackButtonCommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="BackButtonCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BackButtonCommandProperty = DependencyProperty.Register
            (
                nameof(BackButtonCommand), typeof(ICommand), typeof(AppWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets an icon defined by a path
        /// </summary>
        public FrameworkElement PathIcon
        {
            get => (FrameworkElement)GetValue(PathIconProperty);
            set => SetValue(PathIconProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="PathIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PathIconProperty = DependencyProperty.Register
            (
                nameof(PathIcon), typeof(FrameworkElement), typeof(AppWindow), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region Commands
        /// <summary>
        /// Gets the minimize command.
        /// </summary>
        public ICommand MinimizeCommand
        {
            get => (ICommand)GetValue(MinimizeCommandProperty);
            private set => SetValue(MinimizeCommandPropertyKey, value);
        }

        private static readonly DependencyPropertyKey MinimizeCommandPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(MinimizeCommand), typeof(ICommand), typeof(AppWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="MinimizeCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimizeCommandProperty = MinimizeCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the change state command.
        /// </summary>
        public ICommand ChangeStateCommand
        {
            get => (ICommand)GetValue(ChangeStateCommandProperty);
            private set => SetValue(ChangeStateCommandPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ChangeStateCommandPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ChangeStateCommand), typeof(ICommand), typeof(AppWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="ChangeStateCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChangeStateCommandProperty = ChangeStateCommandPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the close command.
        /// </summary>
        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            private set => SetValue(CloseCommandPropertyKey, value);
        }

        private static readonly DependencyPropertyKey CloseCommandPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(CloseCommand), typeof(ICommand), typeof(AppWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="CloseCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CloseCommandProperty = CloseCommandPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when the window is loaded. Override if needed. Always call the base method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args</param>
        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            HandleTitleBarHeightChanged();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void RunChangeStateCommand(object parm)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void SystemParametersStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            /* System parameters can change while the app is running. User can set task bar to auto hide.
             * This increases the maxium height available. Or vice-versa if user removes task bar auto hide.
             */
            if (e.PropertyName == nameof(SystemParameters.MaximizedPrimaryScreenHeight))
            {
                SetMaxHeight();
            }
        }

        private void SetMaxHeight()
        {
            /* Prevents window from extending beneath task bar when maximized, but could be problem with multiple monitors.
             * Why -2? Otherwise, the window is still two pixels under the task bar. Don't know why.
             */
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight - 2;
        }

        private void HandleTitleBarHeightChanged()
        {
            if (WindowChrome.GetWindowChrome(this) is WindowChrome chrome)
            {
                chrome.CaptionHeight = TitleBarHeight - 4;
            }
        }
        #endregion
    }
}