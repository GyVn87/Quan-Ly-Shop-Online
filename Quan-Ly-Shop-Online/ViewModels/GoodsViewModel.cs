using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Data;
using Quan_Ly_Shop_Online.Models;
using Quan_Ly_Shop_Online.Repositories;
using Quan_Ly_Shop_Online.Events;

namespace Quan_Ly_Shop_Online.ViewModels;

public class GoodsViewModel : ViewModelBase {
    private readonly ProductRepository _productRepository;
    private const string AllCategories = "Tất cả danh mục";
    private readonly ObservableCollection<ProductModel> _products = [];

    private int _totalProducts;
    private int _lowStockCount;
    private string _searchText = string.Empty;
    private string _selectedCategory = AllCategories;

    public GoodsViewModel(ProductRepository productRepository) {
        _productRepository = productRepository;
        ShopEvents.ProductsChanged += ReloadFromRepository;

        AddProductCommand = new RelayCommand(OpenAddProduct, _ => true);
        Categories = [];
        FilteredProducts = CollectionViewSource.GetDefaultView(_products);
        FilteredProducts.Filter = MatchesFilter;

        ReloadFromRepository();
    }

    public ProductRepository ProductRepository => _productRepository;
    public event EventHandler? AddProductRequested;
    public ObservableCollection<string> Categories { get; }
    public ICollectionView FilteredProducts { get; }
    public int CategoryCount => Categories.Count - 1;
    public int ResultCount => FilteredProducts.Cast<object>().Count();
    public ICommand AddProductCommand { get; }

    public int TotalProducts {
        get => _totalProducts;
        set {
            _totalProducts = value;
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

    public string SearchText {
        get => _searchText;
        set {
            if (_searchText == value) {
                return;
            }

            _searchText = value;
            OnPropertyChanged();
            RefreshProducts();
        }
    }

    public string SelectedCategory {
        get => _selectedCategory;
        set {
            if (_selectedCategory == value) {
                return;
            }

            _selectedCategory = value;
            OnPropertyChanged();
            RefreshProducts();
        }
    }

    private bool MatchesFilter(object item) {
        if (item is not ProductModel product) 
            return false;

        var matchesSearch = string.IsNullOrWhiteSpace(SearchText)
            || product.Id.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            || product.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        var matchesCategory = SelectedCategory == AllCategories || product.Category == SelectedCategory;

        return matchesSearch && matchesCategory;
    }

    private void ReloadFromRepository() {
        List<ProductModel> latestProducts = _productRepository.GetAll();

        _products.Clear();
        foreach (var product in latestProducts)
            _products.Add(product);

        TotalProducts = latestProducts.Count;
        LowStockCount = latestProducts.Count(product => product.Stock <= 10);

        string previousSelectedCategory = SelectedCategory;
        Categories.Clear();
        Categories.Add(AllCategories);
        foreach (string category in latestProducts.Select(product => product.Category).Distinct().OrderBy(category => category))
            Categories.Add(category);
        OnPropertyChanged(nameof(CategoryCount));
        SelectedCategory = Categories.Contains(previousSelectedCategory) ? previousSelectedCategory : AllCategories;

        RefreshProducts();
    }

    private void OpenAddProduct(object parameter) {
        AddProductRequested?.Invoke(this, EventArgs.Empty);
    }

    private void RefreshProducts() {
        FilteredProducts.Refresh();
        OnPropertyChanged(nameof(ResultCount));
    }
}
