using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides an enumeration of values that affect a <see cref="MessageWindow"/>
    /// </summary>
    public enum MessageWindowType
    {
        /// <summary>
        /// Presents yes/no options.
        /// </summary>
        YesNo,
        /// <summary>
        /// Presents continue/cancel options.
        /// </summary>
        ContinueCancel,
        /// <summary>
        /// Presents a single option.
        /// </summary>
        Okay,
        /// <summary>
        /// Presents a single option
        /// </summary>
        Error,
    }
}
