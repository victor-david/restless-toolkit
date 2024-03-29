﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a text block that has a command associated with it.
    /// </summary>
    public class LinkedTextBlock : TextBlock, ICommandSource
    {
        #region Private
        private Brush originalForeground;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets or sets the command associated with the text block.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register
            (
                nameof(Command), typeof(ICommand), typeof(LinkedTextBlock), new PropertyMetadata(null, OnCommandChanged)
            );

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LinkedTextBlock)?.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register
            (
                nameof(CommandParameter), typeof(object), typeof(LinkedTextBlock), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets the target element on which to fire the command.
        /// This is a dependency property.
        /// </summary>
        public IInputElement CommandTarget
        {
            get => (IInputElement)GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register
            (
                nameof(CommandTarget), typeof(IInputElement), typeof(LinkedTextBlock), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets the brush used for rollover. This is a dependency property.
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
                nameof(RolloverBrush), typeof(Brush), typeof(LinkedTextBlock), new PropertyMetadata(Brushes.DarkRed)
            );
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedTextBlock"/> class.
        /// </summary>
        public LinkedTextBlock()
        {
            Cursor = Cursors.Hand;
            TextDecorations = System.Windows.TextDecorations.Underline;
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Occurs when the mouse enters the control.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            originalForeground = Foreground;
            Foreground = RolloverBrush;
        }

        /// <summary>
        /// Occurs when the mouse leaves the control.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Foreground = originalForeground;
        }

        /// <summary>
        /// Occurs when a mouse button is released.
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.ChangedButton == MouseButton.Left)
            {
                if (Command != null && Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
            {
                CanExecuteChangedEventManager.RemoveHandler(oldCommand, OnCanExecuteChanged);
            }
            if (newCommand != null)
            {
                CanExecuteChangedEventManager.AddHandler(newCommand, OnCanExecuteChanged);
            }
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            if (Command != null)
            {
                if (Command is RoutedCommand rc)
                {
                    IsEnabled = rc.CanExecute(CommandParameter, CommandTarget);
                }
                else
                {
                    IsEnabled = Command.CanExecute(CommandParameter);
                }
            }
        }
        #endregion
    }
}