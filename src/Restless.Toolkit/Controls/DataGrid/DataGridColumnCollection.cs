using Restless.Toolkit.Resource;
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

        #region Public
        /// <summary>
        /// Gets the default target null value
        /// </summary>
        public const string DefaultTargetNullValue = "--";
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
        /// Creates a <see cref="DataGridTextColumn"/> and adds it to the collection.
        /// </summary>
        /// <param name="header">The header for the column.</param>
        /// <param name="bindingName">The name that the column binds to.</param>
        /// <param name="targetNullValue">The value to use when the bound value is null.</param>
        /// <returns>The newly created column.</returns>
        public DataGridTextColumn Create(object header, string bindingName, string targetNullValue = DefaultTargetNullValue)
        {
            ValidateBinding(bindingName);

            DataGridTextColumn col = new DataGridTextColumn
            {
                Header = MakeHeaderControl(header),
                Binding = new Binding(bindingName)
                {
                    TargetNullValue = targetNullValue
                }
            };
            Add(col.SetSelectorName(GetHeaderText(col.Header)));
            return col;
        }

        /// <summary>
        /// Creates a <see cref="DataGridNullableTextColumn"/> if <paramref name="isNullable"/> is true,
        /// or a <see cref="DataGridTextColumn"/> if <paramref name="isNullable"/> is false, and adds
        /// the column to the collection.
        /// </summary>
        /// <param name="header">The header for the column.</param>
        /// <param name="bindingName">The name that the column binds to.</param>
        /// <param name="isNullable">true to create a <see cref="DataGridNullableTextColumn"/></param>
        /// <param name="targetNullValue">
        /// The value to use when the bound value is null. 
        /// Ignored if <paramref name="isNullable"/> is true.
        /// </param>
        /// <returns>The newly created column.</returns>
        public DataGridBoundColumn Create(object header, string bindingName, bool isNullable, string targetNullValue = DefaultTargetNullValue)
        {
            if (isNullable)
            {
                return CreateNullableText(header, bindingName);
            }
            return Create(header, bindingName, targetNullValue);
        }

        /// <summary>
        /// Creates a <see cref="DataGridTextColumn"/> that uses an <see cref="IValueConverter"/> to get its values,
        /// amd adds it to the collection.
        /// </summary>
        /// <typeparam name="T">The converter type</typeparam>
        /// <param name="header">The header for the column.</param>
        /// <param name="bindingName">The name that the column binds to.</param>
        /// <param name="targetNullValue">The value to use when the bound value is null</param>
        /// <returns>The newly created column.</returns>
        public DataGridTextColumn Create<T>(object header, string bindingName, string targetNullValue = null) where T : IValueConverter, new()
        {
            ValidateBinding(bindingName);

            DataGridTextColumn col = new DataGridTextColumn
            {
                Header = MakeHeaderControl(header),
                Binding = new Binding(bindingName)
                {
                    Converter = new T(),
                    TargetNullValue = targetNullValue,
                }
            };
            Add(col.SetSelectorName(GetHeaderText(col.Header)));
            return col;
        }

        /// <summary>
        /// Creates a <see cref="DataGridTextColumn"/> that uses a <see cref="IMultiValueConverter"/> to get its values,
        /// and adds it to the collection.
        /// </summary>
        /// <typeparam name="T">The converter type</typeparam>
        /// <param name="header">The header for the column</param>
        /// <param name="targetNullValue">The value to use when the bound value is null</param>
        /// <param name="bindingNames">The names that the column binds to.</param>
        /// <returns>The newly created column.</returns>
        public DataGridTextColumn Create<T>(object header, object targetNullValue, params string[] bindingNames) where T : IMultiValueConverter, new()
        {
            DataGridTextColumn col = new DataGridTextColumn
            {
                Header = MakeHeaderControl(header)
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
            col.Binding.TargetNullValue = targetNullValue;
            Add(col.SetSelectorName(GetHeaderText(col.Header)));
            return col;
        }

        /// <summary>
        /// Creates a <see cref="DataGridTextColumn"/>n that uses a <see cref="IMultiValueConverter"/> to get its values,
        /// and adds it to the collection. This overload uses <see cref="DefaultTargetNullValue"/>.
        /// </summary>
        /// <typeparam name="T">The converter type</typeparam>
        /// <param name="header">The header for the column</param>
        /// <param name="bindingNames">The names that the column binds to.</param>
        /// <returns>The newly created column.</returns>
        public DataGridTextColumn Create<T>(string header, params string[] bindingNames) where T : IMultiValueConverter, new()
        {
            return Create<T>(header, DefaultTargetNullValue, bindingNames);
        }

        /// <summary>
        /// Creates a <see cref="DataGridTemplateColumn"/> that displays an image according to the specified converter,
        /// and adds it to the collection.
        /// </summary>
        /// <typeparam name="T">The converter type.</typeparam>
        /// <param name="header">The column header string.</param>
        /// <param name="bindingName">The binding name, i.e the column name within the table.</param>
        /// <param name="parameter">A parameter to pass to the converter, ex: the name of the resource to use.</param>
        /// <param name="imageXY">The X and Y size of the image (default 12.0).</param>
        /// <returns>The newly created column.</returns>
        public DataGridTemplateColumn CreateImage<T>(object header, string bindingName, object parameter = null, double imageXY = 12.0) where T : IValueConverter, new()
        {
            DataGridTemplateColumn col = new DataGridTemplateColumn
            {
                Header = MakeHeaderControl(header),
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
            factory.SetValue(FrameworkElement.WidthProperty, imageXY);
            factory.SetValue(FrameworkElement.HeightProperty, imageXY);
            col.CellTemplate = new DataTemplate()
            {
                VisualTree = factory
            };
            Add(col.SetSelectorName(GetHeaderText(col.Header)));
            return col;
        }

        /// <summary>
        /// Creates a <see cref="DataGridTemplateColumn"/> that displays the specified resource according to the specified converter,
        /// and adds it to the collection.
        /// </summary>
        /// <typeparam name="T">The converter type.</typeparam>
        /// <param name="header">The column header string.</param>
        /// <param name="bindingName">The binding name, i.e the column name within the table.</param>
        /// <param name="parameter">A parameter to pass to the converter, ex: the name of the resource to use.</param>
        /// <returns>The newly created column.</returns>
        public DataGridTemplateColumn CreateResource<T>(object header, string bindingName, object parameter) where T : IValueConverter, new()
        {
            ValidateBinding(bindingName);

            DataGridTemplateColumn col = new DataGridTemplateColumn
            {
                Header = MakeHeaderControl(header),
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
            Add(col.SetSelectorName(GetHeaderText(col.Header)));
            return col;
        }

        /// <summary>
        /// Creates a <see cref="DataGridCheckBoxColumn"/>, and adds it to the collection.
        /// </summary>
        /// <param name="header">The header text</param>
        /// <param name="bindingName">The binding name</param>
        /// <param name="isThreeState">Whether the check box is three state</param>
        /// <param name="bindingMode">The binding mode, default is <see cref="BindingMode.TwoWay"/></param>
        /// <param name="elementStyle">The element style</param>
        /// <param name="editingElementStyle">The editing element style</param>
        /// <returns>The newly created <see cref="DataGridCheckBoxColumn"/></returns>
        public DataGridCheckBoxColumn CreateCheckBox(
            object header,
            string bindingName,
            bool isThreeState,
            BindingMode bindingMode = BindingMode.TwoWay,
            Style elementStyle = null,
            Style editingElementStyle = null)
        {
            ValidateBinding(bindingName);

            DataGridCheckBoxColumn col = new DataGridCheckBoxColumn()
            {
                Binding = new Binding(bindingName)
                {
                    Mode = bindingMode,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                },
                Header = MakeHeaderControl(header),
                IsThreeState = isThreeState,

            };

            if (elementStyle != null)
            {
                col.ElementStyle = elementStyle;
            }

            if (editingElementStyle != null)
            {
                col.EditingElementStyle = editingElementStyle;
            }

            Add(col.SetSelectorName(GetHeaderText(col.Header)));
            return col;
        }

        /// <summary>
        /// Creates a two state <see cref="DataGridCheckBoxColumn"/>, and adds it to the collection.
        /// </summary>
        /// <param name="header">The header text</param>
        /// <param name="bindingName">The binding name</param>
        /// <param name="bindingMode">The binding mode, default is <see cref="BindingMode.TwoWay"/></param>
        /// <param name="elementStyle">The element style</param>
        /// <param name="editingElementStyle">The editing element style</param>
        /// <returns>The newly created <see cref="DataGridCheckBoxColumn"/></returns>
        public DataGridCheckBoxColumn CreateCheckBox(
            object header,
            string bindingName,
            BindingMode bindingMode = BindingMode.TwoWay,
            Style elementStyle = null,
            Style editingElementStyle = null)
        {
            return CreateCheckBox(header, bindingName, false, bindingMode, elementStyle, editingElementStyle);
        }

        /// <summary>
        /// Creates a two state, two-way bound <see cref="DataGridCheckBoxColumn"/> with default 
        /// element and editing element styles, and adds it to the collection.
        /// </summary>
        /// <param name="header">The header</param>
        /// <param name="bindingName">The binding name</param>
        /// <returns>The newly created <see cref="DataGridCheckBoxColumn"/></returns>
        public DataGridCheckBoxColumn CreateCheckBox(object header, string bindingName)
        {
            return CreateCheckBox(
                header, bindingName, false, BindingMode.TwoWay,
                ResourceHelper.Get<Style>(ResourceKeys.CheckBoxColumnElementStyleKey),
                ResourceHelper.Get<Style>(ResourceKeys.CheckBoxColumnEditingElementStyleKey));
        }

        /// <summary>
        /// Creates a <see cref="DataGridNullableTextColumn"/>, and adds it to the collection.
        /// </summary>
        /// <param name="header">The header</param>
        /// <param name="bindingName">The binding name</param>
        /// <returns>The newly created <see cref="DataGridNullableTextColumn"/></returns>
        public DataGridNullableTextColumn CreateNullableText(object header, string bindingName)
        {
            ValidateBinding(bindingName);

            DataGridNullableTextColumn col = new DataGridNullableTextColumn()
            {
                Binding = new Binding(bindingName),
                Header = MakeHeaderControl(header)
            };
            Add(col.SetSelectorName(GetHeaderText(col.Header)));
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
                                /* make sure display index within bounds */
                                if (displayIndex >= 0 && displayIndex < Count)
                                {
                                    this[idx].DisplayIndex = displayIndex;
                                }
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

        #region Protected methods
        /// <summary>
        /// Occurs when the column states has changed. Raises the <see cref="ColumnStateChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnColumnStateChanged(ColumnStateChangedEventArgs e)
        {
            ColumnStateChanged?.Invoke(this, e);
        }
        #endregion

        /************************************************************************/

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
        private void ValidateBinding(string bindingName)
        {
            if (string.IsNullOrWhiteSpace(bindingName))
            {
                throw new ArgumentNullException(nameof(bindingName));
            }
        }

        private FrameworkElement MakeHeaderControl(object header)
        {
            if (header != null)
            {
                if (header is string text)
                {
                    return new TextBlock() { Text = text };
                }
                return new ContentControl() { Content = header };
            }

            return null;
        }

        private string GetHeaderText(object header)
        {
            return (header is TextBlock text) ? text.Text : header?.ToString();
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