using Restless.Toolkit.Controls;
using Restless.Toolkit.Mvvm;
using System.Text;

namespace Restless.App.Toolkit
{
    public class OtherViewModel : ViewModelBase
    {
        private string messageWindowResult;

        public string MessageWindowResult
        {
            get => messageWindowResult;
            private set => SetProperty(ref messageWindowResult, value);
        }

        public OtherViewModel()
        {
            DisplayName = "Other";
            Commands.Add("OpenYesNo", RelayCommand.Create(RunOpenYesNoCommand));
            Commands.Add("OpenContinueCancel", RelayCommand.Create(RunOpenContinueCancelCommand));
            Commands.Add("OpenOkay", RelayCommand.Create(RunOpenOkayCommand));
            Commands.Add("OpenError", RelayCommand.Create(RunOpenErrorCommand));
            MessageWindowResult = "(none yet)";
        }

        private void RunOpenYesNoCommand(object parm)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("This operation will turn all pie into high quality cake. ");
            builder.AppendLine("Before you begin, you must wash the dishes and feed the cats.");
            builder.AppendLine();
            builder.Append("Do you want to continue?");
            MessageWindowResult = "(waiting)";
            MessageWindowResult = BooleanToString(MessageWindow.ShowYesNo(builder.ToString()));
        }

        private void RunOpenContinueCancelCommand(object parm)
        {
            string message = "Getting ready to download stock prices for the last six months";
            MessageWindowResult = "(waiting)";
            MessageWindowResult = BooleanToString(MessageWindow.ShowContinueCancel(message));
        }

        private void RunOpenOkayCommand(object parm)
        {
            string message = "The operation completed successfully";
            MessageWindowResult = "(waiting)";
            MessageWindowResult = BooleanToString(MessageWindow.ShowOkay(message));

        }

        private void RunOpenErrorCommand(object parm)
        {
            string message = "The operation failed due to a network error. Please check your connection and try again.";
            MessageWindowResult = "(waiting)";
            MessageWindowResult = BooleanToString(MessageWindow.ShowError(message));
        }

        private string BooleanToString(bool value)
        {
            return value ? "YES" : "NO";
        }
    }
}