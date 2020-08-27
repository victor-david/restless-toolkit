using Restless.Toolkit.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.App.Toolkit
{
    public class DemoViewModel : ViewModelBase
    {
        private string selectedName;
        public DemoViewModel()
        {
            DisplayName = "Demo";
            Names = new List<string>()
            { 
                "Jason","Betty","Sesmame",
                "Cliff","Billy","Susan",
                "Trok","Maria","Hearth",
                "Mabel","Ernest","Jacob",
                "Louisa","Menshem","Patrick",
            };
        }

        public string SelectedName
        {
            get => selectedName;
            set => SetProperty(ref selectedName, value);
        }
        public List<string> Names
        {
            get;
        }
    }
}
