using System;
using System.Windows;
using System.Windows.Media;
using DrawingColor = System.Drawing.Color;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides static helper methods (both standard and extension) for managing colors.
    /// </summary>
    public static class ColorHelper
    {
        #region Public methods
        /// <summary>
        /// Gets a <see cref="Color"/> structure from HSB values.
        /// </summary>
        /// <param name="hue">The hue value.</param>
        /// <param name="saturation">The saturation value.</param>
        /// <param name="brightness">The brightness value]</param>
        /// <returns>The created color.</returns>
        /// <remarks>Algorithm from https://en.wikipedia.org/wiki/HSL_and_HSV #From Hsv</remarks>
        public static Color ColorFromHSB(double hue, double saturation, double brightness)
        {
            hue = hue.Clamp(ColorValues.MinHue, ColorValues.MaxHue);
            saturation = saturation.Clamp(ColorValues.MinSaturation, ColorValues.MaxSaturation);
            brightness = brightness.Clamp(ColorValues.MinBrightness, ColorValues.MaxBrightness);

            byte f(double n)
            {
                double k = (n + hue / 60) % 6;
                double v = brightness - brightness * saturation * Math.Max(Math.Min(Math.Min(k, 4 - k), 1), 0);
                return (byte)Math.Round(v * 255);
            }
            return Color.FromRgb(f(5), f(3), f(1));
        }

        /// <summary>
        /// Gets the hue of the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The hue of the color</returns>
        public static double GetHue(this Color color)
        {
            return DrawingColor.FromArgb(color.A, color.R, color.G, color.B).GetHue();
        }

        /// <summary>
        /// Gets the brightness of the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The brightness of the color.</returns>
        public static double GetBrightness(this Color color)
        {
            DrawingColor drawingColor = DrawingColor.FromArgb(color.A, color.R, color.G, color.B);
            float saturation = drawingColor.GetSaturation();
            float lightness = drawingColor.GetBrightness();
            return lightness + saturation * Math.Min(lightness, 1 - lightness);
        }

        /// <summary>
        /// Gets the saturation of the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The saturation of the color</returns>
        public static double GetSaturation(this Color color)
        {
            DrawingColor drawingColor = DrawingColor.FromArgb(color.A, color.R, color.G, color.B);
            float lightness = drawingColor.GetBrightness();
            double brightness = color.GetBrightness();

            if (brightness == 0) return 0;
            
            return 2 - (2 * lightness / brightness);
        }

        /// <summary>
        /// Gets the specified color with its alpha component set to maximum (i.e. fully opaque)
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The original color with its alpha set to the maximum, i.e. fully opaque.</returns>
        public static Color ToMaxAlpha(this Color color)
        {
            return color.ToSpecifiedAlpha((byte)ColorValues.MaxAlpha);
        }

        /// <summary>
        /// Gets the specified color with its alpha component set to the specified value.
        /// </summary>
        /// <param name="color">The color</param>
        /// <param name="alpha">The alpha value to set.</param>
        /// <returns>The original color with its alpha set to the specified value.</returns>
        public static Color ToSpecifiedAlpha(this Color color, byte alpha)
        {
            color.A = alpha;
            return color;
        }

        /// <summary>
        /// Gets the specified point clamped to the inside of the specified element.
        /// </summary>
        /// <param name="point">The point</param>
        /// <param name="element">The element</param>
        /// <returns>A point inside the element</returns>
        public static Point ClampToElement(this Point point, FrameworkElement element)
        {
            point.X = Math.Min(Math.Max(0, point.X), element.ActualWidth);
            point.Y = Math.Min(Math.Max(0, point.Y), element.ActualHeight);
            return point;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private static double Clamp(this double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        #endregion
    }
}