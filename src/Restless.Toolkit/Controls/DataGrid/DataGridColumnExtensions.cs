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
        public static object DefaultDataGridColumnHeaderStyleKey = null;

        /// <summary>
        /// Defines a default style key for a DataGridColumnHeader in order to center-align it.
        /// </summary>
        public static object CenterAlignedDataGridColumnHeaderStyleKey = null;

        /// <summary>
        /// Defines a default style key for a DataGridColumnHeader in order to right-align it.
        /// </summary>
        public static object RightAlignedDataGridColumnHeaderStyleKey = null;

        /// <summary>
        /// Defines a default style key for a TextBlock in order to center-align it.
        /// </summary>
        [Obsolete("Use CenterAlignedDataGridCellStyleKey")]
        public static object CenterAlignedTextBlockStyleKey = null;

        /// <summary>
        /// Defines a default style key for a TextBlock in order to right-align it.
        /// </summary>
        [Obsolete("Use RightAlignedDataGridCellStyleKey")]
        public static object RightAlignedTextBlockStyleKey = null;

        /// <summary>
        /// Defines a default style key for a data grid cell in order to center align it.
        /// </summary>
        public static object CenterAlignedDataGridCellStyleKey = null;

        /// <summary>
        /// Defines a default style key for a data grid cell in order to right align it.
        /// </summary>
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
            if (string.IsNullOrEmpty(numericFormat))
            {
                numericFormat = "N0";
            }
            col.Binding.StringFormat = numericFormat;
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
        /// The key of the style to apply to the header, or null to use <see cref="CenterAlignedDataGridColumnHeaderStyleKey"/>
        /// </param>
        /// <param name="cellStyleKey">
        /// The key of the style to apply to the cells, or null to use <see cref="CenterAlignedTextBlockStyleKey"/>
        /// </param>
        /// <returns>The column.</returns>
        /// <remarks>
        /// For this method to work, two styles must be available, you must pass <paramref name="headerStyleKey"/> and <paramref name="cellStyleKey"/>
        /// or set <see cref="CenterAlignedDataGridColumnHeaderStyleKey"/>
        /// and <see cref="CenterAlignedDataGridCellStyleKey"/> before calling this method.
        /// </remarks>
        public static DataGridColumn MakeCentered(this DataGridColumn col, object headerStyleKey = null, object cellStyleKey = null)
        {
            Style s1 = (Style)Application.Current.TryFindResource(headerStyleKey ?? CenterAlignedDataGridColumnHeaderStyleKey ?? "5090c02e-fd0d-4f5c-a084-73f9107c93a7"); 
            Style s2 = (Style)Application.Current.TryFindResource(cellStyleKey ?? CenterAlignedDataGridCellStyleKey ?? "94220548-aee0-4740-810c-9fc5536b5e79");
            return col.AddHeaderStyle(s1).AddCellStyle(s2);
        }

        /// <summary>
        /// Makes the column header and the cell style right aligned.
        /// </summary>
        /// <param name="col">The column.</param>
        /// <param name="headerStyleKey">
        /// The key of the style to apply to the header, or null to use <see cref="RightAlignedDataGridColumnHeaderStyleKey"/>
        /// </param>
        /// <param name="cellStyleKey">
        /// The key of the style to apply to the cells, or null to use <see cref="RightAlignedTextBlockStyleKey"/>
        /// </param>
        /// <returns>The column.</returns>
        /// <remarks>
        /// For this method to work, two styles must be available, you must pass <paramref name="headerStyleKey"/> and <paramref name="cellStyleKey"/>
        /// or set <see cref="RightAlignedDataGridColumnHeaderStyleKey"/>
        /// and <see cref="RightAlignedDataGridCellStyleKey"/> before calling this method.
        /// </remarks>
        public static DataGridColumn MakeRightAligned(this DataGridColumn col, object headerStyleKey = null, object cellStyleKey = null)
        {
            Style s1 = (Style)Application.Current.TryFindResource(headerStyleKey ?? RightAlignedDataGridColumnHeaderStyleKey ?? "05b1bdba-1c8a-400b-9cce-4ceb5fae5feb");
            Style s2 = (Style)Application.Current.TryFindResource(cellStyleKey ?? RightAlignedDataGridCellStyleKey ?? "dbace134-2a49-4898-bc89-3736ee6001c9");
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
        /// Adds the specified tooltip to the column's header
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="toolTip">The tool tip</param>
        /// <returns>The column</returns>
        /// <remarks>
        /// <para>
        ///   This method attempts to add a tool tip to the column header.
        ///   If the column header is a TextBlock object, it sets the ToolTip property
        ///   of the TextBlock to the specified text.
        /// </para>
        /// <para>
        ///   Otherwise, it attempts to add the tooltip via the HeaderStyle property of the column.
        ///   If HeaderStyle is null, it first adds the DataGridHeaderDefault style. It then 
        ///   checks the Setters property of the style to see if a new Setter may be added. If so, it adds
        ///   a ToolTipService.ToolTipProperty property setter with the specified tooltip value.
        /// </para>
        /// <para>
        ///   If the HeaderStyle property has already been set (for instance, via a previous call to
        ///   <see cref="MakeCentered(DataGridColumn, object, object)"/>, the HeaderStyle.Setters collection is sealed.
        ///   Under these conditions, this method does not set the tooltip text and no error is thrown.
        /// </para>
        /// </remarks>
        public static DataGridColumn AddToolTip(this DataGridColumn col, object toolTip)
        {
            if (toolTip != null)
            {
                if (col.Header is TextBlock textBlock)
                {
                    textBlock.ToolTip = toolTip;
                }
                else
                {
                    if (col.HeaderStyle == null)
                    {
                        col.HeaderStyle = new Style
                            (
                                typeof(DataGridColumnHeader),
                                (Style)Application.Current.TryFindResource(DefaultDataGridColumnHeaderStyleKey ?? "a6398b9a-3fc4-4dc4-9d86-2ffa681ce514")
                            );
                    }

                    if (col.HeaderStyle != null && !col.HeaderStyle.Setters.IsSealed)
                    {
                        col.HeaderStyle.Setters.Add(new Setter(ToolTipService.ToolTipProperty, toolTip));
                    }
                }
            }
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
    }
}