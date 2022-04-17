using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        #endregion

        /************************************************************************/

        #region Internal
        internal SysDataGrid DataGridOwner
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Public methods
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
            Add(col);
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
            Add(col);
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
            Add(col);
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
            Add(col);
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
            Add(col);
            return col;
        }

        /// <summary>
        /// Sets the default sort column
        /// </summary>
        /// <param name="col">The column</param>
        /// <param name="sortDirection">The sort direction</param>
        [Obsolete("Use DataGridColumnExtensions.MakeInitialSort() instead")]
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
        [Obsolete("Use DataGridColumnExtensions.MakeInitialSort() instead")]
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
                column.SortDirection = null;
            }
        }
        #endregion
    }
}