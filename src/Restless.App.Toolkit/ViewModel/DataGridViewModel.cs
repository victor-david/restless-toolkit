using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Restless.App.Toolkit
{
    public class DataGridViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Person> storage;
        private Person selectedItem;
        private IList selectedDataGridItems;
        private string actionMessage;

        public class Person
        {
            public int Id { get; }
            public string Name { get; }
            public string City { get; }
            public string Country { get; }
            public Person(int id, string name, string city, string country)
            {
                Id = id;
                Name = name;
                City = city;
                Country = country;
            }
        }

        public DataGridColumnCollection Columns { get; }
        public MenuItemCollection MenuItems { get; }
        public ListCollectionView ListView { get; }
        public ICommand HeaderCommand { get; }
        public string ActionMessage
        {
            get => actionMessage;
            private set => SetProperty(ref actionMessage, value);
        }

        public Person SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        /// <summary>
        /// Sets the selected data grid items. This property is bound to the view.
        /// </summary>
        public IList SelectedDataGridItems
        {
            get => selectedDataGridItems;
            set
            {
                SetProperty(ref selectedDataGridItems, value);
                ActionMessage = $"{selectedDataGridItems.Count} items selected";
            }
        }

        public DataGridViewModel()
        {
            DisplayName = "Data Grid";

            Columns = new DataGridColumnCollection();
            Columns.Create("Id", nameof(Person.Id)).MakeFixedWidth(42);

            Columns.Create("Name", nameof(Person.Name)).MakeInitialSortAscending();
            Columns.Create("City", nameof(Person.City));
            Columns.Create("Country", nameof(Person.Country));

            MenuItems = new MenuItemCollection();
            MenuItems.AddItem("Open bio for this person", RelayCommand.Create(p => ActionMessage = "This command does nothing"));
            MenuItems.AddItem("Send this person a coffee", RelayCommand.Create(RunThankYouCommand));
            MenuItems.AddSeparator();
            MenuItems.AddItem("Give this person a cookie", RelayCommand.Create(RunThankYouCommand));

            HeaderCommand = RelayCommand.Create(RunHeaderCommand);

            storage = new ObservableCollection<Person>()
            {
                new Person(1, "David Gorman", "Havana", "Cuba"),
                new Person(2, "Maggie Porter","London","England"),
                new Person(3, "Abigail Nonce","Pittsburg","United States"),
                new Person(4, "Zues Alert Bot","Everywhere","World"),
                new Person(5, "Tomas Ovalle","Mexico City","Mexico"),
                new Person(6, "Pearce Bracket","Ontario","Canada"),
                new Person(7, "Lewis Kalidont","Tokyo","Japan"),
                new Person(8, "Feather Boss","Manheim","Germany"),
                new Person(9, "Suzan Kal Balai","Inquest","Pakistan"),
            };

            ListView = new ListCollectionView(storage);
        }

        private void RunThankYouCommand(object parm)
        {
            MessageWindow.ShowOkay("Thank you");
        }

        private void RunHeaderCommand(object parm)
        {
            if (parm is DataGridColumnHeader header)
            {
                ActionMessage = $"Header clicked: {header.Column.SortMemberPath}";
            }
        }
    }
}