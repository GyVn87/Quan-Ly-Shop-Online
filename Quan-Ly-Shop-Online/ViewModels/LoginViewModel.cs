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
            string query = "SELECT PasswordHash " +
                           "FROM Users " +
                           "WHERE Username = @username";
            SqlParameter[] queryParameters = {
                new("@username", Username)
            };
            DataTable resultTable = DatabaseHelper.ExecuteQuery(query, queryParameters);

            if (resultTable.Rows.Count == 0) { LoginFailed(); return; }
            string storedPassword = resultTable.Rows[0]["PasswordHash"].ToString() ?? string.Empty;
            if (!PasswordHashHelper.VerifyPassword(Password, storedPassword)) { LoginFailed(); return; }
            MessageBox.Show("Đăng nhập thành công!"); // chỉ dùng để test, do việc gọi MessageBox là vi phạm MVVM
        }

        private void LoginFailed() {
            MessageBox.Show("Đăng nhập thất bại!"); // Đăng nhập lỗi
            // chỉ dùng để test, do việc gọi MessageBox là vi phạm MVVM
        }

        // Hàm tạm thời
        private void AddAdminAccount() {
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
