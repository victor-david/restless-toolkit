namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides the values for <see cref="DataGrid.HeaderMode"/>
    /// </summary>
    public enum DataGridHeaderMode
    {
        /// <summary>
        /// No action taken when right clicking a header
        /// </summary>
        None,
        /// <summary>
        /// Execute the assigned header command if it exists
        /// </summary>
        Command,
        /// <summary>
        /// Display the coulumn selector
        /// </summary>
        ColumnSelector,
        /// <summary>
        /// Allow the context menu (if any) to appear
        /// </summary>
        ContextMenu,
    }
}