using Restless.Toolkit.Mvvm;
using System.Collections.Generic;

namespace Restless.App.Toolkit
{
    public class StyleViewModel : ViewModelBase
    {
        private bool isEnabled;

        public bool IsEnabled
        {
            get => isEnabled;
            private set => SetProperty(ref isEnabled, value);
        }

        public List<string> Names { get; }
        public string SelectedName { get; set; }

        public StyleViewModel()
        {
            DisplayName = "Style";
            IsEnabled = true;
            Commands.Add("ToggleEnabled", p => IsEnabled = !IsEnabled);
            Names = new List<string>()
            {
                "Peter", "Marsha", "Hanold", "Olivia", "Walter", "Broyles"
            };

            SelectedName = "Walter";
        }
    }
}