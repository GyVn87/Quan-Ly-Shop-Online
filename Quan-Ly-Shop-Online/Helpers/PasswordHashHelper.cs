using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quan_Ly_Shop_Online.Helpers {
    public static class PasswordHashHelper {
        public static string HashPassword(string password) {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public static bool VerifyPassword(string enteredPassword, string storedPassword) {
            bool result = BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
            return result;
        }
    }
}
