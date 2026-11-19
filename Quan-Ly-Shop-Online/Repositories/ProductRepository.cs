using Microsoft.Data.SqlClient;
using Quan_Ly_Shop_Online.Data;
using Quan_Ly_Shop_Online.Events;
using Quan_Ly_Shop_Online.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Quan_Ly_Shop_Online.Repositories {
    public class ProductRepository {
        // Methods
        public List<ProductModel> GetAll() {
            List<ProductModel> result = new();
            const string query = """
                SELECT Id, ProductName, Category, Price, Stock
                FROM Products
                """;
        
            DataTable table = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in table.Rows) {
                result.Add(new ProductModel {
                    Id = row["Id"]?.ToString() ?? String.Empty,
                    ProductName = row["ProductName"]?.ToString() ?? String.Empty,
                    Category = row["Category"]?.ToString() ?? String.Empty,
                    Price = Convert.ToInt32(row["Price"]),
                    Stock = Convert.ToInt32(row["Stock"])
                });
            }

            return result;
        }

        public string Add(ProductModel addedProduct) {
            const string query = """
                INSERT INTO Products (ProductName, Category, Price, Stock)
                OUTPUT INSERTED.Id
                VALUES (@productName, @category, @price, @stock);
                """;
            SqlParameter[] queryParameters = [
                new("@name", addedProduct.ProductName),
                new("@category", addedProduct.Category),
                new("@price", addedProduct.Price),
                new("@stock", addedProduct.Stock)
            ];

            object? result = DatabaseHelper.ExecuteScalar(query, queryParameters);
            string newProductId = result?.ToString() ?? String.Empty;

            addedProduct.Id = newProductId;
            ShopEvents.NotifyProductsChanged();
            return newProductId;
        }

        public bool Update(string id, ProductModel updatedProduct) {
            const string query = """
                UPDATE Products
                SET ProductName = @productName,
                    Category = @category,
                    Price = @price,
                    Stock = @stock
                WHERE Id = @id
                """;
            SqlParameter[] queryParameters = [
                new("@name", updatedProduct.ProductName),
                new("@category", updatedProduct.Category),
                new("@price", updatedProduct.Price),
                new("@stock", updatedProduct.Stock)
            ];

            bool isSuccess = DatabaseHelper.ExecuteNonQuery(query, queryParameters) > 0;
            if (isSuccess)
                ShopEvents.NotifyProductsChanged();
            return isSuccess;
        }

        public bool Delete(string id) {
            const string query = """
                DELETE FROM Products
                WHERE Id = @id
                """;
            SqlParameter[] queryParameters = [
                new("@id", id)
            ];

            bool isSuccess = DatabaseHelper.ExecuteNonQuery(query, queryParameters) > 0;
            if (isSuccess)
                ShopEvents.NotifyProductsChanged();
            return isSuccess;
        }
    }
}
