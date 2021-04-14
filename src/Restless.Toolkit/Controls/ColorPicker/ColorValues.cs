using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides static values used for various color operations.
    /// </summary>
    /// <remarks>
    /// Although many of the values provided here (such as <see cref="DefaultRed"/>)
    /// are actually of type byte, they are provided as type double because they are
    /// used as default values of dependency properties that are of type double
    /// because they are bound to sliders that require a double.
    /// </remarks>
    public static class ColorValues
    {
        /// <summary>
        /// Gets the default starting color.
        /// </summary>
        public static readonly Color DefaultColor = Colors.Red;

        /// <summary>
        /// Gets the hex code of the default starting color.
        /// </summary>
        public static readonly string DefaultHexCode = DefaultColor.ToString();

        /// <summary>
        /// Gets the red component of the default starting color.
        /// </summary>
        public static readonly double DefaultRed = DefaultColor.R;

        /// <summary>
        /// Gets the green component of the default starting color.
        /// </summary>
        public static readonly double DefaultGreen = DefaultColor.G;

        /// <summary>
        /// Gets the blue component of the default starting color.
        /// </summary>
        /// 
        public static readonly double DefaultBlue = DefaultColor.B;

        /// <summary>
        /// Gets the alpha component of the default starting color.
        /// </summary>
        public static readonly double DefaultAlpha = DefaultColor.A;

        /// <summary>
        /// Get the minimum value for hue.
        /// </summary>
        public const double MinHue = 0;

        /// <summary>
        /// Get the maximum value for hue.
        /// </summary>
        public const double MaxHue = 360;

        /// <summary>
        /// Get the minimum value for saturation.
        /// </summary>
        public const double MinSaturation = 0;

        /// <summary>
        /// Get the maximum value for saturation.
        /// </summary>
        public const double MaxSaturation = 1;

        /// <summary>
        /// Get the minimum value for brightness
        /// </summary>
        public const double MinBrightness = 0;

        /// <summary>
        /// Get the maximum value for brightness.
        /// </summary>
        public const double MaxBrightness = 1;

        /// <summary>
        /// Gets the minimum value of an Rgba color component.
        /// </summary>
        public const double MinRgbaComponent = 0;

        /// <summary>
        /// Gets the maximum value of an Rgba color component.
        /// </summary>
        public const double MaxRgbaComponent = 255;

        /// <summary>
        /// Gets the maximum value for alpha.
        /// </summary>
        public const double MaxAlpha = MaxRgbaComponent;
    }
}