﻿using System;

namespace Restless.Toolkit.Mvvm
{
    /// <summary>
    /// Represents the base class for all view models.This class must be inherited.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject, IDisposable
    {
        #region Private
        private string displayName;
        private bool isActivated;
        #endregion

        /************************************************************************/

        #region Public properties
        /// <summary>
        /// Gets the view model that owns this view model, or null if none.
        /// </summary>
        public ViewModelBase Owner
        {
            get;
        }

        /// <summary>
        /// Gets the display name for this instance.
        /// </summary>
        public string DisplayName
        {
            get => displayName;
            protected set => SetProperty(ref displayName, value);
        }

        /// <summary>
        /// Gets a boolean value that indicates if this VM is active.
        /// </summary>
        /// <remarks>
        /// The activation status of a view model is changed with
        /// the <see cref="Activate"/> and <see cref="Deactivate"/> methods.
        /// When this property is set to true, the <see cref="OnActivated"/> method
        /// is called. When false, the <see cref="OnDeactivated"/> method is called.
        /// </remarks>
        public bool IsActivated
        {
            get => isActivated;
            private set
            {
                if (SetProperty(ref isActivated, value))
                {
                    if (isActivated)
                    {
                        OnActivated();
                    }
                    else
                    {
                        OnDeactivated();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a dictionary of commands.
        /// </summary>
        public CommandDictionary Commands
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="owner">The owner of this view model, or null if none.</param>
        protected ViewModelBase(ViewModelBase owner)
        {
            Owner = owner;
            Commands = new CommandDictionary();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase() : this(null)
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Activates the view model.
        /// </summary>
        public void Activate()
        {
            IsActivated = true;
        }

        /// <summary>
        /// Deactivates the view model.
        /// </summary>
        public void Deactivate()
        {
            IsActivated = false;
        }

        /// <summary>
        /// Toggles the view model between activated and deactivated.
        /// </summary>
        public void ToggleActivation()
        {
            if (!IsActivated)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        /// <summary>
        /// Toggles the specifed view model between activated and deactivated.
        /// </summary>
        /// <param name="vm">The <see cref="ViewModelBase"/> to toggle activation for.</param>
        public void ToggleActivation(ViewModelBase vm)
        {
            if (vm != null)
            {
                vm.ToggleActivation();
                OnActivationToggled(vm);
            }
        }

        /// <summary>
        /// Causes the view model to be updated.
        /// A derived class can override <see cref="OnUpdate"/> to perform update actions.
        /// </summary>
        public void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// Signals the view model that the language has changed.
        /// A derived class can override <see cref="OnLanguageChanged"/>
        /// to perform any needed updates.
        /// </summary>
        public void SignalLanguageChange()
        {
            OnLanguageChanged();
        }

        /// <summary>
        /// Signal the view model to save any state it requires.
        /// </summary>
        public void SignalSave()
        {
            OnSave();
        }

        /// <summary>
        /// Signal the view model that it is closing.
        /// </summary>
        public void SignalClosing()
        {
            OnClosing();
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when this view model becomes active.
        /// A derived class can override this method to perform initialization actions.
        /// The base implementation does nothing.
        /// </summary>
        protected virtual void OnActivated()
        {
        }

        /// <summary>
        /// Called when this view model becomes inactive.
        /// A derived class can override this method to perform cleanup actions.
        /// The base implementation does nothing.
        /// </summary>
        protected virtual void OnDeactivated()
        {
        }

        /// <summary>
        /// Called when an update to this view model is requested.
        /// A derived class can override this method to perform update actions.
        /// The base implementation does nothing.
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// Called after <see cref="ToggleActivation(ViewModelBase)"/> has toggled the activation state of the specified <see cref="ViewModelBase"/>.
        /// A derived class can override this method to perform update actions.
        /// The base implementation does nothing.
        /// </summary>
        /// <param name="vm">The <see cref="ViewModelBase"/> that was toggled.</param>
        protected virtual void OnActivationToggled(ViewModelBase vm)
        {
        }

        /// <summary>
        /// Gets the <see cref="Owner"/> property as the specified type
        /// </summary>
        /// <typeparam name="T">The type that derives from <see cref="ViewModelBase"/>.</typeparam>
        /// <returns>The owner as the specified type, or null if owner not set or can't be cast.</returns>
        protected T GetOwner<T>() where T : ViewModelBase
        {
            return Owner as T;
        }

        /// <summary>
        /// Called when <see cref="SignalLanguageChange"/> is called.
        /// Override in a derived class to perform any needed operations.
        /// The base implementation does nothing.
        /// </summary>
        protected virtual void OnLanguageChanged()
        {
        }

        /// <summary>
        /// Called when <see cref="SignalSave"/> is called.
        /// Override in a derived class to perform any state save required.
        /// The base implementation does nothing.
        /// </summary>
        protected virtual void OnSave()
        {
        }

        /// <summary>
        /// Called when the view model is closing, that is when <see cref="SignalClosing"/> is called.
        /// Override in a derived class to perform cleanup operations such as removing event handlers, etc.
        /// Always call the base method.
        /// </summary>
        protected virtual void OnClosing()
        {
            Commands.Clear();
        }
        #endregion

        /************************************************************************/

        #region IDisposable Members
        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// A derived class can override this method to handle disposal cleanup.
        /// </summary>
        /// <param name="disposing">true if called from the <see cref="Dispose()"/> method.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
        #endregion
    }
}