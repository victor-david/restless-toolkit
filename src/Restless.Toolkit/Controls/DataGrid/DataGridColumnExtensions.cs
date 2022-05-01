using Restless.Toolkit.Converters;
using Restless.Toolkit.Core;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DataBinding = System.Windows.Data.Binding;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides extension methods for DataGridColumn
    /// </summary>
    public static class DataGridColumnExtensions
    {
        #region Default style keys / default width
        /// <summary>
        /// Defines a default style key for a DataGridColumnHeader.
        /// </summary>
        [Obsolete("Use DataGrid.ColumnHeaderStyleKey")]
        public static object DefaultDataGridColumnHeaderStyleKey = null;

        /// <summary>
        /// Defines a default style key for a DataGridColumnHeader in order to center-align it.
        /// </summary>
        [Obsolete("Use DataGrid.ColumnHeaderCenteredStyleKey")]
        public static object CenterAlignedDataGridColumnHeaderStyleKey = null;

        /// <summary>
        /// Defines a default style key for a DataGridColumnHeader in order to right-align it.
        /// </summary>
        [Obsolete("Use DataGrid.ColumnHeaderRightStyleKey")]
        public static object RightAlignedDataGridColumnHeaderStyleKey = null;

        /// <summary>
        /// Defines a default style key for a TextBlock in order to center-align it.
        /// </summary>
        [Obsolete("Use DataGrid.ColumnHeaderCenteredStyleKey")]
        public static object CenterAlignedTextBlockStyleKey = null;

        /// <summary>
        /// Defines a default style key for a TextBlock in order to right-align it.
        /// </summary>
        [Obsolete("Use DataGrid.ColumnHeaderRightStyleKey")]
        public static object RightAlignedTextBlockStyleKey = null;

        /// <summary>
        /// Defines a default style key for a data grid cell in order to center align it.
        /// </summary>
        [Obsolete("Use DataGrid.DataGridCellCenteredStyleKey")]
        public static object CenterAlignedDataGridCellStyleKey = null;

        /// <summary>
        /// Defines a default style key for a data grid cell in order to right align it.
        /// </summary>
        [Obsolete("Use DataGrid.DataGridCellRightStyleKey")]
        public static object RightAlignedDataGridCellStyleKey = null;

        /// <summary>
        /// The default fixed width to be applied when using extension methods that accept an optional width parameter
        /// </summary>
        public const int DefaultColumnWidth = 100;
        #endregion

        /************************************************************************/

        /// <summary>
        /// Formats the column to display a date and makes the column fixed width.
        /// </summary>
        /// <param name="col">The column.</param>
        /// <param name="dateFormat">The desired date format. Null (the default) uses the application default format</param>
        /// <param name="width">The desired width. Default is <see cref="DefaultColumnWidth"/>.</param>
        /// <param name="toLocal">If true (the default), adds a converter to display the bound date as local date/time</param>
        /// <returns>The column</returns>
        public static DataGridBoundColumn MakeDate(this DataGridBoundColumn col, string dateFormat = null, int width = DefaultColumnWidth, bool toLocal = true)
        {
            if (toLocal)
            {
                ((DataBinding)col.Binding).Converter = new DateUtcToLocalConverter();
            }

            col.Binding.StringFormat = !string.IsNullOrEmpty(dateFormat) ? dateFormat : Default.Format.DataGridDate;
            col.MakeFixedWidth(width);
            return col;
        }

        /// <summary>
        /// Formats the column to display a number and makes the column fixed width.
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="numericFormat">The desired date format. Null (the default) uses a standard numeric format.</param>
        /// <param name="width">The desired width. Default is <see cref="DefaultColumnWidth"/>.</param>
        /// <returns>The column</returns>
        public static DataGridBoundColumn MakeNumeric(this DataGridBoundColumn col, string numericFormat = null, int width = DefaultColumnWidth)
        {
            col.Binding.StringFormat = string.IsNullOrWhiteSpace(numericFormat) ? "N0" : numericFormat;
            col.MakeFixedWidth(width);
            return col;
        }

        /// <summary>
        /// Sets the column's CellStyle property to the specified style.
        /// </summary>
        /// <param name="col">The column.</param>
        /// <param name="style">The style to set.</param>
        /// <returns>The column</returns>
        public static DataGridColumn AddCellStyle(this DataGridColumn col, Style style)
        {
            if (style != null && style.TargetType == typeof(DataGridCell))
            {
                col.CellStyle = style;
            }
            return col;
        }

        /// <summary>
        /// Sets the column's HeaderStyle property to the specified style.
        /// </summary>
        /// <param name="col">The column.</param>
        /// <param name="style">The style to set.</param>
        /// <returns>The column.</returns>
        public static DataGridColumn AddHeaderStyle(this DataGridColumn col, Style style)
        {
            if (style != null && style.TargetType == typeof(DataGridColumnHeader))
            {
                col.HeaderStyle = style;
            }
            return col;
        }

        /// <summary>
        /// Makes the column header and the cell style centered.
        /// </summary>
        /// <param name="col">The column.</param>
        /// <param name="headerStyleKey">
        /// The key of the style to apply to the header, or null to use <see cref="DataGrid.ColumnHeaderCenteredStyleKey"/>
        /// </param>
        /// <param name="cellStyleKey">
        /// The key of the style to apply to the cells, or null to use <see cref="DataGrid.DataGridCellCenteredStyleKey"/>
        /// </param>
        /// <returns>The column.</returns>
        public static DataGridColumn MakeCentered(this DataGridColumn col, object headerStyleKey = null, object cellStyleKey = null)
        {
            Style s1 = (Style)Application.Current.TryFindResource(headerStyleKey ?? DataGrid.ColumnHeaderCenteredStyleKey); 
            Style s2 = (Style)Application.Current.TryFindResource(cellStyleKey ?? DataGrid.DataGridCellCenteredStyleKey);
            return col.AddHeaderStyle(s1).AddCellStyle(s2);
        }

        /// <summary>
        /// Makes the column header and the cell style right aligned.
        /// </summary>
        /// <param name="col">The column.</param>
        /// <param name="headerStyleKey">
        /// The key of the style to apply to the header, or null to use <see cref="DataGrid.ColumnHeaderRightStyleKey"/>
        /// </param>
        /// <param name="cellStyleKey">
        /// The key of the style to apply to the cells, or null to use <see cref="DataGrid.DataGridCellRightStyleKey"/>
        /// </param>
        /// <returns>The column.</returns>
        public static DataGridColumn MakeRightAligned(this DataGridColumn col, object headerStyleKey = null, object cellStyleKey = null)
        {
            Style s1 = (Style)Application.Current.TryFindResource(headerStyleKey ?? DataGrid.ColumnHeaderRightStyleKey);
            Style s2 = (Style)Application.Current.TryFindResource(cellStyleKey ?? DataGrid.DataGridCellRightStyleKey);
            return col.AddHeaderStyle(s1).AddCellStyle(s2);
        }

        /// <summary>
        /// Makes the column fixed width, unable to resize
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="width">The desired width. Default is <see cref="DefaultColumnWidth"/>.</param>
        /// <returns>The column</returns>
        public static DataGridColumn MakeFixedWidth(this DataGridColumn col, int width = DefaultColumnWidth)
        {
            col.Width = width;
            col.CanUserResize = false;
            return col;
        }

        /// <summary>
        /// Makes the column flexible width.
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="width">The flex width factor, 1 is standard, 2 is double, etc.</param>
        /// <returns>The column</returns>
        public static DataGridColumn MakeFlexWidth(this DataGridColumn col, double width)
        {
            col.Width = new DataGridLength(width, DataGridLengthUnitType.Star);
            return col;
        }

        /// <summary>
        /// Makes the column read only.
        /// </summary>
        /// <param name="col">The column</param>
        /// <returns>The column</returns>
        public static DataGridColumn MakeReadOnly(this DataGridColumn col)
        {
            col.IsReadOnly = true;
            return col;
        }

        /// <summary>
        /// Makes the column single line display by attaching a converter that removes EOL.
        /// </summary>
        /// <param name="col">The column</param>
        /// <returns>The column</returns>
        public static DataGridBoundColumn MakeSingleLine(this DataGridBoundColumn col)
        {
            ((DataBinding)col.Binding).Converter = new StringToSingleLineConverter();
            return col;
        }

        /// <summary>
        /// Makes the column a masked display by attaching a converter.
        /// </summary>
        /// <param name="col">The column</param>
        /// <returns>The column</returns>
        public static DataGridBoundColumn MakeMasked(this DataGridBoundColumn col)
        {
            ((DataBinding)col.Binding).Converter = new StringToMaskedStringConverter();
            return col;
        }

        /// <summary>
        /// Gets or sets a value that determines whether the
        /// <see cref="AddToolTip(DataGridColumn, object)"/> method
        /// sets a deferred tooltip.
        /// </summary>
        public static bool UseDeferredToolTip = false;

        /// <summary>
        /// Adds the specified tooltip to the column header
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="toolTip">The tool tip</param>
        /// <returns>The column</returns>
        /// <remarks>
        /// <para>
        /// If <see cref="UseDeferredToolTip"/> is true, this method sets
        /// the <see cref="ToolTipService.ToolTipProperty"/> attached property
        /// which is processed after the data grid is loaded in order to establish
        /// the tool tip on the corresponding <see cref="DataGridColumnHeader"/>.
        /// You must use the toolkit <see cref="DataGrid"/> for this to work.
        /// </para>
        /// <para>
        /// If <see cref="UseDeferredToolTip"/> is false (the default), this method
        /// checks to see if the <see cref="DataGridColumn.Header"/> property is
        /// a FrameworkElement; if so, it sets its ToolTip property.
        /// </para>
        /// <para>
        /// If <see cref="UseDeferredToolTip"/> is false, you can still use the
        /// <see cref="AddToolTipDeferred(DataGridColumn, object)"/> extension
        /// method, with the same caveat that the data grid must be the toolkit
        /// <see cref="DataGrid"/>
        /// </para>
        /// <para>
        /// The advantage of setting the tool tip on <see cref="DataGridColumnHeader"/>
        /// is that the tool tip appears in any part of the header; otherwise, it
        /// only appears on the actual content of the header
        /// </para>
        /// </remarks>
        public static DataGridColumn AddToolTip(this DataGridColumn col, object toolTip)
        {
            if (UseDeferredToolTip)
            {
                return col.AddToolTipDeferred(toolTip);
            }

            if (col.Header is FrameworkElement element)
            {
                element.ToolTip = toolTip;
            }
            return col;
        }

        /// <summary>
        /// Adds the specified tool tip to the column header using deferred
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="toolTip">The tool tip</param>
        /// <returns>The column</returns>
        /// <remarks>
        /// This method requires that the data grid is a toolkit <see cref="DataGrid"/>.
        /// See <see cref="AddToolTip(DataGridColumn, object)"/> for more info.
        /// </remarks>
        public static DataGridColumn AddToolTipDeferred(this DataGridColumn col, object toolTip)
        {
            ToolTipService.SetToolTip(col, toolTip);
            return col;
        }

        /// <summary>
        /// Adds a sort specification to the column
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="column1">The name of the column to act as primary sort, or null to use the <paramref name="col"/></param>
        /// <param name="column2">The name of the column to act as a secondary sort.</param>
        /// <param name="behavior">The behavior of the secondary column when sorting.</param>
        /// <returns>The column</returns>
        /// <remarks>
        /// This extension adds a <see cref="DataGridColumnSortSpec"/> property to the column, creating
        /// a secondary sort on this column.
        /// </remarks>
        public static DataGridColumn AddSort(this DataGridColumn col, string column1, string column2, DataGridColumnSortBehavior behavior)
        {
            col.SetValue(DataGrid.CustomSortProperty, new DataGridColumnSortSpec(column1, column2, behavior));
            return col;
        }

        /// <summary>
        /// Adds a custom sort specification (alias of <see cref="AddSort(DataGridColumn, string, string, DataGridColumnSortBehavior)"/>
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="column1">The name of the column to act as primary sort, or null to use the <paramref name="col"/></param>
        /// <param name="column2">The name of the column to act as a secondary sort.</param>
        /// <param name="behavior">The behavior of the secondary column when sorting.</param>
        /// <returns>The column</returns> 
        public static DataGridColumn AddCustomSort(this DataGridColumn col, string column1, string column2, DataGridColumnSortBehavior behavior)
        {
            return col.AddSort(column1, column2, behavior);
        }

        /// <summary>
        /// Makes the column with have the specified initial sort
        /// </summary>
        /// <param name="column">The column</param>
        /// <param name="direction">The sort direction</param>
        /// <returns>The column</returns>
        public static DataGridColumn MakeInitialSort(this DataGridColumn column, ListSortDirection direction)
        {
            column.SetValue(DataGridColumns.SortDirectionProperty, direction);
            column.SortDirection = direction;
            return column;
        }

        /// <summary>
        /// Makes the column have an inital sort of ascending
        /// </summary>
        /// <param name="column">The column</param>
        /// <returns>The column</returns>
        public static DataGridColumn MakeInitialSortAscending(this DataGridColumn column)
        {
            return column.MakeInitialSort(ListSortDirection.Ascending);
        }

        /// <summary>
        /// Makes the column have an inital sort of descending
        /// </summary>
        /// <param name="column">The column</param>
        /// <returns>The column</returns>
        public static DataGridColumn MakeInitialSortDescending(this DataGridColumn column)
        {
            return column.MakeInitialSort(ListSortDirection.Descending);
        }

        /// <summary>
        /// Sets the selector name attached property for the column.
        /// </summary>
        /// <param name="column">The column</param>
        /// <param name="value">The selector name</param>
        /// <returns>The column</returns>
        public static DataGridColumn SetSelectorName(this DataGridColumn column, string value)
        {
            column.SetValue(DataGridColumns.SelectorNameProperty, value);
            return column;
        }
    }
}