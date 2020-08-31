using Restless.Toolkit.Mvvm;
using System.Windows.Input;

namespace Restless.App.Toolkit
{
    public class ControlsViewModel : ViewModelBase
    {
        public ICommand DisplayUnloadTabCommand
        {
            get;
        }

        public ControlsViewModel()
        {
            DisplayName = "Tab Control";
            DisplayUnloadTabCommand = RelayCommand.Create((p) => MainWindowViewModel.Instance.DisplayUnloadTabCommand.Execute(null));
        }
    }
}
