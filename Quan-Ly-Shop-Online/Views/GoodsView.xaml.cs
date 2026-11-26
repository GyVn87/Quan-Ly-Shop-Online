using System.Windows;
using System.Windows.Controls;
using Quan_Ly_Shop_Online.ViewModels;
using Quan_Ly_Shop_Online.Models;

namespace Quan_Ly_Shop_Online.Views;

public partial class GoodsView : UserControl {
    private GoodsViewModel? _viewModel;

    public GoodsView() {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) {
        if (DataContext is not GoodsViewModel viewModel || ReferenceEquals(_viewModel, viewModel)) {
            return;
        }

        DetachViewModel();
        _viewModel = viewModel;
        _viewModel.AddProductRequested += OnAddProductRequested;
        _viewModel.EditProductRequested += OnEditProductRequested;
        _viewModel.DeleteProductConfirmationRequested += OnDeleteProductConfirmationRequested;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e) {
        DetachViewModel();
    }

    private void OnAddProductRequested(object? sender, EventArgs e) {
        if (_viewModel is null) 
            return;

        var addViewModel = new AddProductViewModel(_viewModel.ProductRepository);
        var dialog = new AddProductView(addViewModel) {
            Owner = Window.GetWindow(this)
        };

        dialog.ShowDialog();
    }

    private void OnEditProductRequested(ProductModel product) {
        if (_viewModel is null)
            return;

        var detailViewModel = new ProductDetailViewModel(_viewModel.ProductRepository, product);
        var dialog = new ProductDetailView(detailViewModel) {
            Owner = Window.GetWindow(this)
        };

        dialog.ShowDialog();
    }

    private bool OnDeleteProductConfirmationRequested(ProductModel product) {
        return MessageBox.Show(Window.GetWindow(this),
            $"Bạn có chắc muốn xóa hàng hóa \"{product.ProductName}\" ({product.Id})?",
            "Xóa hàng hóa", MessageBoxButton.YesNo, MessageBoxImage.Warning,
            MessageBoxResult.No) == MessageBoxResult.Yes;
    }

    private void DetachViewModel() {
        if (_viewModel is null) {
            return;
        }

        _viewModel.AddProductRequested -= OnAddProductRequested;
        _viewModel.EditProductRequested -= OnEditProductRequested;
        _viewModel.DeleteProductConfirmationRequested -= OnDeleteProductConfirmationRequested;
        _viewModel = null;
    }
}
