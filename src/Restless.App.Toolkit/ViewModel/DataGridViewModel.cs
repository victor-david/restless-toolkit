using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
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
            public bool IsVerified { get; }
            public Person(int id, string name, string city, string country, bool verified)
            {
                Id = id;
                Name = name;
                City = city;
                Country = country;
                IsVerified = verified;
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

        public IList SelectedDataGridItems
        {
            get => selectedDataGridItems;
            set
            {
                SetProperty(ref selectedDataGridItems, value);
                ActionMessage = $"{selectedDataGridItems?.Count ?? 0} items selected";
            }
        }

        public DataGridViewModel()
        {
            DisplayName = "Data Grid";

            Columns = new DataGridColumnCollection();
            Columns.Create("Id", nameof(Person.Id))
                .MakeCentered()
                .MakeFixedWidth(48);

            Columns.CreateCheckBox("V", nameof(Person.IsVerified), BindingMode.OneWay, Application.Current.TryFindResource("DisabledCheckBoxStyle") as Style)
                .MakeCentered()
                .MakeFixedWidth(48)
                .SetSelectorName("Verified")
                .CanUserSort = false;

            Columns.Create("Name", nameof(Person.Name)).MakeInitialSortAscending();
            Columns.Create("City", nameof(Person.City));
            Columns.Create("Country", nameof(Person.Country), true);

            MenuItems = new MenuItemCollection();
            MenuItems.AddItem("Open bio for this person", RelayCommand.Create(p => ActionMessage = "Bio opened in some alternate universe"));
            MenuItems.AddItem("Send this person a coffee", RelayCommand.Create(p => ActionMessage = "Thanks for the coffee"));
            MenuItems.AddSeparator();
            MenuItems.AddItem("Give this person a cookie", RelayCommand.Create(p => ActionMessage = "Yum. Cookie"));

            HeaderCommand = RelayCommand.Create(RunHeaderCommand);

            storage = new ObservableCollection<Person>()
            {
                new Person(1, "David Gorman", "Havana", "Cuba", true),
                new Person(2, "Maggie Porter", "London", "England", true),
                new Person(3, "Abigail Nonce", "Pittsburg", "United States", true),
                new Person(4, "Hues Alert Bot", "Everywhere", null, false),
                new Person(5, "Tomas Ovalle", "Mexico City", "Mexico", true),
                new Person(6, "Pearce Bracket", "Ontario", "Canada", true),
                new Person(7, "Lewis Kalidont", "Tokyo", "Japan", true),
                new Person(8, "Feather Boss", "Manheim", "Germany", true),
                new Person(9, "Suzan Kal Balai", "Inquest", "Pakistan", false),
            };

            ListView = new ListCollectionView(storage);
            Columns.ColumnStateChanged += ColumnsColumnStateChanged;
        }

        private void ColumnsColumnStateChanged(object sender, ColumnStateChangedEventArgs e)
        {
            ActionMessage = $"Column state changed: {e.State}";
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