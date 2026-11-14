using Quan_Ly_Shop_Online.Models;

namespace Quan_Ly_Shop_Online.ViewModels;

public class NavigationItemViewModel : ViewModelBase {
    private bool _isSelected;

    public NavigationItemViewModel(string title, string icon, NavigationTarget target, bool isEnabled = true) {
        Title = title;
        Icon = icon;
        Target = target;
        IsEnabled = isEnabled;
    }

    public string Title { get; }

    public string Icon { get; }

    public NavigationTarget Target { get; }

    public bool IsEnabled { get; }

    public bool IsSelected {
        get => _isSelected;
        set {
            if (_isSelected == value) {
                return;
            }

            _isSelected = value;
            OnPropertyChanged();
        }
    }
}
