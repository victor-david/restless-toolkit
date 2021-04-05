using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Toolkit.Core
{
    /// <summary>
    /// Provides attached properties
    /// </summary>
    public static class Property
    {
        #region RowHeights
        private const string RowHeightsPropertyName = "RowHeights";

        /// <summary>
        /// Gets the RowHeights attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object from which to retreive the property.</param>
        /// <returns>The property value.</returns>
        public static string GetRowHeights(DependencyObject obj)
        {
            return (string)obj.GetValue(RowHeightsProperty);
        }

        /// <summary>
        /// Sets the RowHeights attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetRowHeights(DependencyObject obj, string value)
        {
            obj.SetValue(RowHeightsProperty, value);
        }

        /// <summary>
        /// Identifies the RowHeights attached dependency property.
        /// </summary>
        public static readonly DependencyProperty RowHeightsProperty = DependencyProperty.RegisterAttached
            (
                RowHeightsPropertyName, typeof(string), typeof(Property), new PropertyMetadata()
                {
                    DefaultValue = string.Empty,
                    PropertyChangedCallback = OnRowHeightsChanged
                }
            );

        private static void OnRowHeightsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Grid grid)) return;

            grid.RowDefinitions.Clear();

            string definitions = e.NewValue.ToString();
            if (string.IsNullOrEmpty(definitions)) return;
            var heights = definitions.Split(',');

            foreach (string value in heights)
            {
                if (value.ToLower() == "auto")
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }
                else if (value.EndsWith("*"))
                {
                    string value2 = value.Replace("*", "");
                    if (string.IsNullOrEmpty(value2)) value2 = "1";
                    var numHeight = double.Parse(value2);
                    grid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(numHeight, GridUnitType.Star)
                    });
                }
                else
                {
                    var numHeight = int.Parse(value);
                    grid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = new GridLength(numHeight, GridUnitType.Pixel)
                    });
                }
            }
        }
        #endregion

        /************************************************************************/

        #region ColumnWidths
        private const string ColumnWidthsPropertyName = "ColumnWidths";

        /// <summary>
        /// Gets the ColumnWidths attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object from which to retreive the property.</param>
        /// <returns>The property value.</returns>
        public static string GetColumnWidths(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnWidthsProperty);
        }

        /// <summary>
        /// Sets the ColumnWidths attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetColumnWidths(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnWidthsProperty, value);
        }

        /// <summary>
        /// Identifies the ColumnWidths attached dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnWidthsProperty = DependencyProperty.RegisterAttached
            (
                ColumnWidthsPropertyName, typeof(string), typeof(Property), new PropertyMetadata()
                {
                    DefaultValue = string.Empty,
                    PropertyChangedCallback = OnColumnWidthsChanged
                }
            );

        private static void OnColumnWidthsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Grid grid)) return;

            grid.ColumnDefinitions.Clear();

            var definitions = e.NewValue.ToString();
            if (string.IsNullOrEmpty(definitions)) return;
            var widths = definitions.Split(',');

            foreach (string value in widths)
            {
                if (value.ToLower() == "auto")
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                }
                else if (value.EndsWith("*"))
                {
                    string value2 = value.Replace("*", "");
                    if (string.IsNullOrEmpty(value2)) value2 = "1";
                    var numWidth = double.Parse(value2);
                    grid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(numWidth, GridUnitType.Star)
                    });
                }
                else
                {
                    var numWidth = int.Parse(value);
                    grid.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(numWidth, GridUnitType.Pixel)
                    });
                }
            }
        }
        #endregion

        /************************************************************************/

        #region IsVisible
        private const string IsVisiblePropertyName = "IsVisible";

        /// <summary>
        /// Gets the IsVisible attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object from which to retreive the property.</param>
        /// <returns>The property value.</returns>
        public static bool GetIsVisible(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsVisibleProperty);
        }

        /// <summary>
        /// Sets the IsVisible attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsVisible(DependencyObject obj, bool value)
        {
            obj.SetValue(IsVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the IsVisible attached dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached
            (
                IsVisiblePropertyName, typeof(bool), typeof(Property), new PropertyMetadata(true, OnIsVisibleChanged)
            );

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                element.Visibility = ((bool)e.NewValue) == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        /************************************************************************/

        #region IsCollapsed
        private const string IsCollapsedPropertyName = "IsCollapsed";

        /// <summary>
        /// Gets the IsCollapsed attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object from which to retreive the property.</param>
        /// <returns>The property value.</returns>
        public static bool GetIsCollapsed(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCollapsedProperty);
        }

        /// <summary>
        /// Sets the IsCollapsed attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsCollapsed(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCollapsedProperty, value);
        }

        /// <summary>
        /// Identifies the IsCollapsed attached dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.RegisterAttached
            (
                IsCollapsedPropertyName, typeof(bool), typeof(Property), new PropertyMetadata(false, OnIsCollapsedChanged)
            );

        private static void OnIsCollapsedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                element.Visibility = ((bool)e.NewValue) == true ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        #endregion

        /************************************************************************/

        #region IsLongVisible
        private const string IsLongVisiblePropertyName = "IsLongVisible";

        /// <summary>
        /// Gets the IsLongVisible attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object from which to retreive the property.</param>
        /// <returns>The property value.</returns>
        /// <remarks>
        /// See <see cref="IsLongVisibleProperty"/> for more information.
        /// </remarks>
        public static long GetIsLongVisible(DependencyObject obj)
        {
            return (long)obj.GetValue(IsLongVisibleProperty);
        }

        /// <summary>
        /// Sets the IsLongVisible attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        /// <remarks>
        /// See <see cref="IsLongVisibleProperty"/> for more information.
        /// </remarks>
        public static void SetIsLongVisible(DependencyObject obj, long value)
        {
            obj.SetValue(IsLongVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the IsLongVisible attached dependency property.
        /// </summary>
        /// <remarks>
        /// This dependency property and its companion <see cref="IsLongVisibleValueProperty"/> may be attached
        /// to an element to control its visibility depending on whether the value bound here is equal to the
        /// value found on the <see cref="IsLongVisibleValueProperty"/>. Typically, this property is bound
        /// to a changing value and <see cref="IsLongVisibleValueProperty"/> contains a fixed value on
        /// which to compare.
        /// </remarks>
        public static readonly DependencyProperty IsLongVisibleProperty = DependencyProperty.RegisterAttached
            (
                IsLongVisiblePropertyName, typeof(long), typeof(Property), new PropertyMetadata()
                {
                    DefaultValue = 0L,
                    PropertyChangedCallback = OnIsLongVisibleChanged
                }
            );

        private static void OnIsLongVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                long val = GetIsLongVisibleValue(element);
                element.Visibility = ((long)e.NewValue) == val ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private const string IsLongVisibleValuePropertyName = "IsLongVisibleValue";

        /// <summary>
        /// Gets the IsLongVisibleValue attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object from which to retreive the property.</param>
        /// <returns>The property value.</returns>
        /// <remarks>
        /// See <see cref="IsLongVisibleValueProperty"/> for more information.
        /// </remarks>
        public static long GetIsLongVisibleValue(DependencyObject obj)
        {
            return (long)obj.GetValue(IsLongVisibleValueProperty);
        }

        /// <summary>
        /// Sets the IsLongVisibleValue attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        /// <remarks>
        /// See <see cref="IsLongVisibleValueProperty"/> for more information.
        /// </remarks>
        public static void SetIsLongVisibleValue(DependencyObject obj, long value)
        {
            obj.SetValue(IsLongVisibleValueProperty, value);
        }

        /// <summary>
        /// Identifies the IsLongVisibleValue attached dependency property.
        /// </summary>
        /// <remarks>
        /// This dependency property and its companion <see cref="IsLongVisibleProperty"/> may be attached
        /// to an element to control its visibility. When <see cref="IsLongVisibleProperty"/> equals the value
        /// of this property, the element is visible; otherwise it is collapsed. Typically, <see cref="IsLongVisibleProperty"/>
        /// is bound to a changing value and this property contains a fixed value on which to compare.
        /// </remarks>
        public static readonly DependencyProperty IsLongVisibleValueProperty = DependencyProperty.RegisterAttached
            (
                IsLongVisibleValuePropertyName, typeof(long), typeof(Property), new PropertyMetadata(0L)
            );
        #endregion

        /************************************************************************/

        #region IsInverseEnabled
        private const string IsInverseEnabledPropertyName = "IsInverseEnabled";

        /// <summary>
        /// Gets the IsInverseEnabled attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object from which to retreive the property.</param>
        /// <returns>The property value.</returns>
        public static bool GetIsInverseEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsInverseEnabledProperty);
        }

        /// <summary>
        /// Sets the IsInverseEnabled attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetIsInverseEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsInverseEnabledProperty, value);
        }

        /// <summary>
        /// Identifies the IsInverseEnabled attached dependency property.
        /// </summary>
        public static readonly DependencyProperty IsInverseEnabledProperty = DependencyProperty.RegisterAttached
            (
                IsInverseEnabledPropertyName, typeof(bool), typeof(Property), new PropertyMetadata(false, OnIsInverseEnabledChanged)
            );

        private static void OnIsInverseEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                element.IsEnabled = (bool)e.NewValue == false;
            }
        }
        #endregion

        /************************************************************************/

        #region RolloverBrush
        private const string RolloverBrushPropertyName = "RolloverBrush";

        /// <summary>
        /// Gets the RolloverBrush attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object from which to retreive the property.</param>
        /// <returns>The property value.</returns>
        public static Brush GetRolloverBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(RolloverBrushProperty);
        }

        /// <summary>
        /// Sets the RolloverBrush attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetRolloverBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(RolloverBrushProperty, value);
        }

        /// <summary>
        /// Identifies the RolloverBrush attached dependency property.
        /// </summary>
        public static readonly DependencyProperty RolloverBrushProperty = DependencyProperty.RegisterAttached
            (
                RolloverBrushPropertyName, typeof(Brush), typeof(Property), new PropertyMetadata(Brushes.Transparent)
            );
        #endregion

        /************************************************************************/

        #region DefaultDock
        private const string DefaultDockPropertyName = "DefaultDock";

        /// <summary>
        /// Gets the DefaultDock attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object from which to retreive the property.</param>
        /// <returns>The property value.</returns>
        public static Dock GetDefaultDock(DependencyObject obj)
        {
            return (Dock)obj.GetValue(DefaultDockProperty);
        }

        /// <summary>
        /// Sets the DefaultDock attached dependency property.
        /// </summary>
        /// <param name="obj">The dependency object on which to set the property.</param>
        /// <param name="value">The value to set.</param>
        public static void SetDefaultDock(DependencyObject obj, Dock value)
        {
            obj.SetValue(DefaultDockProperty, value);
        }

        /// <summary>
        /// Identifies the DefaultDock attached dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultDockProperty = DependencyProperty.RegisterAttached
            (
                DefaultDockPropertyName, typeof(Dock), typeof(Property), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Dock.Left,
                    Inherits = true,
                    AffectsRender = true,
                    PropertyChangedCallback = OnDefaultDockChanged
                }
            );

        private static void OnDefaultDockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Dock dock)
            {
                d.SetValue(DockPanel.DockProperty, dock);
            }
        }
        #endregion
    }
}