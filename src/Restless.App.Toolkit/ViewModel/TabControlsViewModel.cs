using Restless.Toolkit.Mvvm;
using System.Windows.Input;

namespace Restless.App.Toolkit
{
    public class TabControlsViewModel : ViewModelBase
    {
        public ICommand DisplayUnloadTabCommand
        {
            get;
        }

        public TabControlsViewModel()
        {
            DisplayName = "Tab Control";
            DisplayUnloadTabCommand = RelayCommand.Create((p) => MainWindowViewModel.Instance.DisplayUnloadTabCommand.Execute(null));
        }
    }
}
