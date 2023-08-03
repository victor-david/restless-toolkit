using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Restless.Toolkit.Resource
{
    /// <summary>
    /// Provides static resource keys for Button resources 
    /// and static read-only properties to obtain the keyed resources programmatically.
    /// </summary>
    public static class Button
    {
        #region Keys
        /// <summary>
        /// Identifies the resource key for Button default style
        /// </summary>
        public static ResourceKey DefaultStyleKey { get; } = Create();

        /// <summary>
        /// Identifies the resource key for Button bordered style
        /// </summary>
        public static ResourceKey BorderedStyleKey { get; } = Create();

        /// <summary>
        /// Identifies the resource key for Button compact bordered style
        /// </summary>
        public static ResourceKey CompactBorderedStyleKey { get; } = Create();

        /// <summary>
        /// Identifies the resource key for Button border brush
        /// </summary>
        public static ResourceKey BorderBrushKey { get; } = Create();

        /// <summary>
        /// Identifies the resource key for CheckBox foreground brush
        /// </summary>
        public static ResourceKey ForegroundBrushKey { get; } = Create();
        #endregion

        /************************************************************************/

        #region Resources
        /// <summary>
        /// Gets the style with resource key <see cref="DefaultStyleKey"/>
        /// </summary>
        public static Style DefaultStyle => ResourceHelper.Get<Style>(DefaultStyleKey);

        /// <summary>
        /// Gets the style with resource key <see cref="BorderedStyleKey"/>
        /// </summary>
        public static Style BorderedStyle => ResourceHelper.Get<Style>(BorderedStyleKey);

        /// <summary>
        /// Gets the style with resource key <see cref="BorderedStyleKey"/>
        /// </summary>
        public static Style CompactBorderedStyle => ResourceHelper.Get<Style>(CompactBorderedStyleKey);

        /// <summary>
        /// Gets the brush with resource key <see cref="BorderBrushKey"/>
        /// </summary>
        public static Brush BorderBrush => ResourceHelper.Get<Brush>(BorderBrushKey);

        /// <summary>
        /// Gets the brush with resource key <see cref="ForegroundBrushKey"/>
        /// </summary>
        public static Brush ForegroundBrush => ResourceHelper.Get<Brush>(ForegroundBrushKey);
        #endregion

        /************************************************************************/

        #region Private methods
        private static ComponentResourceKey Create([CallerMemberName] string resourceId = null)
        {
            return new ComponentResourceKey(typeof(Button), resourceId);
        }
        #endregion
    }
}