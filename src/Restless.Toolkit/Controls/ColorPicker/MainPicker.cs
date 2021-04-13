﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents the main picker panel
    /// </summary>
    internal class MainPicker : Control
    {
        #region Private
        private readonly MainPickerAdorner adorner;
        private bool isAdornerPositionSyncSuspended;
        private bool isColorComponentChangedEventSuspended;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPicker"/> class.
        /// </summary>
        public MainPicker()
        {
            adorner = new MainPickerAdorner(this);
            InitializePanelBrush();
            Loaded += MainPickerLoaded;
        }

        static MainPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainPicker), new FrameworkPropertyMetadata(typeof(MainPicker)));
        }
        #endregion

        /************************************************************************/

        #region Events
        /// <summary>
        /// Raised when any of the color components (hue, saturation, or brightness) changes.
        /// </summary>
        public event EventHandler<ColorComponents> ColorComponentChanged;
        #endregion

        /************************************************************************/

        #region Properties (color components)
        /// <summary>
        /// Gets or sets the value of the color hue component.
        /// </summary>
        public double Hue
        {
            get => (double)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Hue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register
            (
                nameof(Hue), typeof(double), typeof(MainPicker), new PropertyMetadata()
                {
                    DefaultValue = ColorValues.MinHue,
                    CoerceValueCallback = OnCoerceHueProperty,
                    PropertyChangedCallback = OnColorComponentPropertyChanged
                }
            );

        private static object OnCoerceHueProperty(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Min(Math.Max(value, ColorValues.MinHue), ColorValues.MaxHue);
        }

        /// <summary>
        /// Gets the value of the color saturation component.
        /// </summary>
        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            private set => SetValue(SaturationPropertyKey, value);
        }

        private static readonly DependencyPropertyKey SaturationPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(Saturation), typeof(double), typeof(MainPicker), new PropertyMetadata()
                {
                    DefaultValue = ColorValues.MaxSaturation,
                    CoerceValueCallback = OnCoerceSaturationProperty,
                    PropertyChangedCallback = OnColorComponentPropertyChanged
                }
            );

        /// <summary>
        /// Identifies the <see cref="Saturation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaturationProperty = SaturationPropertyKey.DependencyProperty;

        private static object OnCoerceSaturationProperty(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Min(Math.Max(value, ColorValues.MinSaturation), ColorValues.MaxSaturation);
        }

        /// <summary>
        /// Gets the value of the color brightness component.
        /// </summary>
        public double Brightness
        {
            get => (double)GetValue(BrightnessProperty);
            private set => SetValue(BrightnessPropertyKey, value);
        }

        private static readonly DependencyPropertyKey BrightnessPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(Brightness), typeof(double), typeof(MainPicker), new PropertyMetadata()
                {
                    DefaultValue = ColorValues.MaxBrightness,
                    CoerceValueCallback = OnCoerceBrightnessProperty,
                    PropertyChangedCallback = OnColorComponentPropertyChanged
                }
            );

        /// <summary>
        /// Identifies the <see cref="Brightness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BrightnessProperty = BrightnessPropertyKey.DependencyProperty;

        private static object OnCoerceBrightnessProperty(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Min(Math.Max(value, ColorValues.MinBrightness), ColorValues.MaxBrightness);
        }

        private static void OnColorComponentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainPicker control)
            {
                if (e.Property.Name == nameof(Hue))
                {
                    control.UpdatePanelBrush();
                }
                control.OnColorComponentChanged();
            }
        }
        #endregion

        /************************************************************************/

        #region Properties (brushes)
        /// <summary>
        /// Gets the brush for the main picker panel.
        /// </summary>
        public LinearGradientBrush PanelBrush
        {
            get => (LinearGradientBrush)GetValue(PanelBrushProperty);
            private set => SetValue(PanelBrushPropertyKey, value);
        }

        private static readonly DependencyPropertyKey PanelBrushPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(PanelBrush), typeof(LinearGradientBrush), typeof(MainPicker), new PropertyMetadata()
            );

        /// <summary>
        /// Identifies the <see cref="PanelBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PanelBrushProperty = PanelBrushPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Public methods (although public, this is an internal class)
        /// <summary>
        /// Updates the color components (hue, saturation, and brightness)
        /// according to the specified color.
        /// </summary>
        /// <param name="color">The color</param>
        public void UpdateColorComponents(Color color)
        {
            SuspendColorComponentChangedEvent();
            Hue = color.GetHue();
            Saturation = color.GetSaturation();
            Brightness = color.GetBrightness();
            SyncAdornerPositionToColorComponents();
            ResumeColorComponentChangedEvent();
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called on mouse movement to update the adorner and color components.
        /// </summary>
        /// <param name="e">Mouse args.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Mouse.Capture(this);
                Point point = e.GetPosition(this).ClampToElement(this);
                SyncAdornerPositionToMousePosition(point);
                SyncColorComponentsToMousePosition(point);
            }
        }

        /// <summary>
        /// Called on mouse up to update the adorner and color components.
        /// </summary>
        /// <param name="e">Mouse args.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            Mouse.Capture(null);
            Point point = e.GetPosition(this).ClampToElement(this);
            SyncAdornerPositionToMousePosition(point);
            SyncColorComponentsToMousePosition(point);
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void MainPickerLoaded(object sender, RoutedEventArgs e)
        {
            if (AdornerLayer.GetAdornerLayer(this) is AdornerLayer layer)
            {
                layer.Add(adorner);
                SyncAdornerPositionToColorComponents();
            }
        }

        /// <summary>
        /// Called when any of the color components changes in order to raise
        /// the <see cref="ColorComponentChanged"/> event.
        /// </summary>
        private void OnColorComponentChanged()
        {
            if (!isColorComponentChangedEventSuspended)
            {
                ColorComponentChanged?.Invoke(this, new ColorComponents(Hue, Saturation, Brightness));
            }
        }

        /// <summary>
        /// Prevents the <see cref="ColorComponentChanged"/> event from being raised.
        /// Suspension occurs when the components are being changed by <see cref="ColorPicker"/>
        /// (via the <see cref="UpdateColorComponents(Color)"/> method) instead of from
        /// within this class.
        /// </summary>
        private void SuspendColorComponentChangedEvent()
        {
            isColorComponentChangedEventSuspended = true;
        }

        /// <summary>
        /// Resumes normal operation of the <see cref="ColorComponentChanged"/> event.
        /// </summary>
        private void ResumeColorComponentChangedEvent()
        {
            isColorComponentChangedEventSuspended = false;
        }

        /// <summary>
        /// Sets the adorner position according to the position of the mouse.
        /// This method is called from mouse movement methods
        /// </summary>
        /// <param name="clampedPoint">The mouse position (clamped to be inside of this control)</param>
        private void SyncAdornerPositionToMousePosition(Point clampedPoint)
        {
            adorner.Position = clampedPoint;
        }

        /// <summary>
        /// Sets <see cref="Saturation"/> and <see cref="Brightness"/> according to
        /// the specified mouse position.
        /// </summary>
        /// <param name="clampedPoint">The mouse position (clamped to be inside of this control)</param>
        private void SyncColorComponentsToMousePosition(Point clampedPoint)
        {
            SuspendAdornerPositionSync();
            Saturation = clampedPoint.X / ActualWidth;
            Brightness = 1 - (clampedPoint.Y / ActualHeight);
            ResumeAdornerPositionSync();
        }

        /// <summary>
        /// Sets the adorner position according to the current values of <see cref="Saturation"/>
        /// and <see cref="Brightness"/>.
        /// </summary>
        private void SyncAdornerPositionToColorComponents()
        {
            if (!isAdornerPositionSyncSuspended)
            {
                adorner.Position = new Point(Saturation * ActualWidth, (1 - Brightness) * ActualHeight);
            }
        }

        /// <summary>
        /// Prohibits <see cref="SyncAdornerPositionToColorComponents"/> from updating the adorner
        /// position. Prevents <see cref="Saturation"/> and <see cref="Brightness"/> callbacks
        /// from competing with mouse updates that (if not prohibited) result in adorner movement jitters.
        /// </summary>
        private void SuspendAdornerPositionSync()
        {
            isAdornerPositionSyncSuspended = true;
        }

        /// <summary>
        /// Enables <see cref="SyncAdornerPositionToColorComponents"/> to update the adorner position again.
        /// </summary>
        private void ResumeAdornerPositionSync()
        {
            isAdornerPositionSyncSuspended = false;
        }

        /// <summary>
        /// Sets the <see cref="PanelBrush"/> end gradient in response to a change 
        /// on the <see cref="Hue"/> property.
        /// </summary>
        private void UpdatePanelBrush()
        {
            PanelBrush.GradientStops[1].Color = ColorHelper.ColorFromHSB(Hue, ColorValues.MaxSaturation, ColorValues.MaxBrightness);
        }

        private void InitializePanelBrush()
        {
            PanelBrush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };
            PanelBrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            PanelBrush.GradientStops.Add(new GradientStop(Colors.Red, 1));
        }
        #endregion
    }
}