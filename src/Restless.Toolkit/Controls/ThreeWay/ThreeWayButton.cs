using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a button used in a <see cref="ThreeWay"/> control
    /// </summary>
    public class ThreeWayButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeWayButton"/> class.
        /// </summary>
        public ThreeWayButton()
        {
        }

        static ThreeWayButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThreeWayButton), new FrameworkPropertyMetadata(typeof(ThreeWayButton)));
        }

        /// <summary>
        /// Gets or sets the corner radius
        /// </summary>
        internal CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            (
                nameof(CornerRadius), typeof(CornerRadius), typeof(ThreeWayButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new CornerRadius()
                }
            );

        /// <summary>
        /// Gets or sets...
        /// </summary>
        internal ThreeWayState State
        {
            get => (ThreeWayState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="State"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty StateProperty = DependencyProperty.Register
            (
                nameof(State), typeof(ThreeWayState), typeof(ThreeWayButton), new FrameworkPropertyMetadata()
                {
                    DefaultValue = ThreeWayState.Neutral
                }
            );
    }
}