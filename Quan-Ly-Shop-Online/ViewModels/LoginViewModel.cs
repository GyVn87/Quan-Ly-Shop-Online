using Microsoft.Data.SqlClient;
using System.Data;
using Quan_Ly_Shop_Online.Data;
using System.Windows.Input;
using System.Windows;
using Quan_Ly_Shop_Online.Helpers;
using System.Transactions;

namespace Quan_Ly_Shop_Online.ViewModels {
    public class LoginViewModel : ViewModelBase {
        // Fields
        private string _username = String.Empty;
        private string _password = String.Empty;
        private string _errorMessage = String.Empty;
        public event EventHandler? LoginSuccess;

        // Properties
        public string Username {
            get => _username;
            set {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password {
            get => _password;
            set {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage {
            get => _errorMessage;
            set {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand LoginCommand { get; }

        // Constructors
        public LoginViewModel() {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            AddAdminAccount();
        }

        // Methods
        private bool CanExecuteLogin(object obj) {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) &&
                   Username.Length >= 3 && Password.Length >= 3;
        }

        private void ExecuteLogin(object obj) {
            const string query = "SELECT PasswordHash " +
                                 "FROM Users " +
                                 "WHERE Username COLLATE Latin1_General_CS_AS = @username";
            SqlParameter[] queryParameters = {
                new("@username", Username)
            };

            object? result = DatabaseHelper.ExecuteScalar(query, queryParameters);

            if (result is null) {
                FailToLogin();
                return;
            }

            string storedPassword = result.ToString() ?? string.Empty;
            if (!PasswordHashHelper.VerifyPassword(Password, storedPassword)) {
                FailToLogin();
                return;
            }

            LoginSuccess?.Invoke(this, EventArgs.Empty);
        }

        private void FailToLogin() {
            ErrorMessage = "Tên đăng nhập hoặc mật khẩu có thể đã sai, xin vui lòng đăng nhập lại!";
        }

        // Hàm tạm thời
        private static void AddAdminAccount() {
            string adminUsername = "Admin";
            string rawPassword = "123"; // chỉ dùng cho việc test
            string hashedPassword = PasswordHashHelper.HashPassword(rawPassword);

            string addQuery = "INSERT INTO Users (Username, PasswordHash, Role) " +
                              "VALUES (@username, @password, @role)";
            SqlParameter[] addQueryParameters = {
                new("@username", adminUsername),
                new("@password", hashedPassword),
                new("@role", "Admin")
            };

            string deleteQuery = "DELETE FROM Users " +
                                 "WHERE Username = @username";
            SqlParameter[] deleteQueryParameters = {
                new("@username", adminUsername),
            };

            DatabaseHelper.ExecuteNonQuery(deleteQuery, deleteQueryParameters);
            DatabaseHelper.ExecuteNonQuery(addQuery, addQueryParameters);
        }
    }
}
