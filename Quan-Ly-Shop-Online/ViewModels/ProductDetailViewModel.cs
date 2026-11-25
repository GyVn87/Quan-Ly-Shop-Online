using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using Quan_Ly_Shop_Online.Models;
using Quan_Ly_Shop_Online.Repositories;

namespace Quan_Ly_Shop_Online.ViewModels;

public class ProductDetailViewModel : ViewModelBase {
    private readonly ProductRepository _productRepository;
    private string _productName;
    private string _category;
    private string _importPrice;
    private string _salePrice;
    private string _quantity;
    private string _errorMessage = string.Empty;

    public ProductDetailViewModel(ProductRepository productRepository, ProductModel product) {
        _productRepository = productRepository;

        ProductId = product.Id;
        _productName = product.ProductName;
        _category = product.Category;
        _importPrice = product.ImportPrice.ToString(CultureInfo.CurrentCulture);
        _salePrice = product.Price.ToString(CultureInfo.CurrentCulture);
        _quantity = product.Stock.ToString(CultureInfo.CurrentCulture);
        Categories = new ObservableCollection<string>(productRepository.GetAll()
            .Select(item => item.Category).Append(product.Category).Distinct().OrderBy(category => category));

        SaveCommand = new RelayCommand(Save, _ => true);
        CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(false), _ => true);
    }

    public event Action<bool>? CloseRequested;
    public string ProductId { get; }
    public ObservableCollection<string> Categories { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public string ProductName {
        get => _productName;
        set => SetValue(ref _productName, value);
    }

    public string Category {
        get => _category;
        set => SetValue(ref _category, value);
    }

    public string ImportPrice {
        get => _importPrice;
        set => SetValue(ref _importPrice, value);
    }

    public string SalePrice {
        get => _salePrice;
        set => SetValue(ref _salePrice, value);
    }

    public string Quantity {
        get => _quantity;
        set => SetValue(ref _quantity, value);
    }

    public string ErrorMessage {
        get => _errorMessage;
        private set {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    private void SetValue(ref string field, string value, [CallerMemberName] string? propertyName = null) {
        if (field == value)
            return;

        field = value;
        ErrorMessage = string.Empty;
        OnPropertyChanged(propertyName);
    }

    private void Save(object parameter) {
        if (string.IsNullOrWhiteSpace(ProductName) || ProductName.Trim().Length > 50) {
            ErrorMessage = "Tên hàng hóa phải có từ 1 đến 50 ký tự.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Category) || Category.Trim().Length > 50) {
            ErrorMessage = "Danh Mục phải có từ 1 đến 50 ký tự.";
            return;
        }

        if (!int.TryParse(ImportPrice, out var importPrice) || importPrice < 0) {
            ErrorMessage = "Giá nhập phải là số nguyên không âm và không vượt quá 2.147.483.647.";
            return;
        }

        if (!int.TryParse(SalePrice, out var salePrice) || salePrice < 0) {
            ErrorMessage = "Giá bán phải là số nguyên không âm và không vượt quá 2.147.483.647.";
            return;
        }

        if (!int.TryParse(Quantity, out var quantity) || quantity < 0) {
            ErrorMessage = "Tồn kho phải là số nguyên không âm và không vượt quá 2.147.483.647.";
            return;
        }

        ProductModel updatedProduct = new() {
            Id = ProductId,
            ProductName = ProductName.Trim(),
            Category = Category.Trim(),
            ImportPrice = importPrice,
            Price = salePrice,
            Stock = quantity
        };

        try {
            if (!_productRepository.Update(ProductId, updatedProduct)) {
                ErrorMessage = "Hàng hóa không còn tồn tại. Vui lòng đóng cửa sổ và kiểm tra lại danh sách.";
                return;
            }

            ErrorMessage = string.Empty;
            CloseRequested?.Invoke(true);
        }
        catch (SqlException) {
            ErrorMessage = "Không thể lưu hàng hóa. Vui lòng kiểm tra kết nối cơ sở dữ liệu và thử lại.";
        }
    }
}
