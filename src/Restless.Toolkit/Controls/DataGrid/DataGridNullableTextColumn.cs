using System.Windows;
using System.Windows.Controls;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a data grid column that contains text and displays a null indicator
    /// if its content is null.
    /// </summary>
    public class DataGridNullableTextColumn : DataGridTextColumn
    {
        #region Resource keys
        /// <summary>
        /// Identifies the resource key for the default element style
        /// </summary>
        public static readonly ComponentResourceKey DefaultElementStyleKey = new ComponentResourceKey(typeof(DataGridNullableTextColumn), nameof(DefaultElementStyleKey));

        /// <summary>
        /// Identifies the resource key for the default null indicator.
        /// </summary>
        public static readonly ComponentResourceKey NullIndicatorElementKey = new ComponentResourceKey(typeof(DataGridNullableTextColumn), nameof(NullIndicatorElementKey));
        #endregion

        /// <summary>
        /// The default style used for the column element
        /// </summary>
        public new Style DefaultElementStyle => Application.Current.TryFindResource(DefaultElementStyleKey) as Style;


        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridNullableTextColumn"/> class
        /// </summary>
        public DataGridNullableTextColumn()
        {
            ElementStyle = DefaultElementStyle;
        }

        /// <summary>
        /// Gets a <see cref="Label"/> control that is bound to the column's Binding property value.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
        /// <returns>A new <see cref="Label"/> control that is bound to the column's Binding property value.</returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            Label label = new Label();
            label.SetBinding(ContentControl.ContentProperty, Binding);
            label.ContentStringFormat = Binding.StringFormat;
            if (ElementStyle != null && ElementStyle.TargetType == typeof(Label))
            {
                label.Style = ElementStyle;
            }
            return label;
        }
    }
}