namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents HSB color components. This class is used by <see cref="MainPicker"/>
    /// as event arguments when one of its color components changes.
    /// </summary>
    internal class ColorComponents
    {
        /// <summary>
        /// Gets the hue.
        /// </summary>
        public double Hue { get; }
        /// <summary>
        /// Gets the saturation.
        /// </summary>
        public double Saturation { get; }
        /// <summary>
        /// Gets the brightness.
        /// </summary>
        public double Brightness { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorComponents"/> class.
        /// </summary>
        /// <param name="hue">The hue value</param>
        /// <param name="saturation">The saturation value.</param>
        /// <param name="brightness">The brightness value.</param>
        public ColorComponents(double hue, double saturation, double brightness)
        {
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>A string that contains the HSB values.</returns>
        public override string ToString()
        {
            return $"H: {Hue} S:{Saturation} B: {Brightness}"; ;
        }
    }
}