using Restless.Toolkit.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.App.Toolkit
{
    public class RadioViewModel : ViewModelBase
    {
        private int selectedValue1;
        private int selectedValue2;

        public RadioViewModel()
        {
            DisplayName = "Radio Buttons";
            SelectedValue1 = 1;
            SelectedValue2 = 2;
        }

        public int SelectedValue1
        {
            get => selectedValue1;
            set => SetProperty(ref selectedValue1, value);
        }

        public int SelectedValue2
        {
            get => selectedValue2;
            set => SetProperty(ref selectedValue2, value);
        }

    }
}