using System.Windows;
using Quan_Ly_Shop_Online.ViewModels;

namespace Quan_Ly_Shop_Online.Views;

public partial class ProductDetailView : Window {
    private readonly ProductDetailViewModel _viewModel;

    public ProductDetailView(ProductDetailViewModel viewModel) {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
        _viewModel.CloseRequested += OnCloseRequested;
    }

    protected override void OnClosed(EventArgs e) {
        _viewModel.CloseRequested -= OnCloseRequested;
        base.OnClosed(e);
    }

    private void OnCloseRequested(bool accepted) {
        DialogResult = accepted;
    }
}
