using Restless.Toolkit.Core;
using Restless.Toolkit.Mvvm;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a TabControl with extended capabilities.
    /// </summary>
    [TemplatePart(Name = PartTabPanel, Type = typeof(TabPanel))]
    [TemplatePart(Name = PartButtonTabList, Type = typeof(Button))]
    [TemplatePart(Name = PartTabList, Type = typeof(Popup))]
    [TemplatePart(Name = PartTabListBox, Type = typeof(System.Windows.Controls.ListBox))]
    public class TabControl : System.Windows.Controls.TabControl
    {
        #region Private
        private const string PartTabPanel = "PART_TabPanel";
        private const string PartButtonTabList = "PART_ButtonTabList";
        private const string PartTabList = "PART_TabList";
        private const string PartTabListBox = "PART_TabListBox";

        private TabPanel tabPanel;
        private Button buttonTabList;
        private Popup tabListPopup;
        private System.Windows.Controls.ListBox tabListBox;
        private readonly ICommand defaultCloseTabCommand;

        private Window dragCursor;
        private Point startPoint;
        private bool dragging = false;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl() : base()
        {
            IsSynchronizedWithCurrentItem = true;
            defaultCloseTabCommand = RelayCommand.Create(RunCloseTabCommand);
            CloseTabCommand = defaultCloseTabCommand;
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

        private static object OnCoerceBorderThickness(DependencyObject d, object baseValue)
        {
            if (baseValue is Thickness value)
            {
                double largest = Math.Max(Math.Max(Math.Max(value.Left, value.Right), value.Top), value.Bottom);
                return new Thickness(Math.Max(Math.Min(largest, 2.0), 1.0));
            }
            return new Thickness(1);
        }

        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TabControl)?.ActivateBorderChange();
        }
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the default tab height.
        /// </summary>
        public const double DefaultTabHeight = 32.0;

        /// <summary>
        /// Gets the default value for <see cref="TabHeightIncrease"/>.
        /// </summary>
        public const double DefaultTabHeightIncrease = 4.0;

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
                nameof(AllowTabReorder), typeof(bool), typeof(TabControl), new FrameworkPropertyMetadata(false)
            );

        /// <summary>
        /// Gets or sets a command to be executed after a drag / drop operation to perform the reordering.
        /// </summary>
        /// <remarks>
        /// If this property is not set, the tabs can be reordered internally ItemsSource is bound
        /// to an ObservableCollection or to an object type that derives directly from ObservableCollection.
        /// If this property is set, the command is executed to perform the reordering. The command receives
        /// a <see cref="TabItemDragDrop"/> object as its parameter.
        /// </remarks>
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
                nameof(ReorderTabsCommand), typeof(ICommand), typeof(TabControl), new FrameworkPropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets a value that determines if tabs are closeable.
        /// </summary>
        public bool AreTabsCloseable
        {
            get => (bool)GetValue(AreTabsCloseableProperty);
            set => SetValue(AreTabsCloseableProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="AreTabsCloseable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AreTabsCloseableProperty = DependencyProperty.Register
            (
                nameof(AreTabsCloseable), typeof(bool), typeof(TabControl), new FrameworkPropertyMetadata()
                {
                    DefaultValue = false
                }
            );

        /// <summary>
        /// Gets or sets a command to execute to close a tab.
        /// </summary>
        /// <remarks>
        /// By default, this property is set to a command that removes the tab internally.
        /// For internal tab removal, ItemsSource must be bound to an ObservableCollection,
        /// or a type that derives from ObservableCollection.
        /// If this is not the case, or you require/ additional processing before closing a tab,
        /// you must assign a command to this property that removes the tab.
        /// The parameter to the command is the <see cref="TabItem"/> that is being removed.
        /// </remarks>
        public ICommand CloseTabCommand
        {
            get => (ICommand)GetValue(CloseTabCommandProperty);
            set => SetValue(CloseTabCommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CloseTabCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register
            (
                nameof(CloseTabCommand), typeof(ICommand), typeof(TabControl), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                    CoerceValueCallback = OnCoerceCloseTabCommand
                }
            );

        private static object OnCoerceCloseTabCommand(DependencyObject d, object baseValue)
        {
            return d is TabControl control && baseValue == null ? control.defaultCloseTabCommand : baseValue;
        }

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
                nameof(DragCursorBrush), typeof(Brush), typeof(TabControl), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Brushes.DarkBlue,
                }
            );

        /// <summary>
        /// Gets a boolean value that indicates if reordering is available.
        /// </summary>
        private bool IsReorderAvailable => AllowTabReorder && Items.Count > 1;

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
                nameof(InactiveTabBackground), typeof(Brush), typeof(TabControl), new FrameworkPropertyMetadata()
                {
                    DefaultValue = Brushes.LightGray,
                }
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
                nameof(InactiveTabOpacity), typeof(double), typeof(TabControl), new FrameworkPropertyMetadata()
                {
                    DefaultValue = 0.475,
                }
            );

        /// <summary>
        /// Gets or sets the amount that a tab increases in height when it is selected.
        /// The value of this property is clamped between 0.0 - 8.0, inclusive.
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
            return Math.Max(Math.Min(value, 8.0), 0.0);
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
                nameof(KeepContentOnTabSwitch), typeof(bool), typeof(TabControl), new FrameworkPropertyMetadata()
                {
                    DefaultValue = false,
                }
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
                nameof(SelectedTabContent), typeof(object), typeof(TabControl), new FrameworkPropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="SelectedTabContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedTabContentProperty = SelectedTabContentPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets a boolean value that determines if the tab list is available.
        /// </summary>
        public bool IsTabListAvailable
        {
            get => (bool)GetValue(IsTabListAvailableProperty);
            private set => SetValue(IsTabListAvailablePropertyKey, value);
        }

        private static readonly DependencyPropertyKey IsTabListAvailablePropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(IsTabListAvailable), typeof(bool), typeof(TabControl), new PropertyMetadata(false)
            );

        /// <summary>
        /// Identifies the <see cref="IsTabListAvailable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTabListAvailableProperty = IsTabListAvailablePropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Called when the template is applied to get the tab panel.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (buttonTabList != null) buttonTabList.Click -= ButtonTabListClick;
            if (tabListBox != null) tabListBox.SelectionChanged -= TabListBoxSelectionChanged;

            tabPanel = GetTemplateChild(PartTabPanel) as TabPanel ?? throw new ArgumentException(PartTabPanel);;
            buttonTabList = GetTemplateChild(PartButtonTabList) as Button ?? throw new ArgumentException(PartButtonTabList);
            tabListPopup = GetTemplateChild(PartTabList) as Popup ?? throw new ArgumentException(PartTabList);
            tabListBox = GetTemplateChild(PartTabListBox) as System.Windows.Controls.ListBox ?? throw new ArgumentException(PartTabListBox);

            buttonTabList.Click += ButtonTabListClick;
            tabListBox.SelectionChanged += TabListBoxSelectionChanged;
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Gets a boolean value that indicates if the specified item is its own container.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>true if <paramref name="item"/> is <see cref="TabItem"/>; otherwise, false.</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TabItem;
        }

        /// <summary>
        /// Gets a container item.
        /// </summary>
        /// <returns>A new <see cref="TabItem"/>.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItem();
        }

        /// <summary>
        /// Prepares an item container
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="item">The item.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (element is TabItem tabItem)
            {
                tabItem.SyncToParent(this);

                if (!tabItem.HasHeader)
                {
                    if (item is HeaderedContentControl head)
                    {
                        tabItem.Header = head.Header;
                    }
                    else
                    {
                        tabItem.Header = item.GetType().Name;
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            UpdateSelectedItem();
        }

        /// <inheritdoc/>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            IsTabListAvailable = newValue != null;
        }

        /// <inheritdoc/>
        protected override void OnItemBindingGroupChanged(BindingGroup oldItemBindingGroup, BindingGroup newItemBindingGroup)
        {
            base.OnItemBindingGroupChanged(oldItemBindingGroup, newItemBindingGroup);
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }
        #endregion

        /************************************************************************/

        #region Drag / drop
        /// <summary>
        /// Called when the preview mouse left button down event is raised.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (IsReorderAvailable)
            {
                startPoint = e.GetPosition(null);
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
                    SetAllowDrop(excludedTab: tabSource);
                    dragCursor = CreateDragCursor(tabSource);
                    dragCursor.Show();
                    dragging = true;
                    DragDrop.DoDragDrop(tabSource, tabSource, DragDropEffects.Move);
                    dragging = false;
                    dragCursor.Close();
                    dragCursor = null;
                }
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
        /// Called when an item has been dropped on the control.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (IsReorderAvailable && GetRoutedEventTabItem(e) is TabItem tabTarget)
            {
                TabItem tabSource = e.Data.GetData(typeof(TabItem)) as TabItem;

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
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Sets all tabs to allow drop except for the excluded one, the one being dragged.
        /// </summary>
        /// <param name="excludedTab">The tab to exclude. This is the tab that's being dragged.</param>
        private void SetAllowDrop(TabItem excludedTab)
        {
            foreach (object item in Items)
            {
                if (GetTabItem(item) is TabItem tabItem)
                {
                    tabItem.AllowDrop = tabItem != excludedTab;
                }
            }
        }

        private TabItem GetTabItem(object item) => item is TabItem ? item as TabItem : ItemContainerGenerator.ContainerFromItem(item) as TabItem;

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
        private TabItem GetValidatedChildTabItem(TabItem tab) => CoreHelper.GetVisualParent<TabControl>(tab) == this ? tab : null;

        private void RunCloseTabCommand(object parm)
        {
            if (parm is TabItem tabItem)
            {
                RemoveByItemsSource(tabItem);
            }

        }
        private void RemoveByItemsSource(TabItem tabItem)
        {
            Type itemsSourceObservable = GetItemsSourceTypeAsObservable();

            if (itemsSourceObservable != null)
            {
                int sourceIdx = ItemContainerGenerator.IndexFromContainer(tabItem);
                if (sourceIdx >= 0)
                {
                    System.Reflection.MethodInfo method = itemsSourceObservable.GetMethod("RemoveAt");
                    method.Invoke(ItemsSource, new object[] { sourceIdx });
                }
            }
        }

        private void MoveByItemsSource(TabItem source, TabItem target)
        {
            Type itemsSourceObservable = GetItemsSourceTypeAsObservable();
            if (itemsSourceObservable != null)
            {
                int sourceIdx = ItemContainerGenerator.IndexFromContainer(source);
                int targetIdx = ItemContainerGenerator.IndexFromContainer(target);
                if (sourceIdx >= 0 && targetIdx >= 0)
                {
                    System.Reflection.MethodInfo method = itemsSourceObservable.GetMethod("Move");
                    method.Invoke(ItemsSource, new object[] { sourceIdx, targetIdx });
                }
            }
        }

        private void MoveByItems(TabItem source, TabItem target)
        {
            int sourceIdx = Items.IndexOf(source);
            int targetIdx = Items.IndexOf(target);
            if (sourceIdx >= 0 && targetIdx >= 0)
            {
                Items.Remove(source);
                Items.Insert(targetIdx, source);
            }
        }

        /// <summary>
        /// Gets ItemsSource type as ObservableCollection.
        /// </summary>
        /// <returns>
        /// ObservableCollection type if ItemsSource is not null
        /// and is (or derives from) ObservableCollection
        /// </returns>
        private Type GetItemsSourceTypeAsObservable()
        {
            if (ItemsSource != null)
            {
                Type sourceType = ItemsSource.GetType();
                if (!sourceType.IsGenericType)
                {
                    sourceType = sourceType.BaseType;
                }

                if (sourceType.IsGenericType)
                {
                    Type sourceDef = sourceType.GetGenericTypeDefinition();
                    if (sourceDef == typeof(ObservableCollection<>))
                    {
                        return sourceType;
                    }
                }
            }
            return null;
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
            if (SelectedItem == null)
            {
                return null;
            }

            if (SelectedItem is TabItem item)
            {
                return item;
            }

            return ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;
        }

        private void ActivateBorderChange()
        {
            if (tabPanel != null)
            {
                foreach (TabItem item in tabPanel.Children.OfType<TabItem>())
                {
                    item.SyncToParentBorder(this);
                }
                tabPanel.InvalidateMeasure();
            }
        }

        private void ButtonTabListClick(object sender, RoutedEventArgs e)
        {
            object temp = SelectedItem;
            tabListBox.SelectedItem = null;
            SelectedItem = temp;
            tabListPopup.IsOpen = true;
        }

        private void TabListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tabListPopup.IsOpen = false;
            SelectedItem = tabListBox.SelectedItem;
        }
        #endregion
    }
}