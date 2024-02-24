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
using System.Windows.Shapes;

namespace GoldManagement
{
    /// <summary>
    /// Interaction logic for OrderDetailManager.xaml
    /// </summary>
    public partial class OrderDetailManager : Window
    {
        
        public OrderDetailManager(Models.Order order)
        {
            InitializeComponent();
            PROJECTPRN221Context context = new PROJECTPRN221Context();
            var orderDetails = context.OrderDetails
                         .Include(od => od.Product)
                         .Where(od => od.OrderId == order.Id)
                         .ToList();
            listView.ItemsSource = orderDetails;
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
