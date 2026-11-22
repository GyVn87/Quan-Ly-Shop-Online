using System.Windows;
using System.Windows.Controls;
using Quan_Ly_Shop_Online.ViewModels;

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

    private void DetachViewModel() {
        if (_viewModel is null) {
            return;
        }

        _viewModel.AddProductRequested -= OnAddProductRequested;
        _viewModel = null;
    }
}
