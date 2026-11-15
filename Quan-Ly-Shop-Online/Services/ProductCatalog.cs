using System.Collections.ObjectModel;
using Quan_Ly_Shop_Online.Models;

namespace Quan_Ly_Shop_Online.Services;

public class ProductCatalog {
    public ObservableCollection<Product> Products { get; } = [
        new() { Code = "HH001", Name = "Áo thun basic", Category = "Thời trang", Price = 189000, Stock = 42 },
        new() { Code = "HH002", Name = "Quần jeans slim", Category = "Thời trang", Price = 349000, Stock = 8 },
        new() { Code = "HH003", Name = "Bình nước thể thao", Category = "Phụ kiện", Price = 129000, Stock = 0 },
        new() { Code = "HH004", Name = "Túi tote canvas", Category = "Phụ kiện", Price = 159000, Stock = 16 },
        new() { Code = "HH005", Name = "Sổ tay da", Category = "Văn phòng phẩm", Price = 99000, Stock = 7 },
        new() { Code = "HH006", Name = "Bút gel đen", Category = "Văn phòng phẩm", Price = 25000, Stock = 85 },
        new() { Code = "HH007", Name = "Mũ lưỡi trai", Category = "Thời trang", Price = 149000, Stock = 24 },
        new() { Code = "HH008", Name = "Ví da mini", Category = "Phụ kiện", Price = 219000, Stock = 5 }
    ];
}
