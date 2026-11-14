using System.Collections.ObjectModel;
using System.Windows.Input;
using Quan_Ly_Shop_Online.Models;
using Quan_Ly_Shop_Online.Services;

namespace Quan_Ly_Shop_Online.ViewModels;

public class MainViewModel : ViewModelBase {
    private object _currentViewModel;

    public MainViewModel() {
        var catalog = new ProductCatalog();
        Dashboard = new DashboardViewModel(catalog.Products);
        Goods = new GoodsViewModel(catalog.Products);

        NavigationItems = [
            new("Trang chủ", "pack://application:,,,/Assets/home.png", NavigationTarget.Dashboard),
            new("Hàng hóa", "pack://application:,,,/Assets/product.png", NavigationTarget.Goods),
            new("Đơn hàng", "pack://application:,,,/Assets/bill.png", NavigationTarget.Orders, false),
            new("Nhân viên", "pack://application:,,,/Assets/team.png", NavigationTarget.Employees, false),
            new("Danh mục", "pack://application:,,,/Assets/menu.png", NavigationTarget.Categories, false)
        ];

        NavigateCommand = new RelayCommand(Navigate, CanNavigate);
        _currentViewModel = Dashboard;
        NavigationItems[0].IsSelected = true;
    }

    public DashboardViewModel Dashboard { get; }

    public GoodsViewModel Goods { get; }

    public ObservableCollection<NavigationItemViewModel> NavigationItems { get; }

    public ICommand NavigateCommand { get; }

    public object CurrentViewModel {
        get => _currentViewModel;
        private set {
            if (ReferenceEquals(_currentViewModel, value)) {
                return;
            }

            _currentViewModel = value;
            OnPropertyChanged();
        }
    }

    private bool CanNavigate(object parameter) => parameter is NavigationItemViewModel item && item.IsEnabled;

    private void Navigate(object parameter) {
        if (parameter is not NavigationItemViewModel selectedItem || !selectedItem.IsEnabled) {
            return;
        }

        CurrentViewModel = selectedItem.Target == NavigationTarget.Goods ? Goods : Dashboard;

        foreach (var item in NavigationItems) {
            item.IsSelected = item == selectedItem;
        }
    }
}
