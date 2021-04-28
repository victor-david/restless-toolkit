using Restless.Toolkit.Controls;
using System.Windows.Controls;

namespace Restless.Toolkit.Core
{
    /// <summary>
    /// Provides static default values
    /// </summary>
    public static class Default
    {
        /// <summary>
        /// Provides static default formatting values.
        /// </summary>
        public static class Format
        {
            private static string date = "MMM dd, yyyy";

            /// <summary>
            /// Gets or sets the date format to use for both
            /// the <see cref="DataGridColumnExtensions.MakeDate(DataGridBoundColumn, string, int, bool)"/> extension
            /// and the popup calendar control.
            /// </summary>
            public static string Date
            {
                get => date;
                set
                {
                    date = value;
                    DataGridDate = value;
                    PopupCalendarDate = value;
                }
            }

            /// <summary>
            /// Gets or sets the default date format to use with the <see cref="DataGridColumnExtensions.MakeDate(DataGridBoundColumn, string, int, bool)"/> extension.
            /// </summary>
            public static string DataGridDate = "MMM dd, yyyy";

            /// <summary>
            /// Gets or sets the format used for the PopupCalendar control.
            /// </summary>
            public static string PopupCalendarDate = "MMM dd, yyyy";
        }
    }
}
