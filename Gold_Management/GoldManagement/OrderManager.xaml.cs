using GoldManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoldManagement
{
    /// <summary>
    /// Interaction logic for OrderManager.xaml
    /// </summary>
    public partial class OrderManager : UserControl
    {
        public readonly PROJECTPRN221Context _context;
        public OrderManager()
        {
            InitializeComponent();
            _context = new PROJECTPRN221Context();
            listView.MouseDoubleClick += ListView_MouseDoubleClick;
            LoadData();
        }
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement((ListView)sender, e.OriginalSource as DependencyObject) as ListViewItem;
            if (item == null)
                return;
            var selectedOrder = (Order)item.DataContext;
            ShowOrderDetailsWindow(selectedOrder);
        }
        private void ShowOrderDetailsWindow(Order order)
        {
            var detailsWindow = new OrderDetailManager(order);
            detailsWindow.Show();
        }


        private void LoadData()
        {
            listView.ItemsSource = _context.Orders.Include(a => a.Status).ToList();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
