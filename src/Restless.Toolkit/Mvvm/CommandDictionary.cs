namespace Restless.Toolkit.Mvvm
{
    /// <summary>
    /// Extends <see cref="CommandDictionaryBase{T}"/> to provide a command dictionary that uses string keys.
    /// </summary>
    /// <remarks>
    /// A CommandDictionary collection is used by the various view models and associated controllers
    /// to create commands without the need to declare a separate property for each one.
    /// </remarks>
    public class CommandDictionary : CommandDictionaryBase<string>
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDictionary"/> class.
        /// </summary>
        public CommandDictionary()
        {
        }
        #endregion
    }
}