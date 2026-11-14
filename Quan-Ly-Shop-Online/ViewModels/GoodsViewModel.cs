using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Quan_Ly_Shop_Online.Models;

namespace Quan_Ly_Shop_Online.ViewModels;

public class GoodsViewModel : ViewModelBase {
    private const string AllCategories = "Tất cả danh mục";
    private string _searchText = string.Empty;
    private string _selectedCategory = AllCategories;

    public GoodsViewModel(ObservableCollection<Product> products) {
        Categories = new ObservableCollection<string>(
            [AllCategories, .. products.Select(product => product.Category).Distinct().OrderBy(category => category)]);

        FilteredProducts = CollectionViewSource.GetDefaultView(products);
        FilteredProducts.Filter = MatchesFilter;
        TotalProducts = products.Count;
        LowStockCount = products.Count(product => product.Stock <= 10);
    }

    public ObservableCollection<string> Categories { get; }

    public ICollectionView FilteredProducts { get; }

    public int TotalProducts { get; }

    public int LowStockCount { get; }

    public int CategoryCount => Categories.Count - 1;

    public int ResultCount => FilteredProducts.Cast<object>().Count();

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
        if (item is not Product product) {
            return false;
        }

        var matchesSearch = string.IsNullOrWhiteSpace(SearchText)
            || product.Code.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            || product.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        var matchesCategory = SelectedCategory == AllCategories || product.Category == SelectedCategory;

        return matchesSearch && matchesCategory;
    }

    private void RefreshProducts() {
        FilteredProducts.Refresh();
        OnPropertyChanged(nameof(ResultCount));
    }
}
