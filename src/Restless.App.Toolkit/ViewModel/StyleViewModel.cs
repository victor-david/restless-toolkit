using Restless.Toolkit.Mvvm;

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
        public StyleViewModel()
        {
            DisplayName = "Style";
            IsEnabled = true;
            Commands.Add("ToggleEnabled", p => IsEnabled = !IsEnabled);
        }
    }
}