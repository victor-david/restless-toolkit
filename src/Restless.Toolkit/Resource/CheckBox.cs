using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Restless.Toolkit.Resource
{
    /// <summary>
    /// Provides static resource keys for CheckBox resources 
    /// and static read-only properties to obtain the keyed resources programmatically.
    /// </summary>
    public static class CheckBox
    {
        #region Keys
        /// <summary>
        /// Identifies the resource key for CheckBox default style
        /// </summary>
        public static ResourceKey DefaultStyleKey { get; } = Create();

        /// <summary>
        /// Identifies the resource key for CheckBox border brush
        /// </summary>
        public static ResourceKey BorderBrushKey { get; } = Create();

        /// <summary>
        /// Identifies the resource key for CheckBox checked brush
        /// </summary>
        public static ResourceKey CheckedBrushKey { get; } = Create();

        /// <summary>
        /// Identifies the resource key for CheckBox foreground brush
        /// </summary>
        public static ResourceKey ForegroundBrushKey { get; } = Create();

        /// <summary>
        /// Identifies the resource key for CheckBox disabled foreground brush
        /// </summary>
        public static ResourceKey DisabledForegroundBrushKey { get; } = Create();
        #endregion

        /************************************************************************/

        #region Resources
        /// <summary>
        /// Gets the style with resource key <see cref="DefaultStyleKey"/>
        /// </summary>
        public static Style DefaultStyle => ResourceHelper.Get<Style>(DefaultStyleKey);

        /// <summary>
        /// Gets the brush with resource key <see cref="BorderBrushKey"/>
        /// </summary>
        public static Brush BorderBrush => ResourceHelper.Get<Brush>(BorderBrushKey);

        /// <summary>
        /// Gets the brush with resource key <see cref="CheckedBrushKey"/>
        /// </summary>
        public static Brush CheckedBrush => ResourceHelper.Get<Brush>(CheckedBrushKey);

        /// <summary>
        /// Gets the brush with resource key <see cref="ForegroundBrushKey"/>
        /// </summary>
        public static Brush ForegroundBrush => ResourceHelper.Get<Brush>(ForegroundBrushKey);

        /// <summary>
        /// Gets the brush with resource key <see cref="DisabledForegroundBrushKey"/>
        /// </summary>
        public static Brush DisabledForegroundBrush => ResourceHelper.Get<Brush>(DisabledForegroundBrushKey);
        #endregion

        /************************************************************************/

        #region Private methods
        private static ComponentResourceKey Create([CallerMemberName] string resourceId = null)
        {
            return new ComponentResourceKey(typeof(CheckBox), resourceId);
        }
        #endregion
    }
}