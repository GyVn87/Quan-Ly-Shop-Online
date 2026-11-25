using Quan_Ly_Shop_Online.Models;
using Quan_Ly_Shop_Online.Repositories;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Quan_Ly_Shop_Online.ViewModels;

public class AddProductViewModel : ViewModelBase {
    private readonly ProductRepository _productRepository;
    private const int MaximumImageCount = 3;
    private string _productName = string.Empty;
    private string _description = string.Empty;
    private string _importPrice = string.Empty;
    private string _salePrice = string.Empty;
    private string _quantity = "0";
    private string _category = string.Empty;
    private string _errorMessage = string.Empty;

    public AddProductViewModel(ProductRepository productRepository) {
        _productRepository = productRepository;

        Categories = [];
        ImagePaths = [];
        EmptyImageSlots = [0, 1, 2];

        UpdateCategories();

        SelectImagesCommand = new RelayCommand(RequestImageSelection, _ => ImagePaths.Count < MaximumImageCount);
        RemoveImageCommand = new RelayCommand(RemoveImage, parameter => parameter is string);
        IncreaseQuantityCommand = new RelayCommand(_ => ChangeQuantity(1), _ => true);
        DecreaseQuantityCommand = new RelayCommand(_ => ChangeQuantity(-1), _ => true);
        CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(false), _ => true);
        SubmitCommand = new RelayCommand(Submit, _ => true);
    }

    public event EventHandler? SelectImagesRequested;
    public event Action<bool>? CloseRequested;
    public ObservableCollection<string> ImagePaths { get; }
    public ObservableCollection<int> EmptyImageSlots { get; }
    public ObservableCollection<string> Categories { get; }
    public ICommand SelectImagesCommand { get; }
    public ICommand RemoveImageCommand { get; }
    public ICommand IncreaseQuantityCommand { get; }
    public ICommand DecreaseQuantityCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand SubmitCommand { get; }

    public string ProductName {
        get => _productName;
        set => SetValue(ref _productName, value);
    }

    public string Description {
        get => _description;
        set => SetValue(ref _description, value);
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
   
    public string Category {
        get => _category;
        set => SetValue(ref _category, value);
    }

    public string ErrorMessage {
        get => _errorMessage;
        private set {
            if (_errorMessage == value) {
                return;
            }

            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public void AddImages(IEnumerable<string> paths) {
        foreach (var path in paths.Where(path => !ImagePaths.Contains(path))) {
            if (ImagePaths.Count >= MaximumImageCount) {
                break;
            }

            ImagePaths.Add(path);
        }

        RefreshImageSlots();
    }

    public void ShowImageError(string message) {
        ErrorMessage = message;
    }

    private void SetValue(ref string field, string value, [CallerMemberName] string? propertyName = null) {
        if (field == value) {
            return;
        }

        field = value;
        ErrorMessage = string.Empty;
        OnPropertyChanged(propertyName);
    }

    private void RequestImageSelection(object parameter) {
        SelectImagesRequested?.Invoke(this, EventArgs.Empty);
    }

    private void RemoveImage(object parameter) {
        if (parameter is string path) {
            ImagePaths.Remove(path);
            RefreshImageSlots();
        }
    }

    private void RefreshImageSlots() {
        EmptyImageSlots.Clear();

        for (var index = ImagePaths.Count; index < MaximumImageCount; index++) {
            EmptyImageSlots.Add(index);
        }

        CommandManager.InvalidateRequerySuggested();
    }

    private void ChangeQuantity(int difference) {
        _ = int.TryParse(Quantity, out var currentQuantity);
        Quantity = Math.Max(0, currentQuantity + difference).ToString(CultureInfo.InvariantCulture);
    }

    private void Submit(object parameter) {
        if (string.IsNullOrWhiteSpace(ProductName)) {
            ErrorMessage = "Vui lòng nhập tên hàng hóa.";
            return;
        }

        if (!TryReadAmount(ImportPrice, out var purchasePrice) || purchasePrice < 0) {
            ErrorMessage = "Giá nhập phải là một số hợp lệ và không âm.";
            return;
        }

        if (!TryReadAmount(SalePrice, out var salePrice) || salePrice < 0) {
            ErrorMessage = "Giá bán phải là một số hợp lệ và không âm.";
            return;
        }

        if (!int.TryParse(Quantity, out var quantity) || quantity < 0) {
            ErrorMessage = "Số lượng phải là số nguyên không âm.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Category)) {
            ErrorMessage = "Xin vui lòng chọn Danh Mục phù hợp";
            return;
        }

        ErrorMessage = string.Empty;

        ProductModel newProduct = new() {
            ProductName = this.ProductName,
            ImportPrice = Convert.ToInt32(this.ImportPrice),
            Price = Convert.ToInt32(this.SalePrice),
            Category = this.Category,
            Stock = Convert.ToInt32(this.Quantity)
        };
        _productRepository.Add(newProduct);

        CloseRequested?.Invoke(true);
    }

    private static bool TryReadAmount(string value, out decimal amount) {
        return decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out amount)
            || decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out amount);
    }

    private void UpdateCategories() {
        List<ProductModel> latestProducts = _productRepository.GetAll();
        Categories.Clear();
        foreach (string category in latestProducts.Select(product => product.Category).Distinct().OrderBy(category => category))
            Categories.Add(category);
    }
}
