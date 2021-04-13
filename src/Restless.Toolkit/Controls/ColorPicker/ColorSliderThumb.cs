using System.Windows;
using System.Windows.Controls.Primitives;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides the thumb used on a <see cref="ColorSlider"/> control.
    /// </summary>
    public class ColorSliderThumb : Thumb
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSliderThumb"/> class.
        /// </summary>
        public ColorSliderThumb()
        {
        }

        static ColorSliderThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSliderThumb), new FrameworkPropertyMetadata(typeof(ColorSliderThumb)));
        }
        #endregion
    }
}