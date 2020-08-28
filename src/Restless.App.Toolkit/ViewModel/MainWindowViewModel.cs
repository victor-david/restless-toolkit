using Restless.Toolkit.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Restless.App.Toolkit
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private
        private bool keepContentOnTabSwitch;
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
            Commands.Add("DisplayWelcome", RelayCommand.Create((p) => Create<WelcomeViewModel>()));
            Commands.Add("DisplayMvvm", RelayCommand.Create((p) => Create<MvvmViewModel>()));
            Commands.Add("DisplayControls", RelayCommand.Create((p) => Create<ControlsViewModel>()));
            Commands.Add("DisplayDemo", RelayCommand.Create((p) => Create<DemoViewModel>()));
            Commands.Add("ToggleKeepContentOnTabSwitch", RelayCommand.Create((p) => KeepContentOnTabSwitch = !KeepContentOnTabSwitch));
            InitializePages();
        }
        #endregion

        /************************************************************************/

        #region Properties
        public bool KeepContentOnTabSwitch
        {
            get => keepContentOnTabSwitch;
            set => SetProperty(ref keepContentOnTabSwitch, value);
        }

        public ObservableCollection<ViewModelBase> Pages
        {
            get;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void InitializePages()
        {
            Create<WelcomeViewModel>();
            Create<MvvmViewModel>();
            Create<ControlsViewModel>();
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
            var iterator = Pages.OfType<T>();
            if (iterator.Count() > 0)
            {
                return iterator.First();
            }
            return null;
        }

        private void SetActivePage(ViewModelBase page)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Pages);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(page);
            }
        }
        #endregion
    }
}