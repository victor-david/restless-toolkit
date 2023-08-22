using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Restless.Toolkit.Resource
{
    /// <summary>
    /// Provides static resource keys for resources.
    /// </summary>
    public static class ResourceKeys
    {
        #region Brushes (Defaults)
        /// <summary>
        /// Identifies the resource key for default border brush.
        /// </summary>
        public static readonly ComponentResourceKey DefaultBackgroundBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for default border brush.
        /// </summary>
        public static readonly ComponentResourceKey DefaultBorderBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for default rollover border brush.
        /// </summary>
        public static readonly ComponentResourceKey DefaultRolloverBorderBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for default foreground brush
        /// </summary>
        public static readonly ComponentResourceKey DefaultForegroundBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for default disabled foreground brush
        /// </summary>
        public static readonly ComponentResourceKey DefaultDisabledForegroundBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for default header brush.
        /// </summary>
        public static readonly ComponentResourceKey DefaultHeaderBrushKey = Create();
        #endregion

        /************************************************************************/

        #region Brushes (Button)
        /// <summary>
        /// Identifies the resource key for button border brush.
        /// </summary>
        public static readonly ComponentResourceKey ButtonBorderBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for Button foreground brush
        /// </summary>
        public static readonly ComponentResourceKey ButtonForegroundBrushKey = Create();
        #endregion

        /************************************************************************/

        #region Brushes (CheckBox)
        /// <summary>
        /// Identifies the resource key for CheckBox border brush
        /// </summary>
        public static readonly ComponentResourceKey CheckBoxBorderBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for CheckBox foreground brush
        /// </summary>
        public static readonly ComponentResourceKey CheckBoxForegroundBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for CheckBox checked brush
        /// </summary>
        public static readonly ComponentResourceKey CheckBoxCheckedBrushKey = Create();
        #endregion

        /************************************************************************/

        #region Brushes (ComboBox)
        /// <summary>
        /// Identifies the resource key for ComboBox background brush
        /// </summary>
        public static readonly ComponentResourceKey ComboBoxBackgroundBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for ComboBox border brush
        /// </summary>
        public static readonly ComponentResourceKey ComboBoxBorderBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for ComboBox border brush
        /// </summary>
        public static readonly ComponentResourceKey ComboBoxItemBorderBrushKey = Create();
        #endregion

        /************************************************************************/

        #region Brushes (Path)
        /// <summary>
        /// Identifies the resource key for Path fill brush
        /// </summary>
        public static readonly ComponentResourceKey PathFillBrushKey = Create();
        #endregion

        /************************************************************************/

        #region Brushes (TextBox)
        /// <summary>
        /// Identifies the resource key for TextBox border brush
        /// </summary>
        public static readonly ComponentResourceKey TextBoxBorderBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for TextBox foreground brush
        /// </summary>
        public static readonly ComponentResourceKey TextBoxForegroundBrushKey = Create();

        /// <summary>
        /// Identifies the resource key for TextBox background brush
        /// </summary>
        public static readonly ComponentResourceKey TextBoxBackgroundBrushKey = Create();
        #endregion

        /************************************************************************/

        #region Brushes (Window)
        /// <summary>
        /// Identifies the resource key for window background brush.
        /// </summary>
        public static readonly ComponentResourceKey WindowBackgroundBrush = Create();

        /// <summary>
        /// Identifies the resource key for window border brush.
        /// </summary>
        public static readonly ComponentResourceKey WindowBorderBrush = Create();

        /// <summary>
        /// Identifies the resource key for window title bar border brush.
        /// </summary>
        public static readonly ComponentResourceKey WindowTitleBarBorderBrush = Create();

        /// <summary>
        /// Identifies the resource key for window title bar background brush.
        /// </summary>
        public static readonly ComponentResourceKey WindowTitleBarBackgroundBrush = Create();

        /// <summary>
        /// Identifies the resource key for window title bar foreground brush.
        /// </summary>
        public static readonly ComponentResourceKey WindowTitleBarForegroundBrush = Create();

        /// <summary>
        /// Identifies the resource key for window title bar button brush.
        /// </summary>
        public static readonly ComponentResourceKey WindowTitleBarButtonBrush = Create();

        /// <summary>
        /// Identifies the resource key for window menu border brush.
        /// </summary>
        public static readonly ComponentResourceKey WindowMenuBorderBrush = Create();

        /// <summary>
        /// Identifies the resource key for window menu background brush.
        /// </summary>
        public static readonly ComponentResourceKey WindowMenuBackgroundBrush = Create();
        #endregion

        /************************************************************************/

        #region Button
        /// <summary>
        /// Identifies the resource key for Button default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultButtonStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for Button bordered style
        /// </summary>
        public static readonly ComponentResourceKey BorderedButtonStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for Button compact bordered style
        /// </summary>
        public static readonly ComponentResourceKey CompactBorderedButtonStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for Button status bar style
        /// </summary>
        public static readonly ComponentResourceKey StatusBarButtonStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for splitter grid toggle button style.
        /// </summary>
        public static readonly ComponentResourceKey SplitterGridToggleButtonStyleKey = Create();
        #endregion

        /************************************************************************/

        #region CheckBox
        /// <summary>
        /// Identifies the resource key for CheckBox default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultCheckBoxStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for the element style used in <see cref="DataGridCheckBoxColumn"/>
        /// </summary>
        public static readonly ComponentResourceKey CheckBoxColumnElementStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for the editing element style used in <see cref="DataGridCheckBoxColumn"/>
        /// </summary>
        public static readonly ComponentResourceKey CheckBoxColumnEditingElementStyleKey = Create();
        #endregion

        /************************************************************************/

        #region ComboBox
        /// <summary>
        /// Identifies the resource key for ComboBox default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultComboBoxStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for ComboBox toggle button default style
        /// </summary>
        public static readonly ComponentResourceKey ComboBoxToggleButtonStyleKey = Create();
        #endregion

        /************************************************************************/

        #region DataGrid (Aliases)
        /// <summary>
        /// Identifies the resource key for data grid default style.
        /// This is an alias for <see cref="Controls.DataGrid.DataGridStyleKey"/>.
        /// </summary>
        public static readonly ComponentResourceKey DefaultDataGridStyleKey = Controls.DataGrid.DataGridStyleKey;

        /// <summary>
        /// Identifies the resource key for data grid column header default style.
        /// This is an alias for <see cref="Controls.DataGrid.ColumnHeaderStyleKey"/>.
        /// </summary>
        public static readonly ComponentResourceKey DefaultDataGridColumnHeaderStyleKey = Controls.DataGrid.ColumnHeaderStyleKey;

        /// <summary>
        /// Identifies the resource key for data grid cell default style.
        /// This is an alias for <see cref="Controls.DataGrid.DataGridCellStyleKey"/>.
        /// </summary>
        public static readonly ComponentResourceKey DefaultDataGridCellStyleKey = Controls.DataGrid.DataGridCellStyleKey;
        #endregion

        /************************************************************************/

        #region Element
        /// <summary>
        /// Identifies the resource key for an animated framework element style
        /// </summary>
        public static readonly ComponentResourceKey FrameworkElementOpacityAnimationStyleKey = Create();
        #endregion

        /************************************************************************/

        #region TextBox
        /// <summary>
        /// Identifies the resource key for TextBox default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultTextBoxStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for data grid cell edit TextBox style.
        /// </summary>
        public static readonly ComponentResourceKey DataGridCellEditTextBoxStyleKey = Create();
        #endregion

        /************************************************************************/

        #region Panel
        /// <summary>
        /// Identifies the resource key for StatusBar default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultStatusBarStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for vertical grid splitter default style
        /// </summary>
        public static readonly ComponentResourceKey VerticalGridSplitterStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for horizontal grid splitter default style
        /// </summary>
        public static readonly ComponentResourceKey HorizontalGridSplitterStyleKey = Create();
        #endregion

        /************************************************************************/

        #region Path
        /// <summary>
        /// Identifies the resource key for Path default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultPathStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for Path medium size style
        /// </summary>
        public static readonly ComponentResourceKey MediumPathStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for Path small size style
        /// </summary>
        public static readonly ComponentResourceKey SmallPathStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for Path used in tab close
        /// </summary>
        public static readonly ComponentResourceKey TabClosePathStyleKey = Create();

        /// <summary>
        /// Identifies the respource key for chevron down geometry
        /// </summary>
        public static readonly ComponentResourceKey ChevronDownGeometryKey = Create();

        /// <summary>
        /// Identifies the respource key for chevron left geometry
        /// </summary>
        public static readonly ComponentResourceKey ChevronLeftGeometryKey = Create();

        /// <summary>
        /// Identifies the respource key for chevron right geometry
        /// </summary>
        public static readonly ComponentResourceKey ChevronRightGeometryKey = Create();

        /// <summary>
        /// Identifies the respource key for chevron up geometry
        /// </summary>
        public static readonly ComponentResourceKey ChevronUpGeometryKey = Create();
        #endregion

        /************************************************************************/

        #region TabItem
        /// <summary>
        /// Identifies the resource key for TabItem default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultTabItemStyleKey = Create();

        /// <summary>
        /// Identifies the resource key for TabItem default template
        /// </summary>
        public static readonly ComponentResourceKey DefaultTabItemTemplateKey = Create();
        #endregion

        /************************************************************************/

        #region System
        /// <summary>
        /// Identifies the resource key for minimum header height.
        /// </summary>
        public static readonly ComponentResourceKey MinHeaderHeightKey = Create();

        /// <summary>
        /// Identifies the resource key for default header padding.
        /// </summary>
        public static readonly ComponentResourceKey DefaultHeaderPaddingKey = Create();

        /// <summary>
        /// Identifies the resource key for minimum data grid row height.
        /// </summary>
        public static readonly ComponentResourceKey MinDataGridRowHeightKey = Create();

        /// <summary>
        /// Identifies the resource key for minimum textbox height.
        /// </summary>
        public static readonly ComponentResourceKey MinTextBoxHeightKey = Create();

        /// <summary>
        /// Identifies the resource key for window border thickness.
        /// </summary>
        public static readonly ComponentResourceKey WindowBorderThicknessKey = Create();

        /// <summary>
        /// Identifies the resource key for textbox border thickness.
        /// </summary>
        public static readonly ComponentResourceKey TextBoxBorderThicknessKey = Create();
        #endregion

        /************************************************************************/

        #region Private
        private static ComponentResourceKey Create([CallerMemberName] string resourceId = null)
        {
            return new ComponentResourceKey(typeof(ResourceKeys), resourceId);
        }
        #endregion
    }
}