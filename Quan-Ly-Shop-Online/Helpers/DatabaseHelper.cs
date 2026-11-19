using System.Data;
using Microsoft.Data.SqlClient;

namespace Quan_Ly_Shop_Online.Data {
    public static class DatabaseHelper {
        private static readonly string connectionString = @"Data Source=localhost;Initial Catalog=ShopLocalDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        // Hàm truy vấn (Dùng cho SELECT) trả về kết quả là một DataTable
        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null) {
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    if (parameters != null) 
                        cmd.Parameters.AddRange(parameters);
                    try {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd)) {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                    catch (SqlException ex) {
                        System.Diagnostics.Debug.WriteLine($"[SQL ERROR] Câu lệnh: {query}");
                        System.Diagnostics.Debug.WriteLine($"[SQL ERROR] Chi tiết: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        // Hàm truy vấn trả về một giá trị duy nhất của hàng và cột đầu tiên
        public static object? ExecuteScalar(string query, SqlParameter[]? parameters = null) {
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    if (parameters != null) 
                        cmd.Parameters.AddRange(parameters);
                    try {
                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                    catch (SqlException ex) {
                        System.Diagnostics.Debug.WriteLine($"[SQL ERROR] Câu lệnh: {query}");
                        System.Diagnostics.Debug.WriteLine($"[SQL ERROR] Chi tiết: {ex.Message}");
                        throw;
                    } 
                }
            }
        }

        // Hàm thực thi (Dùng cho INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters) {
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    try {
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex) {
                        System.Diagnostics.Debug.WriteLine($"[SQL ERROR] Câu lệnh: {query}");
                        System.Diagnostics.Debug.WriteLine($"[SQL ERROR] Chi tiết: {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}

