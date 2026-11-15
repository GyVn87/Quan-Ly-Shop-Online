using Quan_Ly_Shop_Online.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Quan_Ly_Shop_Online.Views {
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window {
        // Fields
        private LoginViewModel? _viewModel;

        // Constructors
        public LoginView() {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        // Methods
        private void OnLoaded(object sender, RoutedEventArgs e) {
            if (DataContext is not LoginViewModel viewModel || ReferenceEquals(_viewModel, viewModel)) {
                return;
            }

            DetachViewModel();
            _viewModel = viewModel;
            _viewModel.LoginSuccess += OnLoginSuccess;
        }


        private void OnUnloaded(object sender, RoutedEventArgs e) {
            DetachViewModel();
        }

        private void DetachViewModel() {
            if (_viewModel is null) {
                return;
            }

            _viewModel.LoginSuccess -= OnLoginSuccess; 
            _viewModel = null;
        }

        private void OnLoginSuccess(object? sender, EventArgs e) {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }
    }
}
