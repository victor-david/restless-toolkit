using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SysDataGrid = System.Windows.Controls.DataGrid;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a bindable collection of data grid columns with convienence methods for adding items
    /// </summary>
    public class DataGridColumnCollection : ObservableCollection<DataGridColumn>
    {
        #region Private
        private DataGridColumn defaultSortColumn;
        private ListSortDirection defaultSortDirection;
        private SysDataGrid dataGridOwner;
        #endregion

        /************************************************************************/

        #region Internal
        internal SysDataGrid DataGridOwner
        {
            get => dataGridOwner;
            set
            {
                dataGridOwner = value;
                if (dataGridOwner is DataGrid tkDataGrid)
                {
                    tkDataGrid.ColumnStateChanged -= DataGridColumnStateChanged;
                    tkDataGrid.ColumnStateChanged += DataGridColumnStateChanged;
                }
            }
        }

        private void DataGridColumnStateChanged(object sender, EventArgs e)
        {
            OnColumnStateChanged(new ColumnStateChangedEventArgs(GetColumnState()));
        }
        #endregion

        /************************************************************************/

        #region Methods (column creation)
        /// <summary>
        /// Creates a text column and adds it to the collection
        /// </summary>
        /// <param name="header">The header for the column</param>
        /// <param name="bindingName">The name that the column should bind to.</param>
        /// <param name="targetNullValue">The value to use when the bound value is null</param>
        /// <returns>The newly created column</returns>
        public DataGridBoundColumn Create(string header, string bindingName, string targetNullValue = "--")
        {
            if (string.IsNullOrEmpty(header))
            {
                throw new ArgumentNullException(nameof(header));
            }

            if (string.IsNullOrWhiteSpace(bindingName))
            {
                throw new ArgumentNullException(nameof(bindingName));
            }

            DataGridTextColumn col = new DataGridTextColumn
            {
                Header = MakeTextBlockHeader(header),
                Binding = new Binding(bindingName)
                {
                    TargetNullValue = targetNullValue
                }
            };
            Add(col.SetSelectorName(header));
            return col;
        }

        /// <summary>
        /// Creates a text column that uses a IValueConverter to get its values
        /// </summary>
        /// <typeparam name="T">The converter type</typeparam>
        /// <param name="header">The header for the column</param>
        /// <param name="bindingName">The name that the column should bind to.</param>
        /// <param name="targetNullValue">The value to use when the bound value is null</param>
        /// <returns>The newly created column.</returns>
        public DataGridBoundColumn Create<T>(string header, string bindingName, string targetNullValue = "--") where T: IValueConverter, new()
        {
            if (string.IsNullOrEmpty(header))
            {
                throw new ArgumentNullException(nameof(header));
            }

            if (string.IsNullOrWhiteSpace(bindingName))
            {
                throw new ArgumentNullException(nameof(bindingName));
            }

            DataGridTextColumn col = new DataGridTextColumn
            {
                Header = MakeTextBlockHeader(header),
                Binding = new Binding(bindingName)
                {
                    Converter = new T(),
                    TargetNullValue = targetNullValue,
                }
            };
            Add(col.SetSelectorName(header));
            return col;
        }

        /// <summary>
        /// Creates a text column that uses a IMultiValueConverter to get its values
        /// </summary>
        /// <typeparam name="T">The converter type</typeparam>
        /// <param name="header">The header for the column</param>
        /// <param name="bindingNames">The names that the column should bind to.</param>
        /// <returns>The newly created column.</returns>
        public DataGridBoundColumn Create<T>(string header, params string[] bindingNames) where T: IMultiValueConverter, new()
        {
            DataGridTextColumn col = new DataGridTextColumn
            {
                Header = MakeTextBlockHeader(header)
            };

            MultiBinding multiBinding = new MultiBinding
            {
                Converter = new T()
            };
            foreach (string name in bindingNames)
            {
                multiBinding.Bindings.Add(new Binding(name));
            }
            col.Binding = multiBinding;
            col.Binding.TargetNullValue = "--";
            Add(col.SetSelectorName(header));
            return col;
        }

        /// <summary>
        /// Creates an image column that displays an image according to the specified converter.
        /// </summary>
        /// <typeparam name="T">The converter type.</typeparam>
        /// <param name="header">The column header string.</param>
        /// <param name="bindingName">The binding name, i.e the column name within the table.</param>
        /// <param name="parameter">A parameter to pass to the converter, ex: the name of the resource to use.</param>
        /// <param name="imageXY">The X and Y size of the image (default 12.0).</param>
        /// <returns>The newly created column.</returns>
        public DataGridTemplateColumn CreateImage<T>(string header, string bindingName, object parameter = null, double imageXY = 12.0) where T : IValueConverter, new()
        {
            DataGridTemplateColumn col = new DataGridTemplateColumn
            {
                Header = MakeTextBlockHeader(header),
                CanUserResize = false,
                Width = new DataGridLength(imageXY * 1.55, DataGridLengthUnitType.Pixel)
            };

            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Image));
            Binding binding = new Binding(bindingName)
            {
                Mode = BindingMode.OneWay,
                Converter = new T(),
                ConverterParameter = parameter
            };
            factory.SetValue(Image.SourceProperty, binding);
            factory.SetValue(Image.WidthProperty, imageXY);
            factory.SetValue(Image.HeightProperty, imageXY);
            col.CellTemplate = new DataTemplate()
            {
                VisualTree = factory
            };
            Add(col.SetSelectorName(header));
            return col;
        }

        /// <summary>
        /// Creates a content control column that displays the specified resource according to the specified converter.
        /// </summary>
        /// <typeparam name="T">The converter type.</typeparam>
        /// <param name="header">The column header string.</param>
        /// <param name="bindingName">The binding name, i.e the column name within the table.</param>
        /// <param name="parameter">A parameter to pass to the converter, ex: the name of the resource to use.</param>
        /// <returns>The newly created column.</returns>
        public DataGridTemplateColumn CreateResource<T>(string header, string bindingName, object parameter) where T : IValueConverter, new()
        {
            DataGridTemplateColumn col = new DataGridTemplateColumn
            {
                Header = MakeTextBlockHeader(header),
            };

            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(ContentControl));
            Binding binding = new Binding(bindingName)
            {
                Mode = BindingMode.OneWay,
                Converter = new T(),
                ConverterParameter = parameter
            };
            factory.SetValue(ContentControl.ContentProperty, binding);
            col.CellTemplate = new DataTemplate
            {
                VisualTree = factory
            };
            Add(col.SetSelectorName(header));
            return col;
        }
        #endregion

        /************************************************************************/

        #region Methods (sorting)
        /// <summary>
        /// Clears all column sort directions (built in and attached)
        /// and sets the specified column to the specified direction
        /// </summary>
        /// <param name="column">The column to set</param>
        /// <param name="direction">The direction</param>
        /// <exception cref="ArgumentNullException"><paramref name="column"/> is null</exception>
        public void SetInitialSort(DataGridColumn column, ListSortDirection direction)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            ClearColumnSortDirections();
            column.SetValue(DataGridColumns.SortDirectionProperty, direction);
            column.SortDirection = direction;
        }

        /// <summary>
        /// Clears all column sort directions (built in and attached)
        /// and sets the specified column to ascending
        /// </summary>
        /// <param name="column">The column to set</param>
        /// <exception cref="ArgumentNullException"><paramref name="column"/> is null</exception>
        public void SetInitialSortAscending(DataGridColumn column)
        {
            SetInitialSort(column, ListSortDirection.Ascending);
        }

        /// <summary>
        /// Clears all column sort directions (built in and attached)
        /// and sets the specified column to descending
        /// </summary>
        /// <param name="column">The column to set</param>
        /// <exception cref="ArgumentNullException"><paramref name="column"/> is null</exception>
        public void SetInitialSortDescending(DataGridColumn column)
        {
            SetInitialSort(column, ListSortDirection.Descending);
        }

        /// <summary>
        /// Clears all column sort directions (built in and attached)
        /// and sets the specified column to the specified direction
        /// </summary>
        /// <param name="columnIndex">The index of the column to set</param>
        /// <param name="direction">The direction</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="columnIndex"/> is out of range</exception>
        public void SetInitialSort(int columnIndex, ListSortDirection direction)
        {
            SetInitialSort(this[columnIndex], direction);
        }

        /// <summary>
        /// Clears all column sort directions (built in and attached)
        /// and sets the specified column to ascending
        /// </summary>
        /// <param name="columnIndex">The index of the column to set</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="columnIndex"/> is out of range</exception>
        public void SetInitialSortAscending(int columnIndex)
        {
            SetInitialSort(this[columnIndex], ListSortDirection.Ascending);
        }

        /// <summary>
        /// Clears all column sort directions (built in and attached)
        /// and sets the specified column to descending
        /// </summary>
        /// <param name="columnIndex">The index of the column to set</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="columnIndex"/> is out of range</exception>
        public void SetInitialSortDescending(int columnIndex)
        {
            SetInitialSort(this[columnIndex], ListSortDirection.Descending);
        }

        /// <summary>
        /// Sets the default sort column
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="sortDirection">The sort direction</param>
        [Obsolete("Use DataGridColumnExtensions.MakeInitialSort() or SetInitialSort() instead")]
        public void SetDefaultSort(DataGridColumn col, ListSortDirection? sortDirection)
        {
            defaultSortColumn = col;
            defaultSortDirection = sortDirection ?? ListSortDirection.Ascending;
            if (defaultSortColumn != null)
            {
                ClearColumnSortDirections();
                defaultSortColumn.SortDirection = defaultSortDirection;
            }
        }

        /// <summary>
        /// Restores the default sort established by <see cref="SetDefaultSort(DataGridColumn, ListSortDirection?)"/>
        /// </summary>
        [Obsolete("Use DataGridColumnExtensions.MakeInitialSort() or SetInitialSort() instead")]
        public void RestoreDefaultSort()
        {
            if (defaultSortColumn != null)
            {
                ClearColumnSortDirections();
                defaultSortColumn.SortDirection = defaultSortDirection;
            }
        }
        #endregion

        /************************************************************************/

        #region Methods (column state)
        /// <summary>
        /// Gets a string that represents the state of the columns
        /// </summary>
        /// <returns>
        /// A string that describes the state of this column collection
        /// </returns>
        public string GetColumnState()
        {
            StringBuilder builder = new StringBuilder();
                
            foreach (DataGridColumn column in this)
            {
                int isVisible = column.Visibility == Visibility.Visible ? 1 : 0;
                ListSortDirection? direction = DataGridColumns.GetSortDirection(column);
                int sort = direction.HasValue ? (int)direction.Value + 1 : 0;
                builder.Append($"{column.DisplayIndex};{isVisible};{sort},");
            }

            if (builder.Length > 0)
            {
                /* remove last comma */
                builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Restores the column state using the specified string.
        /// </summary>
        /// <param name="state">The state string</param>
        /// <remarks>
        /// Use the string obtained by <see cref="GetColumnState"/> to restore
        /// </remarks>
        public void RestoreColumnState(string state)
        {
            if (!string.IsNullOrWhiteSpace(state))
            {
                string[] cols = state.Split(',');
                for (int idx = 0; idx < cols.Length; idx++)
                {
                    if (idx < Count)
                    {
                        string[] parms = cols[idx].Split(';');
                        if (parms.Length > 0)
                        {
                            if (int.TryParse(parms[0], out int displayIndex))
                            {
                                this[idx].DisplayIndex = displayIndex;
                            }
                        }

                        if (parms.Length > 1)
                        {
                            if (int.TryParse(parms[1], out int isVisible))
                            {
                                this[idx].Visibility = isVisible == 0 ? Visibility.Collapsed : Visibility.Visible;
                            }
                        }

                        if (parms.Length > 2)
                        {
                            /* direction is stored 1 greater than its value so zero can signify none */
                            if (int.TryParse(parms[2], out int direction) && direction > 0)
                            {
                                ClearColumnSortDirections();
                                DataGridColumns.SetSortDirection(this[idx], (ListSortDirection)(direction - 1));
                            }
                        }
                    }
                }
            }
        }
        #endregion

        /************************************************************************/
        
        #region Events
        /// <summary>
        /// Occurs when the state of the columns has changed, display index, visibility
        /// </summary>
        public event EventHandler<ColumnStateChangedEventArgs> ColumnStateChanged;
        #endregion

        /************************************************************************/

        /// <summary>
        /// Occurs when the column states has changed. Raises the <see cref="ColumnStateChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnColumnStateChanged(ColumnStateChangedEventArgs e)
        {
            ColumnStateChanged?.Invoke(this, e);
        }

        #region Internal methods
        /// <summary>
        /// Saves the current values of display index in the attached property
        /// </summary>
        internal void SaveDisplayIndex()
        {
            foreach (DataGridColumn column in this)
            {
                DataGridColumns.SetDisplayIndex(column, column.DisplayIndex);
            }
        }

        /// <summary>
        /// Restores the values of display index from the attached property
        /// </summary>
        internal void RestoreDisplayIndex()
        {
            foreach (DataGridColumn column in this)
            {
                int displayIndex = DataGridColumns.GetDisplayIndex(column);
                if (displayIndex != -1)
                {
                    column.DisplayIndex = displayIndex;
                }
            }
        }

        /// <summary>
        /// Sequences the display index of columns in the collection
        /// </summary>
        internal void SequenceDisplayIndex()
        {
            int displayIdx = 0;
            foreach (DataGridColumn column in this)
            {
                column.DisplayIndex = displayIdx++;
            }
        }



        #endregion

        /************************************************************************/

        #region Private Methods
        private TextBlock MakeTextBlockHeader(string text)
        {
            return new TextBlock()
            {
                Text = text
            };
        }

        private void ClearColumnSortDirections()
        {
            foreach (DataGridColumn column in this)
            {
                column.SetValue(DataGridColumns.SortDirectionProperty, null);
                column.SortDirection = null;
            }
        }
        #endregion
    }
}