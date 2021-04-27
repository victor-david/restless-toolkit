using Restless.Toolkit.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a modal window used to display messages.
    /// </summary>
    public class MessageWindow : AppWindow
    {
        #region Constructors
        /// <summary>
        /// Creates a <see cref="MessageWindow"/> of type <see cref="MessageWindowType.YesNo"/> and displays it.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="owner">The window that owns the <see cref="MessageWindow"/>, or null for auto owner (if <paramref name="autoOwner"/> is true)</param>/// 
        /// <param name="autoOwner">true (default) to set owner automatically if <paramref name="owner"/> is null</param>
        /// <returns>true if affirmative selected by user; otherwise, false.</returns>
        public static bool ShowYesNo(string message, Window owner = null, bool autoOwner = true)
        {
            return Show(MessageWindowType.YesNo, message, owner, autoOwner);
        }

        /// <summary>
        /// Creates a <see cref="MessageWindow"/> of type <see cref="MessageWindowType.ContinueCancel"/> and displays it.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="owner">The window that owns the <see cref="MessageWindow"/>, or null for auto owner (if <paramref name="autoOwner"/> is true)</param>
        /// <param name="autoOwner">true (default) to set owner automatically if <paramref name="owner"/> is null</param>
        /// <returns>true if affirmative selected by user; otherwise, false.</returns>
        public static bool ShowContinueCancel(string message, Window owner = null, bool autoOwner = true)
        {
            return Show(MessageWindowType.ContinueCancel, message, owner, autoOwner);
        }

        /// <summary>
        /// Creates a <see cref="MessageWindow"/> of type <see cref="MessageWindowType.Okay"/> and displays it.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="owner">The window that owns the <see cref="MessageWindow"/>, or null for auto owner (if <paramref name="autoOwner"/> is true)</param>
        /// <param name="autoOwner">true (default) to set owner automatically if <paramref name="owner"/> is null</param> 
        /// <returns>always returns false.</returns>
        public static bool ShowOkay(string message, Window owner = null, bool autoOwner = true)
        {
            return Show(MessageWindowType.Okay, message, owner, autoOwner);
        }

        /// <summary>
        /// Creates a <see cref="MessageWindow"/> of type <see cref="MessageWindowType.Error"/> and displays it.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="owner">The window that owns the <see cref="MessageWindow"/>, or null for auto owner (if <paramref name="autoOwner"/> is true)</param>
        /// <param name="autoOwner">true (default) to set owner automatically if <paramref name="owner"/> is null</param> 
        /// <returns>always returns false.</returns>
        public static bool ShowError(string message, Window owner = null, bool autoOwner = true)
        {
            return Show(MessageWindowType.Error, message, owner, autoOwner);
        }

        /// <summary>
        /// Creates a <see cref="MessageWindow"/> of the specified type and displays it.
        /// </summary>
        /// <param name="type">The type of message window.</param>
        /// <param name="message">The message to display</param>
        /// <param name="owner">The window that owns the <see cref="MessageWindow"/>, or null for auto owner (if <paramref name="autoOwner"/> is true)</param>
        /// <param name="autoOwner">true (default) to set owner automatically if <paramref name="owner"/> is null</param> 
        /// <returns>true if affirmative selected by user; otherwise, false.</returns>
        private static bool Show(MessageWindowType type, string message, Window owner, bool autoOwner = true)
        {
            var modal = new MessageWindow(type, message, owner, autoOwner);
            modal.ShowDialog();
            return modal.DialogResult == true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageWindow"/> class
        /// </summary>
        private MessageWindow(MessageWindowType type, string message, Window owner, bool autoOwner)
        {
            MessageWindowType = type;
            ResizeMode = ResizeMode.NoResize;
            SizeToContent = SizeToContent.Height;
            Topmost = true;
            Owner = owner;
            if (owner == null && autoOwner)
            {
                Owner = Application.Current.MainWindow;
            }
            WindowStartupLocation = Owner == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
            Message = message;
            ButtonYesCommand = RelayCommand.Create((p) => Close(true));
            ButtonNoCommand = RelayCommand.Create((p) => Close(false));
        }

        static MessageWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageWindow), new FrameworkPropertyMetadata(typeof(MessageWindow)));
        }
        #endregion

        /************************************************************************/

        #region Message / message type
        /// <summary>
        /// Gets the type of message window.
        /// </summary>
        public MessageWindowType MessageWindowType
        {
            get => (MessageWindowType)GetValue(MessageWindowTypeProperty);
            private set => SetValue(MessageWindowTypePropertyKey, value);
        }

        private static readonly DependencyPropertyKey MessageWindowTypePropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(MessageWindowType), typeof(MessageWindowType), typeof(MessageWindow), new PropertyMetadata(MessageWindowType.Okay)
            );

        /// <summary>
        /// Identifies the <see cref="MessageWindowType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageWindowTypeProperty = MessageWindowTypePropertyKey.DependencyProperty;


        /// <summary>
        /// Gets the message text.
        /// </summary>
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            private set => SetValue(MessagePropertyKey, value);
        }

        private static readonly DependencyPropertyKey MessagePropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(Message), typeof(string), typeof(MessageWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="Message"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageProperty = MessagePropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Button properties
        /// <summary>
        /// Gets or sets the text to display on the affirmative button.
        /// </summary>
        public string ButtonYesText
        {
            get => (string)GetValue(ButtonYesTextProperty);
            set => SetValue(ButtonYesTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ButtonYesText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonYesTextProperty = DependencyProperty.Register
            (
                nameof(ButtonYesText), typeof(string), typeof(MessageWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets the text to use on the negative button.
        /// </summary>
        public string ButtonNoText
        {
            get => (string)GetValue(ButtonNoTextProperty);
            set => SetValue(ButtonNoTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ButtonNoText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonNoTextProperty = DependencyProperty.Register
            (
                nameof(ButtonNoText), typeof(string), typeof(MessageWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets the style that is applied to the action buttons.
        /// </summary>
        public Style ButtonStyle
        {
            get => (Style)GetValue(ButtonStyleProperty);
            set => SetValue(ButtonStyleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ButtonStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register
            (
                nameof(ButtonStyle), typeof(Style), typeof(MessageWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets the command used by the affirmative button
        /// </summary>
        public ICommand ButtonYesCommand
        {
            get => (ICommand)GetValue(ButtonYesCommandProperty);
            private set => SetValue(ButtonYesCommandPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ButtonYesCommandPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ButtonYesCommand), typeof(ICommand), typeof(MessageWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="ButtonYesCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonYesCommandProperty = ButtonYesCommandPropertyKey.DependencyProperty;


        /// <summary>
        /// Gets the command used by the negative button.
        /// </summary>
        public ICommand ButtonNoCommand
        {
            get => (ICommand)GetValue(ButtonNoCommandProperty);
            private set => SetValue(ButtonNoCommandPropertyKey, value);
        }

        private static readonly DependencyPropertyKey ButtonNoCommandPropertyKey = DependencyProperty.RegisterReadOnly
            (
                nameof(ButtonNoCommand), typeof(ICommand), typeof(MessageWindow), new PropertyMetadata(null)
            );

        /// <summary>
        /// Identifies the <see cref="ButtonNoCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonNoCommandProperty = ButtonNoCommandPropertyKey.DependencyProperty;
        #endregion

        /************************************************************************/

        #region Private methods
        private void Close(bool result)
        {
            DialogResult = result;
            Close();
        }
        #endregion
    }
}