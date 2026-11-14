namespace Quan_Ly_Shop_Online.Models;

public class Product {
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public int Stock { get; init; }

    public string Status => Stock switch {
        <= 0 => "Hết hàng",
        <= 10 => "Sắp hết",
        _ => "Còn hàng"
    };
}
