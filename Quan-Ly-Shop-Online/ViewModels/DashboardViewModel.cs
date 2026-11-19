using System.Collections.ObjectModel;
using Quan_Ly_Shop_Online.Events;
using Quan_Ly_Shop_Online.Models;
using Quan_Ly_Shop_Online.Repositories;

namespace Quan_Ly_Shop_Online.ViewModels;

public class DashboardViewModel : ViewModelBase {
    private readonly ProductRepository _productRepository;

    private int _totalProducts;
    private int _inStockProducts;
    private ObservableCollection<ProductModel> _lowStockProducts = [];
    private int _lowStockCount;
    private long _inventoryValue;

    public DashboardViewModel(ProductRepository productRepository) {
        _productRepository = productRepository;
        ShopEvents.ProductsChanged += RefreshSummary;
        RefreshSummary();
    }

    public int TotalProducts {
        get => _totalProducts;
        set {
            _totalProducts = value;
            OnPropertyChanged();
        }
    }

    public int InStockProducts {
        get => _inStockProducts;
        set {
            _inStockProducts = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ProductModel> LowStockProducts {
        get => _lowStockProducts;
        set {
            _lowStockProducts = value;
            OnPropertyChanged();
        }
    }

    public int LowStockCount {
        get => _lowStockCount;
        set {
            _lowStockCount = value;
            OnPropertyChanged();
        }
    }

    public long InventoryValue {
        get => _inventoryValue;
        set {
            _inventoryValue = value;
            OnPropertyChanged();
        }
    }

    private void RefreshSummary() {
        List<ProductModel> productList = _productRepository.GetAll();

        TotalProducts = productList.Count;
        InStockProducts = productList.Count(product => product.Stock > 0);
        LowStockProducts = new ObservableCollection<ProductModel>(productList.Where(product => product.Stock <= 10));
        LowStockCount = LowStockProducts.Count;
        InventoryValue = productList.Sum(product => product.Price * product.Stock);
    }
}
