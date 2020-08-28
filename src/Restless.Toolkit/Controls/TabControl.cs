using Restless.Toolkit.Core;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a TabControl with extended capabilities.
    /// </summary>
    [TemplatePart(Name = PartItemsHolder, Type = typeof(Panel))]
    public class TabControl : System.Windows.Controls.TabControl
    {
        #region Private
        private Panel itemsHolderPanel;
        private const string PartItemsHolder = "PART_ItemsHolder";
        private Window dragCursor;
        private Point startPoint;
        private bool dragging = false;
        private const double DefaultTabHeight = 40.0;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl() : base()
        {
            IsSynchronizedWithCurrentItem = true;
            Loaded += TabControlLoaded;
        }

        static TabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));
            BorderThicknessProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata()
            { 
                DefaultValue = new Thickness(1),
                CoerceValueCallback = OnCoerceBorderThickness,
                PropertyChangedCallback = OnBorderThicknessChanged,
            });
        }
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the default value for <see cref="TabHeightIncrease"/>.
        /// </summary>
        public const double DefaultTabHeightIncrease = 5.0;

        /// <summary>
        /// Gets or sets a value that indicates if the tabs of this control can be reordered using drag and drop.
        /// </summary>
        public bool AllowTabReorder
        {
            get => (bool)GetValue(AllowTabReorderProperty);
            set => SetValue(AllowTabReorderProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the <see cref="AllowTabReorder"/> property.
        /// </summary>
        public static readonly DependencyProperty AllowTabReorderProperty = DependencyProperty.Register
            (
                nameof(AllowTabReorder), typeof(bool), typeof(TabControl), new UIPropertyMetadata(false)
            );

        /// <summary>
        /// Gets or sets a command to be executed after a drag / drop operation to perform the reordering.
        /// If this property is not set, the tabs can be reordered internally if the ItemsSource is bound
        /// to an ObservableCollection or to an object type that derives directly from ObservableCollection.
        /// </summary>
        public ICommand ReorderTabsCommand
        {
            get => (ICommand)GetValue(ReorderTabsCommandProperty);
            set => SetValue(ReorderTabsCommandProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that describes the <see cref="ReorderTabsCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty ReorderTabsCommandProperty = DependencyProperty.Register
            (
                nameof(ReorderTabsCommand), typeof(ICommand), typeof(TabControl), new UIPropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets a brush that will be used in the tab drag cursor
        /// </summary>
        public Brush DragCursorBrush
        { 
            get => (Brush)GetValue(DragCursorBrushProperty);
            set => SetValue(DragCursorBrushProperty, value);
        }

        /// <summary>
        /// Identifies the dependency property for <see cref="DragCursorBrush"/>.
        /// </summary>
        public static readonly DependencyProperty DragCursorBrushProperty = DependencyProperty.Register
            (
                nameof(DragCursorBrush), typeof(Brush), typeof(TabControl), new PropertyMetadata(new SolidColorBrush(Colors.DarkRed))
            );

        /// <summary>
        /// Gets a boolean value that indicates if reordering is available.
        /// </summary>
        private bool IsReorderAvailable
        {
            get => AllowTabReorder && Items.Count > 1;
        }

        /// <summary>
        /// Gets or sets the height of the tabs.
        /// This property is clamped between 16.0 and 92.0.
        /// </summary>
        public double TabHeight
        {
            get => (double)GetValue(TabHeightProperty);
            set => SetValue(TabHeightProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TabHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabHeightProperty = DependencyProperty.Register
            (
                nameof(TabHeight), typeof(double), typeof(TabControl), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultTabHeight,
                    CoerceValueCallback = OnCoerceTabHeight,
                }
            );

        private static object OnCoerceTabHeight(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Max(Math.Min(92.0, value), 16.0);
        }

        /// <summary>
        /// Gets or sets the minimum tab width.
        /// This property is clamped between 16.0 and 128.0.
        /// </summary>
        public double MinTabWidth
        {
            get => (double)GetValue(MinTabWidthProperty);
            set => SetValue(MinTabWidthProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="MinTabWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinTabWidthProperty = DependencyProperty.Register
            (
                nameof(MinTabWidth), typeof(double), typeof(TabControl), new FrameworkPropertyMetadata()
                {
                    DefaultValue = 16.0,
                    CoerceValueCallback = OnCoerceMinTabWidth,
                }
            );

        private static object OnCoerceMinTabWidth(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Max(Math.Min(128.0, value), 16.0);
        }

        /// <summary>
        /// Gets or sets the brush for tabs that are not currently selected.
        /// </summary>
        public Brush InactiveTabBackground
        {
            get => (Brush)GetValue(InactiveTabBackgroundProperty);
            set => SetValue(InactiveTabBackgroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="InactiveTabBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InactiveTabBackgroundProperty = DependencyProperty.Register
            (
                nameof(InactiveTabBackground), typeof(Brush), typeof(TabControl), new FrameworkPropertyMetadata(Brushes.LightGray)
            );

        /// <summary>
        /// Gets or sets the opacity for tabs that are not currently selected
        /// </summary>
        public double InactiveTabOpacity
        {
            get => (double)GetValue(InactiveTabOpacityProperty);
            set => SetValue(InactiveTabOpacityProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="InactiveTabOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InactiveTabOpacityProperty = DependencyProperty.Register
            (
                nameof(InactiveTabOpacity), typeof(double), typeof(TabControl), new PropertyMetadata(0.25)
            );

        /// <summary>
        /// Gets the tab border thickness
        /// </summary>
        public Thickness TabBorderThickness
        {
            get => (Thickness)GetValue(TabBorderThicknessProperty);
            private set => SetValue(TabBorderThicknessPropertyKey, value);
        }

        private static readonly DependencyPropertyKey TabBorderThicknessPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(TabBorderThickness), typeof(Thickness), typeof(TabControl), new PropertyMetadata(new Thickness(1, 1, 1, 0))
            );

        /// <summary>
        /// Identifies the <see cref="TabBorderThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabBorderThicknessProperty = TabBorderThicknessPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets the amount that a tab increases in height when it is selected.
        /// The value of this property is clamped between 2.0 - 8.0, inclusive.
        /// </summary>
        public double TabHeightIncrease
        {
            get => (double)GetValue(TabHeightIncreaseProperty);
            set => SetValue(TabHeightIncreaseProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TabHeightIncrease"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TabHeightIncreaseProperty = DependencyProperty.Register
            (
                nameof(TabHeightIncrease), typeof(double), typeof(TabControl), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DefaultTabHeightIncrease,
                    CoerceValueCallback = OnCoerceTabHeightIncrease,
                    PropertyChangedCallback = OnTabHeightIncreaseChanged,
                }
            );

        private static object OnCoerceTabHeightIncrease(DependencyObject d, object baseValue)
        {
            double value = (double)baseValue;
            return Math.Max(Math.Min(value, 8.0), 2.0);
        }

        private static void OnTabHeightIncreaseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if tab content is unloaded or maintained when the tab becomes inactive.
        /// The default value is false. Set to true to prevent inactive tab content from being unloaded.
        /// </summary>
        public bool KeepContentOnTabSwitch
        {
            get => (bool)GetValue(KeepContentOnTabSwitchProperty);
            set => SetValue(KeepContentOnTabSwitchProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="KeepContentOnTabSwitch"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KeepContentOnTabSwitchProperty = DependencyProperty.Register
            (
                nameof(KeepContentOnTabSwitch), typeof(bool), typeof(TabControl), new PropertyMetadata(false)
            );

        /// <summary>
        /// Gets the selected tab content
        /// </summary>
        public object SelectedTabContent
        {
            get => GetValue(SelectedTabContentProperty);
            private set => SetValue(SelectedTabContentPropertyKey, value);
        }

        private static readonly DependencyPropertyKey SelectedTabContentPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(SelectedTabContent), typeof(object), typeof(TabControl), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="SelectedTabContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedTabContentProperty = SelectedTabContentPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Get the ItemsHolder and generate any children
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            itemsHolderPanel = GetTemplateChild(PartItemsHolder) as Panel;
            //UpdateSelectedItem();
        }
        #endregion

        /************************************************************************/

        #region Protected methods

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (item is TabItem tabItem)
            {
                tabItem.MinWidth = MinTabWidth;
                tabItem.TabHeightIncrease = TabHeightIncrease;
            }
            return item is TabItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItem()
            {
                Height = TabHeight,
                MinWidth = MinTabWidth,
                TabHeightIncrease = TabHeightIncrease,
            };
        }

        /// <summary>
        /// Called when the selection on the control changes.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            UpdateSelectedItem();
        }
        #endregion

        /************************************************************************/

        #region Drag /drop
        /// <summary>
        /// Called when the preview mouse left button down event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (IsReorderAvailable && GetRoutedEventTabItem(e) is TabItem tab)
            {
                startPoint = e.GetPosition(null);
                SetAllowDrop(excludedTab: tab);
            }
        }

        /// <summary>
        /// Called when the preview mouse move event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (IsReorderAvailable && !dragging && e.LeftButton == MouseButtonState.Pressed && GetRoutedEventTabItem(e) is TabItem tabSource)
            {
                Point pos = e.GetPosition(null);

                if (Math.Abs(pos.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(pos.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    tabSource.AllowDrop = false;
                    dragCursor = CreateDragCursor(tabSource);
                    dragCursor.Show();
                    dragging = true;
                    DragDrop.DoDragDrop(tabSource, tabSource, DragDropEffects.Move);
                    tabSource.AllowDrop = true;
                    dragging = false;
                    dragCursor.Close();
                    dragCursor = null;
                }
            }
        }

        /// <summary>
        /// Called when the DragEnter event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (IsReorderAvailable && GetRoutedEventTabItem(e) is TabItem tab)
            {
                tab.Opacity = 0.475;
            }
        }

        /// <summary>
        /// Called when the give feedback event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            e.UseDefaultCursors = false;
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            dragCursor.Left = w32Mouse.X + 8;
            dragCursor.Top = w32Mouse.Y - (dragCursor.ActualHeight / 2);
            dragCursor.Opacity = (e.Effects == DragDropEffects.Move) ? 1.0 : 0.35;
            e.Handled = true;
        }

        /// <summary>
        /// Called when the DragLeave event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            if (IsReorderAvailable && GetRoutedEventTabItem(e) is TabItem tab)
            {
                tab.Opacity = 1.0;
            }
        }

        /// <summary>
        /// Called when an item has been dropped on the control.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (IsReorderAvailable && GetRoutedEventTabItem(e) is TabItem tabTarget)
            {
                var tabSource = e.Data.GetData(typeof(TabItem)) as TabItem;

                if (!tabTarget.Equals(tabSource))
                {
                    if (ReorderTabsCommand != null)
                    {
                        ReorderTabsCommand.Execute(new TabItemDragDrop(tabSource, tabTarget));
                    }
                    else if (ItemsSource != null)
                    {
                        MoveByItemsSource(tabSource, tabTarget);
                    } else
                    {
                        MoveByItems(tabSource, tabTarget);
                    }
                    tabSource.Opacity = 1.0;
                    tabTarget.Opacity = 1.0;
                    e.Handled = true;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods

        private void TabControlLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= TabControlLoaded;
            UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            if (KeepContentOnTabSwitch)
            {
                if (GetSelectedTabItem() is TabItem item)
                {
                    SelectedTabContent = item.GetContentPresenter();
                }
            }
            else
            {
                SelectedTabContent = SelectedContent;
            }
        }

        private TabItem GetSelectedTabItem()
        {
            if (SelectedItem == null) return null;
            if (SelectedItem is TabItem item) return item;
            return ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;
        }

        private TabItem GetTabItem(object item)
        {
            if (item is TabItem) return item as TabItem;
            return ItemContainerGenerator.ContainerFromItem(item) as TabItem;
        }

        private void MoveByItemsSource(TabItem source, TabItem target)
        {
            Type sourceType = ItemsSource.GetType();

            if (!sourceType.IsGenericType)
            {
                sourceType = sourceType.BaseType;
            }

            // sourceType being null is highly unlikely
            if (sourceType != null && sourceType.IsGenericType)
            {
                var sourceDef = sourceType.GetGenericTypeDefinition();

                if (sourceDef == typeof(ObservableCollection<>))
                {
                    int sourceIdx = ItemContainerGenerator.IndexFromContainer(source);
                    int targetIdx = ItemContainerGenerator.IndexFromContainer(target);
                    if (sourceIdx >= 0 && targetIdx >= 0)
                    {
                        var method = sourceType.GetMethod("Move");
                        method.Invoke(ItemsSource, new object[] { sourceIdx, targetIdx });
                    }
                }
            }
        }

        private void MoveByItems(TabItem source, TabItem target)
        {
            int sourceIdx = Items.IndexOf(source);
            int targetIdx = Items.IndexOf(target);
            if (sourceIdx >=0 && targetIdx >= 0)
            {
                Items.Remove(source);
                Items.Insert(targetIdx, source);
            }
        }

        /// <summary>
        /// Gets a tab item during a routed event
        /// </summary>
        /// <param name="e">The routed event args</param>
        /// <returns>The tab item, or null.</returns>
        /// <remarks>
        /// This method find the tabitem for the event. When tabs are defined directly in xaml,
        /// e.Source is the tab item. However, when items are bound via ItemsSource,
        /// e.Source is the tab control, and e.OriginalSource is some piece that's a child of the
        /// actual TabItem. This method examines all posibilities and returns the actual TabItem.
        /// </remarks>
        private TabItem GetRoutedEventTabItem(RoutedEventArgs e)
        {
            if (e.Source is TabItem tab1)
            {
                return GetValidatedChildTabItem(tab1);
            }
            if (e.OriginalSource is TabItem tab2)
            {
                return GetValidatedChildTabItem(tab2);
            }
            if (e.OriginalSource is DependencyObject dp)
            {
                return GetValidatedChildTabItem(CoreHelper.GetVisualParent<TabItem>(dp));
            }
            return null;
        }

        /// <summary>
        /// Gets the validated tab item
        /// </summary>
        /// <param name="tab">The tab item</param>
        /// <returns><paramref name="tab"/> if it belongs to this instance of the tab control</returns>
        private TabItem GetValidatedChildTabItem(TabItem tab)
        {
            if (CoreHelper.GetVisualParent<TabControl>(tab) == this)
            {
                return tab;
            }
            return null;
        }
        #endregion

        /************************************************************************/

        #region Private methods (drag and drop support)
        /// <summary>
        /// Sets all tabs to allow drop except for the excluded one, the one being dragged.
        /// </summary>
        /// <param name="excludedTab">The tab to exclude. This is the tab that's being dragged.</param>
        private void SetAllowDrop(TabItem excludedTab)
        {
            foreach (var item in Items)
            {
                TabItem tab = GetTabItem(item);
                if (tab != null && tab != excludedTab)
                {
                    tab.AllowDrop = true;
                }
            }
        }

        private Window CreateDragCursor(FrameworkElement dragElement)
        {
            dragCursor = new Window()
            {
                Background = DragCursorBrush,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Topmost = true,
                ShowInTaskbar = false,
                Width = dragElement.ActualWidth,
                Height = dragElement.ActualHeight + 6.0,
                Content = new Rectangle()
                {
                    Width = dragElement.ActualWidth,
                    Height = dragElement.ActualHeight,
                    Fill = new VisualBrush(dragElement)
                },
            };

            return dragCursor;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        };
        #endregion

        /************************************************************************/

        #region Private static methods
        private static object OnCoerceBorderThickness(DependencyObject d, object baseValue)
        {
            if (baseValue is Thickness value)
            {
                double largest = Math.Max(Math.Max(Math.Max(value.Left, value.Right), value.Top), value.Bottom);
                return new Thickness(Math.Min(largest, 2.0));
            }
            return new Thickness(1);
        }

        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabControl control)
            {
                double value = control.BorderThickness.Left;
                control.TabBorderThickness = new Thickness(value, value, value, 0.0);
            }
        }
        #endregion
    }
}