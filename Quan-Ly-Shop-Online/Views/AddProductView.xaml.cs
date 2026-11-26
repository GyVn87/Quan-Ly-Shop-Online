using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Quan_Ly_Shop_Online.ViewModels;

namespace Quan_Ly_Shop_Online.Views;

public partial class AddProductView : Window {
    private const long MaximumImageSize = 5 * 1024 * 1024;
    private readonly AddProductViewModel _viewModel;

    public AddProductView(AddProductViewModel viewModel) {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
        _viewModel.CloseRequested += OnCloseRequested;
        _viewModel.SelectImagesRequested += OnSelectImagesRequested;
    }

    protected override void OnClosed(EventArgs e) {
        _viewModel.CloseRequested -= OnCloseRequested;
        _viewModel.SelectImagesRequested -= OnSelectImagesRequested;
        base.OnClosed(e);
    }

    private void OnCloseRequested(bool accepted) {
        DialogResult = accepted;
    }

    private void OnSelectImagesRequested(object? sender, EventArgs e) {
        var dialog = new OpenFileDialog {
            Title = "Chọn hình ảnh hàng hóa",
            Filter = "Hình ảnh|*.jpg;*.jpeg;*.png;*.webp|JPG|*.jpg;*.jpeg|PNG|*.png|WebP|*.webp",
            Multiselect = true
        };

        if (dialog.ShowDialog(this) != true) {
            return;
        }

        var validImages = dialog.FileNames
            .Where(path => new FileInfo(path).Length <= MaximumImageSize)
            .ToList();

        if (validImages.Count != dialog.FileNames.Length) {
            _viewModel.ShowImageError("Một số ảnh vượt quá giới hạn 5MB nên không được thêm.");
        }

        _viewModel.AddImages(validImages);
    }

    private void OnHeaderMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        if (e.LeftButton == MouseButtonState.Pressed) {
            DragMove();
        }
    }
}
