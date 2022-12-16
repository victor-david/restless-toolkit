using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a bindable collection of menu items with convienence methods for adding items.
    /// </summary>
    /// <remarks>
    /// See also <see cref="MenuItemExtensions"/>
    /// </remarks>
    public class MenuItemCollection : ObservableCollection<Control>
    {
        /// <summary>
        /// Adds a new menu item to the collection.
        /// </summary>
        /// <param name="header">The item header, that which is displayed in the UI</param>
        /// <param name="command">The command associated with this item.</param>
        /// <returns>The newly added item.</returns>
        public MenuItem AddItem(string header, ICommand command)
        {
            MenuItem item = CreateItem(header, command);
            Add(item);
            return item;
        }

        /// <summary>
        /// Inserts a new menu item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the item</param>
        /// <param name="header">The item header, that which is displayed in the UI</param>
        /// <param name="command">The command associated with this item.</param>
        /// <returns>The newly inserted item.</returns>
        public MenuItem InsertItem(int index, string header, ICommand command)
        {
            MenuItem item = CreateItem(header, command);
            Insert(index, item);
            return item;
        }

        /// <summary>
        /// Adds a menu separator to the collection.
        /// </summary>
        public void AddSeparator()
        {
            Add(new Separator());
        }

        /// <summary>
        /// Inserts a menu separator at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the separator.</param>
        public void InsertSeparator(int index)
        {
            Insert(index, new Separator());
        }

        /// <summary>
        /// Called when items are being cleared.
        /// </summary>
        protected override void ClearItems()
        {
            foreach (MenuItem item in this.OfType<MenuItem>())
            {
                item.Command = null;
            }
            base.ClearItems();
        }

        private MenuItem CreateItem(string header, ICommand command)
        {
            return new MenuItem
            {
                Header = header,
                Command = command,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center
            };
        }
    }
}