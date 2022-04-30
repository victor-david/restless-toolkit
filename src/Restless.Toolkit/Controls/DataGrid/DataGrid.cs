using Restless.Toolkit.Core;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SysDataGrid = System.Windows.Controls.DataGrid;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Extends <see cref="System.Windows.Controls.DataGrid"/> to provide custom functionality.
    /// </summary>
    public class DataGrid : SysDataGrid
    {
        #region Private
        private readonly DataGridColumnSelector columnSelector;
        private ScrollViewer outerScrollViewer;
        #endregion

        /************************************************************************/

        #region Resource keys
        /// <summary>
        /// Identifies the resource key for data grid
        /// </summary>
        public static readonly ComponentResourceKey DataGridStyleKey = new ComponentResourceKey(typeof(DataGrid), nameof(DataGridStyleKey));

        /// <summary>
        /// Identifies the resource key for column header style
        /// </summary>
        public static readonly ComponentResourceKey ColumnHeaderStyleKey = new ComponentResourceKey(typeof(DataGrid), nameof(ColumnHeaderStyleKey));

        /// <summary>
        /// Identifies the resource key for data grid cell style
        /// </summary>
        public static readonly ComponentResourceKey DataGridCellStyleKey = new ComponentResourceKey(typeof(DataGrid), nameof(DataGridCellStyleKey));

        /// <summary>
        /// Identifies the resource key for the sort indicator
        /// </summary>
        public static readonly ComponentResourceKey SortIndicatorStyleKey = new ComponentResourceKey(typeof(DataGrid), nameof(SortIndicatorStyleKey));
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGrid"/> class.
        /// </summary>
        public DataGrid()
        {
            DataContextChanged += OnDataContextChanged;
            AddHandler(LoadedEvent, new RoutedEventHandler(OnLoaded));
            columnSelector = new DataGridColumnSelector();
        }
        #endregion

        /************************************************************************/

        #region CustomSort (attached property)
        private const string CustomSort = nameof(CustomSort);
        /// <summary>
        /// Defines an attached dependency property that is used to provide custom sorting
        /// on a DataGridColumn by adding a secondary sort on another column.
        /// </summary>
        public static readonly DependencyProperty CustomSortProperty = DependencyProperty.RegisterAttached
            (
                CustomSort, typeof(DataGridColumnSortSpec), typeof(DataGrid), new PropertyMetadata()
            );

        /// <summary>
        /// Gets the <see cref="CustomSortProperty"/> for the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to get the property for.</param>
        /// <returns>The attached property, or null if none.</returns>
        public static DataGridColumnSortSpec GetCustomSort(DependencyObject obj)
        {
            return (DataGridColumnSortSpec)obj.GetValue(CustomSortProperty);
        }

        /// <summary>
        /// Sets the <see cref="CustomSortProperty"/> on the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to set the property on.</param>
        /// <param name="value">The property to set.</param>
        public static void SetCustomSort(DependencyObject obj, DataGridColumnSortSpec value)
        {
            obj.SetValue(CustomSortProperty, value);
        }
        #endregion

        /************************************************************************/

        #region DoubleClick (attached property)
        private const string DoubleClickCommand = nameof(DoubleClickCommand);
        /// <summary>
        /// Defines an attached dependency property that enables binding the mouse double-click 
        /// on a data grid row to a command.
        /// </summary>
        /// <remarks>
        /// See:
        /// http://stackoverflow.com/questions/17419570/bind-doubleclick-command-from-datagrid-row-to-vm
        /// </remarks>
        public static DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached
           (
                DoubleClickCommand, typeof(ICommand), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                    PropertyChangedCallback = OnDoubleClickPropertyChanged
                }
           );

        private static void OnDoubleClickPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGridRow element)
            {
                if (e.NewValue != null)
                {
                    element.AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(DataGridMouseDoubleClick));
                }
                else
                {
                    element.RemoveHandler(MouseDoubleClickEvent, new RoutedEventHandler(DataGridMouseDoubleClick));
                }
            }
        }

        private static void DataGridMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (sender is DataGridRow element && GetDoubleClickCommand(element) is ICommand command)
            {
                if (command.CanExecute(element.Item))
                {
                    command.Execute(element.Item);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="DoubleClickCommandProperty"/> for the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to get the property for.</param>
        /// <returns>The attached property, or null if none.</returns>
        public static ICommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DoubleClickCommandProperty);
        }

        /// <summary>
        /// Sets the <see cref="DoubleClickCommandProperty"/> on the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to set the property on.</param>
        /// <param name="value">The property to set.</param>
        public static void SetDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleClickCommandProperty, value);
        }
        #endregion

        /************************************************************************/

        #region Header
        /// <summary>
        /// Gets or sets the mode for the data grid headers
        /// </summary>
        public DataGridHeaderMode HeaderMode
        {
            get => (DataGridHeaderMode)GetValue(HeaderModeProperty);
            set => SetValue(HeaderModeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HeaderMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderModeProperty = DependencyProperty.Register
            (
                nameof(HeaderMode), typeof(DataGridHeaderMode), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DataGridHeaderMode.None
                }
            );

        /// <summary>
        /// Gets or sets a command to execute when the mouse is right clicked on a header.
        /// </summary>
        /// <remarks>
        /// <see cref="HeaderMode"/> must be <see cref="DataGridHeaderMode.Command"/> for this
        /// command to execute. The parameter for this command is the <see cref="DataGridColumnHeader"/>
        /// that was clicked.
        /// </remarks>
        public ICommand HeaderCommand
        {
            get => (ICommand)GetValue(HeaderCommandProperty);
            set => SetValue(HeaderCommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="HeaderCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderCommandProperty = DependencyProperty.Register
            (
                nameof(HeaderCommand), typeof(ICommand), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                }
            );

        /// <summary>
        /// Gets or sets the background for the column selector
        /// </summary>
        public Brush ColumnSelectorBackground
        {
            get => (Brush)GetValue(ColumnSelectorBackgroundProperty);
            set => SetValue(ColumnSelectorBackgroundProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSelectorBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSelectorBackgroundProperty = DependencyProperty.Register
            (
                nameof(ColumnSelectorBackground), typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DataGridColumnSelector.DefaultBackground,
                    PropertyChangedCallback = OnColumnSelectorBackgroundChanged
                }
            );

        private static void OnColumnSelectorBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataGrid).columnSelector.Background = (Brush)e.NewValue;
        }

        /// <summary>
        /// Gets or sets the border brush for the column selector
        /// </summary>
        public Brush ColumnSelectorBorderBrush
        {
            get => (Brush)GetValue(ColumnSelectorBorderBrushProperty);
            set => SetValue(ColumnSelectorBorderBrushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSelectorBorderBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSelectorBorderBrushProperty = DependencyProperty.Register
            (
                nameof(ColumnSelectorBorderBrush), typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DataGridColumnSelector.DefaultBorderBrush,
                    PropertyChangedCallback = OnColumnSelectorBorderBrushChanged
                }
            );

        private static void OnColumnSelectorBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataGrid).columnSelector.BorderBrush = (Brush)e.NewValue;
        }

        /// <summary>
        /// Gets or sets the column selector border thickness
        /// </summary>
        public Thickness ColumnSelectorBorderThickness
        {
            get => (Thickness)GetValue(ColumnSelectorBorderThicknessProperty);
            set => SetValue(ColumnSelectorBorderThicknessProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSelectorBorderThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSelectorBorderThicknessProperty = DependencyProperty.Register
            (
                nameof(ColumnSelectorBorderThickness), typeof(Thickness), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DataGridColumnSelector.DefaultBorderThickness,
                    PropertyChangedCallback = OnColumnSelectorBorderThicknessChanged
                }
            );

        private static void OnColumnSelectorBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataGrid).columnSelector.BorderThickness = (Thickness)e.NewValue;
        }

        /// <summary>
        /// Gets or sets the column selector uniform padding
        /// </summary>
        public double ColumnSelectorUniformPadding
        {
            get => (double)GetValue(ColumnSelectorUniformPaddingProperty);
            set => SetValue(ColumnSelectorUniformPaddingProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSelectorUniformPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSelectorUniformPaddingProperty = DependencyProperty.Register
            (
                nameof(ColumnSelectorUniformPadding), typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DataGridColumnSelector.DefaultUniformPadding,
                    CoerceValueCallback = OnCoerceColumnSelectorUniformPadding,
                    PropertyChangedCallback = OnColumnSelectorUniformPaddingChanged
                }
            );

        private static object OnCoerceColumnSelectorUniformPadding(DependencyObject d, object baseValue)
        {
            return Math.Min(Math.Max((double)baseValue, DataGridColumnSelector.MinimumUniformPadding), DataGridColumnSelector.MaximumUniformPadding);
        }

        private static void OnColumnSelectorUniformPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataGrid).columnSelector.UniformPadding = (double)e.NewValue;
        }

        /// <summary>
        /// Gets or sets the margin used for column selector check boxes
        /// </summary>
        public Thickness ColumnSelectorCheckBoxMargin
        {
            get => (Thickness)GetValue(ColumnSelectorCheckBoxMarginProperty);
            set => SetValue(ColumnSelectorCheckBoxMarginProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSelectorCheckBoxMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSelectorCheckBoxMarginProperty = DependencyProperty.Register
            (
                nameof(ColumnSelectorCheckBoxMargin), typeof(Thickness), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DataGridColumnSelector.DefaultCheckBoxMargin,
                    PropertyChangedCallback = OnColumnSelectorCheckBoxMarginChanged
                }
            );

        private static void OnColumnSelectorCheckBoxMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataGrid).columnSelector.CheckBoxMargin = (Thickness)e.NewValue;
        }

        /// <summary>
        /// Gets or sets the minimum number of columns that can remain visible when selecting
        /// columns with the column selector
        /// </summary>
        public int ColumnSelectorMinimumVisible
        {
            get => (int)GetValue(ColumnSelectorMinimumVisibleProperty);
            set => SetValue(ColumnSelectorMinimumVisibleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSelectorMinimumVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSelectorMinimumVisibleProperty = DependencyProperty.Register
            (
                nameof(ColumnSelectorMinimumVisible), typeof(int), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DataGridColumnSelector.DefaultMinimumVisible,
                    CoerceValueCallback = OnCoerceColumnSelectorMinimum,
                    PropertyChangedCallback = OnColumnSelectorMinimumChanged
                }
            );
        
        private static object OnCoerceColumnSelectorMinimum(DependencyObject d, object baseValue)
        {
            return Math.Max((int)baseValue, 1);
        }

        private static void OnColumnSelectorMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataGrid).columnSelector.MinimumVisible = (int)e.NewValue;
        }

        /// <summary>
        /// Gets or sets the text used for the reset link
        /// </summary>
        public string ColumnSelectorResetText
        {
            get => (string)GetValue(ColumnSelectorResetTextProperty);
            set => SetValue(ColumnSelectorResetTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSelectorResetText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSelectorResetTextProperty = DependencyProperty.Register
            (
                nameof(ColumnSelectorResetText), typeof(string), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = DataGridColumnSelector.DefaultResetText,
                    CoerceValueCallback = OnCoerceColumnSelectorResetText,
                    PropertyChangedCallback = OnColumnSelectorResetTextChanged
                }
            );

        private static object OnCoerceColumnSelectorResetText(DependencyObject d, object baseValue)
        {
            return string.IsNullOrWhiteSpace(baseValue as string) ? DataGridColumnSelector.DefaultResetText : baseValue;
        }

        private static void OnColumnSelectorResetTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DataGrid).columnSelector.ResetText = (string)e.NewValue;
        }

        #endregion

        /************************************************************************/

        #region ContextMenuOpeningCommand
        /// <summary>
        /// Gets or sets a command to be executed when a context menu associated with the data grid is opening.
        /// </summary>
        public ICommand ContextMenuOpeningCommand
        {
            get => (ICommand)GetValue(ContextMenuOpeningCommandProperty);
            set => SetValue(ContextMenuOpeningCommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ContextMenuOpeningCommand"/> dependency property.
        /// </summary>
        public static DependencyProperty ContextMenuOpeningCommandProperty = DependencyProperty.Register
             (
                nameof(ContextMenuOpeningCommand), typeof(ICommand), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null
                }
             );
        #endregion

        /************************************************************************/

        #region SortingCommand
        /// <summary>
        /// Gets or sets a command to be executed when the data grid is sorting as defined by <see cref="SortingCommandProperty"/>.
        /// </summary>
        public ICommand SortingCommand
        {
            get => (ICommand)GetValue(SortingCommandProperty);
            set => SetValue(CustomSortProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that allows the consumer to run a command when the DataGrid is sorting.
        /// </summary>
        public static DependencyProperty SortingCommandProperty = DependencyProperty.Register
             (
                nameof(SortingCommand), typeof(ICommand), typeof(DataGrid), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null
                }
             );
        #endregion

        /************************************************************************/

        #region SelectedItemsList
        /// <summary>
        /// Gets or sets the selected items list as defined by <see cref="SelectedItemsListProperty"/>.
        /// </summary>
        public IList SelectedItemsList
        {
            get => (IList)GetValue(SelectedItemsListProperty);
            set => SetValue(SelectedItemsListProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that enables the consumer to bind to multiple selected items
        /// </summary>
        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register
            (
                nameof(SelectedItemsList), typeof(IList), typeof(DataGrid), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region AutoFocusOnMouseEnter
        /// <summary>
        /// Gets or sets a boolean value that indicates whether the control will recieve focus when the mouse enters as defined by the <see cref="AutoFocusOnMouseEnterProperty"/>.
        /// </summary>
        public bool AutoFocusOnMouseEnter
        {
            get => (bool)GetValue(AutoFocusOnMouseEnterProperty);
            set => SetValue(AutoFocusOnMouseEnterProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that specifies whether the control will recieve focus when the mouse enters.
        /// </summary>
        public static readonly DependencyProperty AutoFocusOnMouseEnterProperty = DependencyProperty.Register
            (
                nameof(AutoFocusOnMouseEnter), typeof(bool), typeof(DataGrid), new PropertyMetadata(false)
            );
        #endregion

        /************************************************************************/

        #region RestoreStateBehavior
        /// <summary>
        /// Gets or sets a value that determines which actions will be taken to restore the control state as defined by the <see cref="RestoreStateBehaviorProperty"/>.
        /// </summary>
        public RestoreDataGridState RestoreStateBehavior
        {
            get => (RestoreDataGridState)GetValue(RestoreStateBehaviorProperty);
            set => SetValue(RestoreStateBehaviorProperty, value);
        }

        /// <summary>
        /// Defines a dependency property that specifies which actions will be taken to restore the control state.
        /// </summary>
        public static readonly DependencyProperty RestoreStateBehaviorProperty = DependencyProperty.Register
            (
                nameof(RestoreStateBehavior), typeof(RestoreDataGridState), typeof(DataGrid), new PropertyMetadata(RestoreDataGridState.None)
            );
        #endregion

        /************************************************************************/

        #region ScrollViewerVerticalOffset
#if VERTOFFSET
        /// <summary>
        /// Gets or sets a value that determines the vertical offset of the data grid scroll viewer
        /// </summary>
        public double ScrollViewerVerticalOffset
        {
            get => (double)GetValue(ScrollViewerVerticalOffsetProperty);
            set => SetValue(ScrollViewerVerticalOffsetProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ScrollViewerVerticalOffset"/> dependency property
        /// </summary>
        public static readonly DependencyProperty ScrollViewerVerticalOffsetProperty = DependencyProperty.Register
            (
                nameof(ScrollViewerVerticalOffset), typeof(double), typeof(DataGrid),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnScrollViewerVerticalOffsetChanged)
            );

        private static void OnScrollViewerVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid control)
            {
                var info = new ScrollInfo(control, (double)e.NewValue);
                if (control.scrollViewer == null)
                {
                    control.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(PerformScroll), info);
                }
                else
                {
                    PerformScroll(info);
                }
            }
        }

        private static object PerformScroll(object parm)
        {
            if (parm is ScrollInfo info && info.Grid.scrollViewer != null)
            {
                double currentOffset = info.Grid.scrollViewer.VerticalOffset;
                double newOffset = info.VerticalOffset;
                if (newOffset != currentOffset)
                {
                    info.Grid.scrollViewer.ScrollToVerticalOffset(newOffset);
                }
            }
            return null;
        }
#endif
        #endregion

        /************************************************************************/

        #region UseOuterScrollViewer
        /// <summary>
        /// Gets or sets a boolean value that determines if the mouse wheel movement
        /// is handled by an outer scroll viewer. The default is false.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see cref="DataGrid"/> is contained within an outer scroll viewer
        /// with other content to scroll as a unit, the mouse wheel has no effect when the mouse
        /// is within the list box. Set this property to true to allow mouse wheel movement
        /// within the list box to manage the outer scroll viewer.
        /// </para>
        /// <para>
        /// If the data grid has its Height property set to a value that does not allow all of its
        /// items to display, or is otherwise constrained, it will activate its built in scroll viewer.
        /// In these cases, you should not set this property to true.
        /// </para>
        /// </remarks>
        public bool UseOuterScrollViewer
        {
            get => (bool)GetValue(UseOuterScrollViewerProperty);
            set => SetValue(UseOuterScrollViewerProperty, value);
        }

        /// <summary>
        /// Dependency property for <see cref="UseOuterScrollViewer"/>
        /// </summary>
        public static readonly DependencyProperty UseOuterScrollViewerProperty = DependencyProperty.Register
            (
                nameof(UseOuterScrollViewer), typeof(bool), typeof(DataGrid), new PropertyMetadata(false)
            );
        #endregion

        /************************************************************************/

        #region OnBeginningEditCommand
        /// <summary>
        /// Gets or sets a command to be executed when the grid begins to edit.
        /// </summary>
        public ICommand OnBeginningEditCommand
        {
            get => (ICommand)GetValue(OnBeginningEditCommandProperty);
            set => SetValue(OnBeginningEditCommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OnBeginningEditCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OnBeginningEditCommandProperty = DependencyProperty.Register
            (
                nameof(OnBeginningEditCommand), typeof(ICommand), typeof(DataGrid), new PropertyMetadata(null)
            );

        #endregion
        
        /************************************************************************/

        #region OnCellEditEndingCommand
        /// <summary>
        /// Gets or sets a command to be executed when a grid cell ends it edit as defined by the  <see cref="OnCellEditEndingCommandProperty"/>.
        /// </summary>
        public ICommand OnCellEditEndingCommand
        {
            get => (ICommand)GetValue(OnCellEditEndingCommandProperty);
            set => SetValue(OnCellEditEndingCommandProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="OnCellEditEndingCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OnCellEditEndingCommandProperty = DependencyProperty.Register
            (
                nameof(OnCellEditEndingCommand), typeof(ICommand), typeof(DataGrid), new PropertyMetadata(null)
            );
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <inheritdoc/>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (ReferenceEquals(e.Source, e.OriginalSource))
            {
                if (AutoFocusOnMouseEnter)
                {
                    Focus();
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if (UseOuterScrollViewer)
            {
                if (GetOuterScrollViewer() is ScrollViewer scrollViewer)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonUp(e);
            if (e.OriginalSource is DependencyObject dp)
            {
                if (CoreHelper.GetVisualParent<DataGridColumnHeader>(dp) is DataGridColumnHeader header)
                {
                    switch (HeaderMode)
                    {
                        case DataGridHeaderMode.Command:
                            if (HeaderCommand?.CanExecute(header) ?? false)
                            {
                                HeaderCommand.Execute(header);
                            }
                            break;

                        case DataGridHeaderMode.ColumnSelector:
                            columnSelector.Show(header, Columns);
                            break;

                        case DataGridHeaderMode.None:
                        case DataGridHeaderMode.ContextMenu:
                            break;

                    }
                    e.Handled = HeaderMode != DataGridHeaderMode.ContextMenu;
                }
            }
        }

        /// <summary>
        /// Occurs when the selected item changes, raises the SelectionChanged event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            SelectedItemsList = SelectedItems;
        }

        /// <summary>
        /// Used by <see cref="DataGridColumns"/> to restore the sort
        /// </summary>
        /// <param name="column">The column. May be null</param>
        internal void OnSorting(DataGridColumn column)
        {
            if (column != null)
            {
                OnSorting(new DataGridSortingEventArgs(column));
            }
        }

        /// <summary>
        /// Occurs when the <see cref="DataGrid"/> is sorting, raises the Sorting event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSorting(DataGridSortingEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);
            if (view == null)
            {
                return;
            }

            /* Priority 1: Sorting command */
            if (SortingCommand != null)
            {
                view.SortDescriptions.Clear();
                PrepareForSort(e.Column);
                SortingCommand.Execute(e.Column);
                e.Handled = view.SortDescriptions.Count > 0;
                if (!e.Handled)
                {
                    /* If sort command didn't handle the sort, reverse the effects of PrepareForSort() */
                    PrepareForSort(e.Column);
                }
            }

            /* Priority 2: Custom sort spec */
            if (!e.Handled && e.Column.GetValue(CustomSortProperty) is DataGridColumnSortSpec sortSpec)
            {
                view.SortDescriptions.Clear();
                PrepareForSort(e.Column);

                string primaryPath = !string.IsNullOrEmpty(sortSpec.Column1) ?  sortSpec.Column1 :  e.Column.SortMemberPath;
                view.SortDescriptions.Add(new SortDescription(primaryPath, e.Column.SortDirection.Value));

                ListSortDirection secondaryDirection = sortSpec.Behavior switch
                {
                    DataGridColumnSortBehavior.AlwaysAscending => ListSortDirection.Ascending,
                    DataGridColumnSortBehavior.AlwaysDescending => ListSortDirection.Descending,
                    DataGridColumnSortBehavior.ReverseFollowPrimary => (e.Column.SortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending,
                    _ => e.Column.SortDirection.Value,
                };
                view.SortDescriptions.Add(new SortDescription(sortSpec.Column2, secondaryDirection));
                e.Handled = true;
            }

            /* Priority 3: Standard sort */
            if (!e.Handled)
            {
                base.OnSorting(e);
            }

            SetAttachedSortDirection(e.Column);
        }

        private void PrepareForSort(DataGridColumn column)
        {
            ListSortDirection? direction = column.SortDirection;

            foreach (DataGridColumn col in Columns)
            {
                col.SortDirection = null;
            }
            
            column.SortDirection = 
                (direction.HasValue && direction.Value == ListSortDirection.Ascending) ?
                ListSortDirection.Descending :
                ListSortDirection.Ascending;
        }

        private void SetAttachedSortDirection(DataGridColumn column)
        {
            foreach (DataGridColumn col in Columns)
            {
                DataGridColumns.SetSortDirection(col, null);
            }
            DataGridColumns.SetSortDirection(column, column.SortDirection);
        }

        /// <summary>
        /// Called when a context menu associated with this data grid is opening.
        /// </summary>
        /// <param name="e">The event arguments</param>
        /// <remarks>
        /// If the <see cref="ContextMenuOpeningCommand"/> has been set, this method calls the command that was established,
        /// passing event arguments <paramref name="e"/> to the command handler.
        /// </remarks>
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);

            if (ContextMenuOpeningCommand?.CanExecute(e) ?? false)
            {
                ContextMenuOpeningCommand.Execute(e);
            }
        }

        /// <summary>
        /// Called when the data grid begins edit.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBeginningEdit(DataGridBeginningEditEventArgs e)
        {
            base.OnBeginningEdit(e);
            if (OnBeginningEditCommand?.CanExecute(e) ?? false)
            {
                OnBeginningEditCommand.Execute(e);
            }
        }

        /// <summary>
        /// Called when the data grid ends cell editing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
        {
            base.OnCellEditEnding(e);
            if (OnCellEditEndingCommand?.CanExecute(e) ?? false)
            {
                OnCellEditEndingCommand.Execute(e);
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (DataGridColumn column in Columns)
            {
                if (DataGridColumns.GetAttachedOwner(column) is SysDataGrid dataGrid)
                {
                    if (ToolTipService.GetToolTip(column) is object toolTip)
                    {
                        if (GetDataGridColumnHeader(column, dataGrid) is DataGridColumnHeader header)
                        {
                            ToolTipService.SetInitialShowDelay(header, 170);
                            ToolTipService.SetShowDuration(header, 18000);
                            header.ToolTip = toolTip;
                        }
                    }

                }
            }
#if VERTOFFSET
            scrollViewer = CoreHelper.GetVisualChild<ScrollViewer>(this);

            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged += (s, e2) =>
                {
                    SetValue(ScrollViewerVerticalOffsetProperty, e2.VerticalOffset);
                };
            }
#endif
            DispatcherRestoreState();
        }

        private DataGridColumnHeader GetDataGridColumnHeader(DataGridColumn column, DependencyObject reference)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(reference); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(reference, i);

                if (child is DataGridColumnHeader header1 && header1.Column == column)
                {
                    return header1;
                }

                if (GetDataGridColumnHeader(column, child) is DataGridColumnHeader header2)
                {
                    return header2;
                }
            }

            return null;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DispatcherRestoreState();
        }

        private void DispatcherRestoreState()
        {
            if (RestoreStateBehavior != RestoreDataGridState.None)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback((p) =>
                {
                    RestoreState();
                    return null;
                }), null);
            }
        }

        /// <summary>
        /// Restores the state. This must be run from the Disptacher callback
        /// </summary>
        private void RestoreState()
        {
            object temp = SelectedItem;
            var b = RestoreStateBehavior;

            //  no item selected previously. 
            if (temp == null)
            {
                if (b.HasFlag(RestoreDataGridState.SelectFirst))
                {
                    Items.MoveCurrentToFirst();
                    temp = Items.CurrentItem;
                }

                if (b.HasFlag(RestoreDataGridState.SelectLast))
                {
                    Items.MoveCurrentToLast();
                    temp = Items.CurrentItem;
                }

                if (SelectionMode == DataGridSelectionMode.Extended)
                {
                    SelectedItems.Clear();
                }

                SelectedItem = temp;
            }

            // item selected previously
            else if (b.HasFlag(RestoreDataGridState.RestoreLastSelection))
            {
                if (SelectionMode == DataGridSelectionMode.Extended)
                {
                    SelectedItems.Clear();
                }
                SelectedItem = null;
                SelectedItem = temp;
            }

            // Now. If we have a selected item, scroll it into view
            if (SelectedItem != null)
            {
                ScrollIntoView(SelectedItem);
            }
        }

        /// <summary>
        /// Gets the outer scroll viewer. If cached, returns the cached object. Otherwise, looks it up.
        /// </summary>
        /// <returns>The scroll viewer, or null if none.</returns>
        private ScrollViewer GetOuterScrollViewer()
        {
            if (outerScrollViewer == null)
            {
                outerScrollViewer = CoreHelper.GetVisualParent<ScrollViewer>(this);
            }
            return outerScrollViewer;
        }
        #endregion

        /************************************************************************/

        #region Private helper class (ScrollInfo)
#if VERTOFFSET
        private ScrollViewer scrollViewer;
        private class ScrollInfo
        {
            public DataGrid Grid { get; private set; }
            public double VerticalOffset { get; private set; }
            public ScrollInfo(DataGrid grid, double verticalOffset)
            {
                Grid = grid;
                VerticalOffset = verticalOffset;
            }
        }
#endif
        #endregion
    }
}