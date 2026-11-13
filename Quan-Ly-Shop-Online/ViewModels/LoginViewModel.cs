using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Quan_Ly_Shop_Online.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        // Fields
        private string _username;
        private string _password;

        // Properties
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set 
            {
                _password = value; 
                OnPropertyChanged(); 
            }
        }

        // Commands
        public ICommand LoginCommand { get; }

        // Constructors
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        // Methods
        private bool CanExecuteLogin(object obj)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) &&
                   Username.Length >= 3 && Password.Length >= 3;
        }

        private void ExecuteLogin(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
