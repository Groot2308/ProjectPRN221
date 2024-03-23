using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
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
using GoldManagement.Models;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace GoldManagement
{
    /// <summary>
    /// Interaction logic for PriceFluctuations.xaml
    /// </summary>
    public partial class PriceFluctuations : UserControl
    {
        public readonly PROJECTPRN221Context _context;
        public PriceFluctuations()
        {
            InitializeComponent();
            _context = new PROJECTPRN221Context();
            LoadData();
        }
        public void LoadData()
        {
            listView.ItemsSource = _context.Products.ToList();
        }
        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement((ListView)sender, e.OriginalSource as DependencyObject) as ListViewItem;
            if (item == null)
                return;
            var selectedProduct = (Product)item.DataContext;
            ShowChartWindow(selectedProduct);
        }

        private void ShowChartWindow(Product product)
        {
            var chartWindow = new ChartWindow(product);
            chartWindow.Show();
        }
    }
}
