using Restless.Toolkit.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Restless.App.Toolkit
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private
        private bool allowTabReorder;
        private bool keepContentOnTabSwitch;
        private bool isSeattleChecked;
        private bool isNewYorkChecked;
        private Thickness tabBorderThickness;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();

        private MainWindowViewModel()
        {
            DisplayName = "Restless Toolkit Demo";
            Pages = new ObservableCollection<ViewModelBase>();

            Commands.Add("ToggleAllowTabReorder", RelayCommand.Create(p => AllowTabReorder = !AllowTabReorder));
            Commands.Add("ToggleKeepContentOnTabSwitch", RelayCommand.Create(p => KeepContentOnTabSwitch = !KeepContentOnTabSwitch));
            Commands.Add("ToggleSeattle", p => IsSeattleChecked = !IsSeattleChecked);
            Commands.Add("ToggleNewYork", p => IsNewYorkChecked = !IsNewYorkChecked);
            DisplayUnloadTabCommand = RelayCommand.Create(p => Create<DemoViewModel>());
            ToggleBorderThicknessCommand = RelayCommand.Create(RunToggleBorderThicknessCommand);

            TabBorderThickness = new Thickness(1.0);
            AllowTabReorder = true;
            IsSeattleChecked = true;
            InitializePages();
        }
        #endregion

        /************************************************************************/

        #region Properties
        public bool AllowTabReorder
        {
            get => allowTabReorder;
            set => SetProperty(ref allowTabReorder, value);
        }

        public bool KeepContentOnTabSwitch
        {
            get => keepContentOnTabSwitch;
            set => SetProperty(ref keepContentOnTabSwitch, value);
        }

        public bool IsSeattleChecked
        {
            get => isSeattleChecked;
            set => SetProperty(ref isSeattleChecked, value);
        }

        public bool IsNewYorkChecked
        {
            get => isNewYorkChecked;
            set => SetProperty(ref isNewYorkChecked, value);
        }

        public ObservableCollection<ViewModelBase> Pages
        {
            get;
        }

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
        #endregion

        /************************************************************************/

        #region Private methods
        private void InitializePages()
        {
            Create<WelcomeViewModel>();
            Create<DataGridViewModel>();
            Create<ColorPickerViewModel>();
            Create<RadioViewModel>();
            Create<OtherViewModel>();
            Create<StyleViewModel>();
            Create<MvvmViewModel>();
            Create<DemoViewModel>();
            SetActivePage(Pages[0]);
        }

        private void Create<T>() where T : ViewModelBase, new()
        {
            ViewModelBase page = GetPageOfType<T>();

            if (page == null)
            {
                page = new T();
                Pages.Add(page);
            }
            SetActivePage(page);
        }

        private T GetPageOfType<T>() where T : ViewModelBase
        {
            IEnumerable<T> iterator = Pages.OfType<T>();
            return iterator.Count() > 0 ? iterator.First() : null;
        }

        private void SetActivePage(ViewModelBase page)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Pages);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(page);
            }
        }

        private void RunToggleBorderThicknessCommand(object parm)
        {
            if (TabBorderThickness.Left == 1.0)
            {
                TabBorderThickness = new Thickness(2.0);
            }
            else
            {
                TabBorderThickness = new Thickness(1.0);
            }
        }
        #endregion
    }
}