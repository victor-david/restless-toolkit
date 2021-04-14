using System;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides enumeration values used by <see cref="ColorPicker"/>
    /// to configure the display of its sliders.
    /// </summary>
    [Flags]
    public enum ColorSliderConfig
    {
        /// <summary>
        /// No sliders are displayed.
        /// </summary>
        None = 0,
        /// <summary>
        /// Alpha slider is displayed.
        /// </summary>
        Alpha = 1,
        /// <summary>
        /// Rgb sliders are displayed.
        /// </summary>
        Rgb = 2,
        /// <summary>
        /// Alpha and Rgb sliders displayed.
        /// </summary>
        AlphaRgb = Alpha | Rgb,
        /// <summary>
        /// Default, alpha and Rgb sliders displayed.
        /// </summary>
        Default = AlphaRgb,


    }
}
