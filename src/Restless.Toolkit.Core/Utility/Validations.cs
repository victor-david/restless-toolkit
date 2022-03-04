using System;

namespace Restless.Toolkit.Core.Utility
{
    /// <summary>
    /// Provides static methods to perform standard validation tasks.
    /// </summary>
    public class Validations
    {
        /// <summary>
        /// Gets a boolean value that indicates if the platform is at least Windows 7.
        /// </summary>
        public static bool RunningOnWindows7
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    return Environment.OSVersion.Version.CompareTo(new Version(6, 1)) >= 0;
                }
                return false;
            }
        }

        /// <summary>
        /// Throws an exception if <see cref="RunningOnWindows7"/> is false.
        /// </summary>
        public static void ThrowIfNotWindows7()
        {
            if (!RunningOnWindows7)
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}