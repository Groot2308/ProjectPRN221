using GoldManagement.Models;
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
    /// Interaction logic for MyOrder.xaml
    /// </summary>
    public partial class MyOrder : Window
    {
        public readonly PROJECTPRN221Context _context;
        public MyOrder()
        {
            InitializeComponent();
            _context = new PROJECTPRN221Context();
            LoadData();
        }

        private void LoadData()
        {
            listView.ItemsSource = _context.Orders.Where(o => o.UserId == Session.Account.Id)
                .OrderByDescending(o => o.OrderDate)   
                .ToList();
        }
        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
