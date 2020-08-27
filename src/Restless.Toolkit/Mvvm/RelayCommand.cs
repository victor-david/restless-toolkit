﻿using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Restless.Toolkit.Mvvm
{
    /// <summary>
    /// Represents a command. This is the class from which all all application commands are created.
    /// </summary>
    public class RelayCommand : ICommand 
    { 
        #region Private 
        private readonly Action<object> execute; 
        private readonly Predicate<object> canExecute;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets or sets a value that determines if the command is supported.
        /// </summary>
        public CommandSupported Supported
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a parameter that is associated with this command.
        /// </summary>
        public object Parameter
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor (private)
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The method that executes the command.</param>
        /// <param name="canExecute">The method that checks if this command can execute. If null, no check is performed.</param>
        /// <param name="supported">A value that determines if the command is supported.</param>
        /// <param name="parameter">The parameter, or null if none needed.</param>
        private RelayCommand(Action<object> execute, Predicate<object> canExecute, CommandSupported supported, object parameter) 
        {
            this.execute = execute ?? throw new ArgumentNullException("execute"); 
            this.canExecute = canExecute;
            Supported = supported;
            Parameter = parameter;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Creates and returns an instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">The method that executes the command</param>
        /// <param name="canExecute">The method that checks if this command can execute. If null, no check is performed.</param>
        /// <param name="supported">A value that determines if the command is supported.</param>
        /// <param name="parameter">The parameter, or null if none needed.</param>
        /// <returns>A <see cref="RelayCommand"/> object.</returns>
        public static RelayCommand Create(Action<object> execute, Predicate<object> canExecute, CommandSupported supported, object parameter)
        {
            return new RelayCommand(execute, canExecute, supported, parameter);
        }

        /// <summary>
        /// Creates and returns an instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">The method that executes the command</param>
        /// <param name="canExecute">The method that checks if this command can execute. If null, no check is performed.</param>
        /// <param name="supported">A value that determines if the command is supported.</param>
        /// <returns>A <see cref="RelayCommand"/> object.</returns>
        public static RelayCommand Create(Action<object> execute, Predicate<object> canExecute, CommandSupported supported)
        {
            return new RelayCommand(execute, canExecute, supported, null);
        }

        /// <summary>
        /// Creates and returns an instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">The method that executes the command.</param>
        /// <param name="canExecute">The method that checks if this command can execute. If null, no check is performed.</param>
        /// <returns>A <see cref="RelayCommand"/> object.</returns>
        /// <remarks>This overload creates a command that is marked as supported.</remarks>
        public static RelayCommand Create(Action<object> execute, Predicate<object> canExecute)
        {
            return new RelayCommand(execute, canExecute, CommandSupported.Yes, null);
        }

        /// <summary>
        /// Creates and returns an instance of <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">The method that executes the command.</param>
        /// <returns>A <see cref="RelayCommand"/> object.</returns>
        /// <remarks>This overload creates a command that has no corresponding command predicate and is marked as supported.</remarks>
        public static RelayCommand Create(Action<object> execute)
        {
            return new RelayCommand(execute, null, CommandSupported.Yes, null);
        }
        #endregion

        /************************************************************************/

        #region ICommand Members
        /// <summary>
        /// Checks to see if this command can execute.
        /// </summary>
        /// <param name="parameter">An object to pass to the command evaulator method.</param>
        /// <returns>true if the command can excecute; otherwise, false.</returns>
        [DebuggerStepThrough] 
        public bool CanExecute(object parameter) 
        {
            parameter = Parameter ?? parameter;

            switch (Supported)
            {
                case CommandSupported.No:
                    return false;
                default:
                    return canExecute == null ? true : canExecute(parameter); 
            }
        } 
        
        /// <summary>
        /// Occurs when the conditions that affect whether a command may excute change.
        /// </summary>
        public event EventHandler CanExecuteChanged 
        { 
            add => CommandManager.RequerySuggested += value; 
            remove => CommandManager.RequerySuggested -= value; 
        } 
        
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter">An object to pass to the command method.</param>
        public void Execute(object parameter) 
        {
            switch (Supported)
            {
                case CommandSupported.Yes:
                    parameter = Parameter ?? parameter;
                    execute(parameter);
                    break;
                case CommandSupported.No:
                    break;
                case CommandSupported.NoWithException:
                    throw new NotSupportedException("The command is not supported.");
            }
        } 
        #endregion 
    }
}