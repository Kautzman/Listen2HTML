using System.Collections.ObjectModel;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Controls;
using System.Windows;

namespace Listen2Html.ViewModel
{
    class Listen2HtmlVM : BindableBase
    {
        private ObservableCollection<Listener> _listeners = new ObservableCollection<Listener>();
        public ObservableCollection<Listener> Listeners
        {
            get { return _listeners; }
            set { SetProperty(ref _listeners, value); }
        }

        private string _newSmtpServer = "smtp.gmail.com";
        public string NewSmtpServer
        {
            get { return _newSmtpServer; }
            set { SetProperty(ref _newSmtpServer, value); }
        }

        private string _newAuthAccount;
        public string NewAuthAccount
        {
            get { return _newAuthAccount; }
            set { SetProperty(ref _newAuthAccount, value); }
        }

        private string _newAuthPassword;
        public string NewAuthPassword
        {
            get { return _newAuthPassword; }
            set { SetProperty(ref _newAuthPassword, value); }
        }

        private string _newToAddress;
        public string NewToAddress
        {
            get { return _newToAddress; }
            set { SetProperty(ref _newToAddress, value); }
        }

        private string _newListenerUrl;
        public string NewListenerUrl
        {
            get { return _newListenerUrl; }
            set { SetProperty(ref _newListenerUrl, value); }
        }

        public DelegateCommand<object> UpdateUserInfoCommand { get; private set; }
        public DelegateCommand SendTestEmailCommand { get; private set; }
        public DelegateCommand NewListenerCommand { get; private set; }

        public Listen2HtmlVM()
        {
            UpdateUserInfoCommand = new DelegateCommand<object>(UpdateUserInfo);
            SendTestEmailCommand = new DelegateCommand(SendTestEmail);
            NewListenerCommand = new DelegateCommand(AddNewListener);
        }

        //TODO: Save this between app loads
        //TODO: Graphical indicator when New Data doesn't match existing data
        private void UpdateUserInfo(object thisPwBox)
        {
            PasswordBox pwBox = (thisPwBox as PasswordBox);

            UserInfo.EmailServer = NewSmtpServer;
            UserInfo.ToAddress = NewToAddress;
            UserInfo.UserName = NewAuthAccount;
            UserInfo.Password = pwBox.Password;

            MessageBox.Show("Credentials Saved!");
        }

        private void SendTestEmail()
        {
            UserInfo.SendTestEmail();
        }

        //TODO: Save Listeners data between app loads
        private void AddNewListener()
        {
            Listeners.Add(new Listener(NewListenerUrl));

            NewListenerUrl = string.Empty;
        }
    }
}
