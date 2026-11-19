using System;
using System.Collections.Generic;
using System.Text;

namespace Quan_Ly_Shop_Online.Models {
    public class UserModel {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = String.Empty;
        public string PasswordHash { get; set; } = String.Empty;
        public string Role { get; set; } = "Staff";
    }
}
