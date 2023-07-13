using System;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents the arguments that are sent with a <see cref="DataGridColumnCollection.ColumnStateChanged"/> event
    /// </summary>
    public class ColumnStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the column state, represented as a string
        /// I;V;S[,next col,next col,etc] I=display index,V=visible(0/1),S=sort(0=none,1=asc,2=desc)
        /// </summary>
        public string State { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnStateChangedEventArgs"/> class
        /// </summary>
        public ColumnStateChangedEventArgs(string state)
        {
            State = state;
        }
    }
}