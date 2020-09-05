using Restless.Toolkit.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace Restless.App.Toolkit
{
    public class WelcomeViewModel : ViewModelBase
    {
        public ICommand DisplayUnloadTabCommand
        {
            get;
        }

        public ICommand ToggleBorderThicknessCommand
        {
            get;
        }

        public WelcomeViewModel()
        {
            DisplayName = "Toolkit";
            DisplayUnloadTabCommand = RelayCommand.Create((p) => MainWindowViewModel.Instance.DisplayUnloadTabCommand.Execute(null));
            ToggleBorderThicknessCommand = RelayCommand.Create((p) => MainWindowViewModel.Instance.ToggleBorderThicknessCommand.Execute(null));
        }
    }
}
