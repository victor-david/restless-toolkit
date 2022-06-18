using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Restless.Toolkit.Core
{
    /// <summary>
    /// Provides attached properties
    /// </summary>
    public static class Behavior
    {
        #region LoadedCommand
        /// <summary>
        /// Gets the loaded command for the specified dependency object
        /// </summary>
        /// <param name="obj">The dependency object</param>
        /// <returns>The command or null</returns>
        public static ICommand GetLoadedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(LoadedCommandProperty);
        }

        /// <summary>
        /// Sets the loaded command for the specified dependency object
        /// </summary>
        /// <param name="obj">The dependency object</param>
        /// <param name="value">The command to set</param>
        public static void SetLoadedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(LoadedCommandProperty, value);
        }

        private const string LoadedCommand = nameof(LoadedCommand);

        /// <summary>
        /// Identifies the LoadedCommand attached dependency property
        /// </summary>
        public static readonly DependencyProperty LoadedCommandProperty = DependencyProperty.RegisterAttached
            (
                LoadedCommand, typeof(ICommand), typeof(Behavior), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                    PropertyChangedCallback = OnLoadedCommandPropertyChanged
                }
            );

        private static void OnLoadedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && e.NewValue is ICommand command)
            {
                if (!Loaded.Contains(element))
                {
                    Loaded.Add(element);
                    element.Loaded += (s, a) => command.Execute(null);
                }
            }
        }
        #endregion

        /************************************************************************/

        #region UnloadedCommand
        /// <summary>
        /// Gets the unloaded command for the specified dependency object
        /// </summary>
        /// <param name="obj">The dependency object</param>
        /// <returns>The command or null</returns>
        public static ICommand GetUnloadedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(UnloadedCommandProperty);
        }

        /// <summary>
        /// Sets the unloaded command for the specified dependency object
        /// </summary>
        /// <param name="obj">The dependency object</param>
        /// <param name="value">The command to set</param>
        public static void SetUnloadedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(UnloadedCommandProperty, value);
        }

        private const string UnloadedCommand = nameof(UnloadedCommand);

        /// <summary>
        /// Identifies the LoadedCommand attached dependency property
        /// </summary>
        public static readonly DependencyProperty UnloadedCommandProperty = DependencyProperty.RegisterAttached
            (
                UnloadedCommand, typeof(ICommand), typeof(Behavior), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                    PropertyChangedCallback = OnUnloadedCommandPropertyChanged
                }
            );

        private static void OnUnloadedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && e.NewValue is ICommand command)
            {
                if (!Unloaded.Contains(element))
                {
                    Unloaded.Add(element);
                    element.Unloaded += (s, a) =>
                    {
                        command.Execute(null);
                        Loaded.Remove(element);
                        Unloaded.Remove(element);
                    };
                }
            }
        }

        private static readonly List<FrameworkElement> Loaded = new List<FrameworkElement>();
        private static readonly List<FrameworkElement> Unloaded = new List<FrameworkElement>();
        #endregion
    }
}