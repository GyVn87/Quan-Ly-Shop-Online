using System;
using System.Collections.Generic;
using System.Text;

namespace Quan_Ly_Shop_Online.Events {
    public static class ShopEvents {
        // Events
        public static event Action? ProductsChanged;

        // Methods
        public static void NotifyProductsChanged() => ProductsChanged?.Invoke();
    }
}
