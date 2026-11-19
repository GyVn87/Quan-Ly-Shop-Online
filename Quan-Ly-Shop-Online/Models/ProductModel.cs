namespace Quan_Ly_Shop_Online.Models;

public class ProductModel {
    public string Id { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Price { get; set; }
    public int Stock { get; set; }
}
