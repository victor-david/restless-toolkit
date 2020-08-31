using Restless.Toolkit.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace Restless.App.Toolkit
{
    public class TabControlsViewModel : ViewModelBase
    {
        private Thickness tabBorderThickness;

        public ICommand DisplayUnloadTabCommand
        {
            get;
        }

        public ICommand ToggleBorderThicknessCommand
        {
            get;
        }

        public Thickness TabBorderThickness
        {
            get => tabBorderThickness;
            private set => SetProperty(ref tabBorderThickness, value);
        }

        public TabControlsViewModel()
        {
            DisplayName = "Tab Control";
            DisplayUnloadTabCommand = RelayCommand.Create((p) => MainWindowViewModel.Instance.DisplayUnloadTabCommand.Execute(null));
            ToggleBorderThicknessCommand = RelayCommand.Create(RunToggleBorderThicknessCommand);
            TabBorderThickness = new Thickness(1.0);
        }

        private void RunToggleBorderThicknessCommand(object parm)
        {
            if (TabBorderThickness.Left == 1.0)
            {
                TabBorderThickness = new Thickness(3.0);
            }
            else
            {
                TabBorderThickness = new Thickness(1.0);
            }
        }
    }
}
