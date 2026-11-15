using System.Collections.ObjectModel;
using Quan_Ly_Shop_Online.Models;

namespace Quan_Ly_Shop_Online.ViewModels;

public class DashboardViewModel : ViewModelBase {
    public DashboardViewModel(IEnumerable<Product> products) {
        var productList = products.ToList();

        TotalProducts = productList.Count;
        InStockProducts = productList.Count(product => product.Stock > 0);
        LowStockProducts = new ObservableCollection<Product>(productList.Where(product => product.Stock <= 10));
        InventoryValue = productList.Sum(product => product.Price * product.Stock);
    }

    public int TotalProducts { get; }

    public int InStockProducts { get; }

    public int LowStockCount => LowStockProducts.Count;

    public decimal InventoryValue { get; }

    public ObservableCollection<Product> LowStockProducts { get; }
}
