using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using SysDataGrid = System.Windows.Controls.DataGrid;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides attached properties that augment the use of <see cref="DataGridColumnCollection"/>
    /// </summary>
    /// <remarks>
    /// See:
    /// http://stackoverflow.com/questions/3065758/wpf-mvvm-datagrid-dynamic-columns
    /// </remarks>
    public static class DataGridColumns
    {
        #region Columns (attached property)
        private const string Columns = nameof(Columns);

        /// <summary>
        /// Defines an attached dependency property that enables the consumer to manipulate the data columns from a view model.
        /// </summary>
        /// <remarks>
        /// This attached property is loosely based on the article at:
        /// http://stackoverflow.com/questions/3065758/wpf-mvvm-datagrid-dynamic-columns
        /// </remarks>
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached
            (
                Columns, typeof(DataGridColumnCollection), typeof(DataGridColumns), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                    PropertyChangedCallback = OnColumnsPropertyChanged
                }
            );

        /// <summary>
        /// Gets the <see cref="ColumnsProperty"/> for the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to get the property for.</param>
        /// <returns>The attached property, or null if none.</returns>
        public static DataGridColumnCollection GetColumns(DependencyObject obj)
        {
            return (DataGridColumnCollection)obj.GetValue(ColumnsProperty);
        }

        /// <summary>
        /// Sets the <see cref="ColumnsProperty"/> on the specified element.
        /// </summary>
        /// <param name="obj">The dependency object to set the property on.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetColumns(DependencyObject obj, DataGridColumnCollection value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        private static void OnColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SysDataGrid dataGrid && e.NewValue is DataGridColumnCollection columns)
            {
                columns.DataGridOwner = dataGrid;

                PropertyInfo prop = typeof(DataGridColumn).GetProperty("DataGridOwner", BindingFlags.Instance | BindingFlags.NonPublic);

                dataGrid.Columns.Clear();
                DataGridColumn sortColumn = null;
                // DataGridColumn defaultSortColumn = null;

                foreach (DataGridColumn col in columns)
                {
                    /* Must set internal DataGridOwner to null or WPF throws (it sets it) */
                    prop?.SetValue(col, null);
                    col.SetValue(AttachedOwnerProperty, dataGrid);
                    dataGrid.Columns.Add(col);

                    if (GetSortDirection(col) is ListSortDirection direction)
                    {
                        sortColumn = col;
                        sortColumn.SortDirection = ReversedDirection(direction);
                    }
                }

                /* Perform the sort as needed. If sortColumn is null, the method does nothing */
                (dataGrid as DataGrid)?.OnSorting(sortColumn);

                columns.CollectionChanged -= ColumnsCollectionChanged;
                columns.CollectionChanged += ColumnsCollectionChanged;
            }
        }

        private static ListSortDirection ReversedDirection(ListSortDirection direction)
        {
            return direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
        }

        private static void ColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SysDataGrid dataGrid = (sender as DataGridColumnCollection)?.DataGridOwner ?? throw new ArgumentException("Invalid data grid owner");

            if (e.NewItems != null)
            {
                foreach (DataGridColumn column in e.NewItems.OfType<DataGridColumn>())
                {
                    dataGrid.Columns.Add(column);
                }
            }

            if (e.OldItems != null)
            {
                foreach (DataGridColumn column in e.OldItems.OfType<DataGridColumn>())
                {
                    dataGrid.Columns.Remove(column);
                }
            }
        }
        #endregion

        /************************************************************************/

        #region DefaultSort (internal)
        //private const string DefaultSort = nameof(DefaultSort);

        //internal static readonly DependencyProperty DefaultSortProperty = DependencyProperty.RegisterAttached
        //    (
        //        DefaultSort, typeof(ListSortDirection?), typeof(DataGridColumns), new FrameworkPropertyMetadata()
        //        {
        //            DefaultValue = null,
        //        }
        //    );

        //internal static ListSortDirection? GetDefaultSort(DependencyObject obj)
        //{
        //    return (ListSortDirection?)obj.GetValue(DefaultSortProperty);
        //}

        //internal static void SetDefaultSort(DependencyObject obj, ListSortDirection? value)
        //{
        //    obj.SetValue(DefaultSortProperty, value);
        //}
        #endregion

        /************************************************************************/

        #region SortDirection (internal)
        private const string SortDirection = nameof(SortDirection);

        internal static readonly DependencyProperty SortDirectionProperty = DependencyProperty.RegisterAttached
            (
                SortDirection, typeof(ListSortDirection?), typeof(DataGridColumns), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                }
            );

        internal static ListSortDirection? GetSortDirection(DependencyObject obj)
        {
            return (ListSortDirection?)obj.GetValue(SortDirectionProperty);
        }

        internal static void SetSortDirection(DependencyObject obj, ListSortDirection? value)
        {
            obj.SetValue(SortDirectionProperty, value);
        }
        #endregion

        /************************************************************************/

        #region AttachedOwner (internal)
        private const string AttachedOwnerPropertyName = "AttachedOwner";

        internal static readonly DependencyProperty AttachedOwnerProperty = DependencyProperty.RegisterAttached
            (
                AttachedOwnerPropertyName, typeof(SysDataGrid), typeof(DataGridColumns), new FrameworkPropertyMetadata()
                {
                    DefaultValue = null,
                }
            );

        internal static SysDataGrid GetAttachedOwner(DependencyObject obj)
        {
            return (SysDataGrid)obj.GetValue(AttachedOwnerProperty);
        }

        internal static void SetAttachedOwner(DependencyObject obj, SysDataGrid value)
        {
            obj.SetValue(AttachedOwnerProperty, value);
        }
        #endregion
    }
}