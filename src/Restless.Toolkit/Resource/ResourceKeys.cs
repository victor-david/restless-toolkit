using System.Runtime.CompilerServices;
using System.Windows;

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
        #endregion

        /************************************************************************/

        #region CheckBox
        /// <summary>
        /// Identifies the resource key for CheckBox default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultCheckBoxStyleKey = Create();
        #endregion

        /************************************************************************/

        #region TextBox
        /// <summary>
        /// Identifies the resource key for TextBox default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultTextBoxStyleKey = Create();
        #endregion

        /************************************************************************/

        #region Panel
        /// <summary>
        /// Identifies the resource key for StatusBar default style
        /// </summary>
        public static readonly ComponentResourceKey DefaultStatusBarStyleKey = Create();
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
        /// Identifies the resource key for minimum textbox height.
        /// </summary>
        public static readonly ComponentResourceKey MinTextBoxHeightKey = Create();

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