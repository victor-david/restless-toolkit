using System;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides enumeration values used by <see cref="ColorPicker"/>
    /// to configure the display of its demo components.
    /// </summary>
    [Flags]
    public enum ColorDemoConfig
    {
        /// <summary>
        /// No demo components are displayed.
        /// </summary>
        None = 0,
        /// <summary>
        /// Demo canvas is displayed.
        /// </summary>
        Canvas = 1,
        /// <summary>
        /// Demo hex code is displayed.
        /// </summary>
        HexCode = 2,
        /// <summary>
        /// Demo canvas and hex code displayed.
        /// </summary>
        CanvasHexCode = Canvas | HexCode,
        /// <summary>
        /// Default, demo canvas and hex code displayed.
        /// </summary>
        Default = CanvasHexCode,
    }
}