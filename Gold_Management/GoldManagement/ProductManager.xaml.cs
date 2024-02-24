using GoldManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
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
    /// Interaction logic for ProductManager.xaml
    /// </summary>
    public partial class ProductManager : UserControl
    {
        public readonly PROJECTPRN221Context _context;
        public ProductManager()
        {
            InitializeComponent();
            _context = new PROJECTPRN221Context();
            LoadData();
        }

        public Product GetData()
        {
            Product product = new Product();
            product.Id = searchById.Text;
            product.Name = searchByName.Text;
            product.Quantity =double.Parse(searchByQuantity.Text);
            product.PurchasePrice = double.Parse(searchByPurchasePrice.Text);
            product.RetailPrice = double.Parse(searchByRetailPrice.Text);
            product.Stock = int.Parse(searchByStock.Text);
            product.CategoryId = Int32.Parse(searchByCategory.SelectedValue.ToString());
            return product;
        }
        public void LoadData()
        {
            listView.ItemsSource = _context.Products.ToList();
            searchByCategory.ItemsSource = _context.Categories.ToList();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            string id = searchById.Text;
            string name = searchByName.Text;
            double Quantity = string.IsNullOrEmpty(searchByQuantity.Text) ? 0 : double.Parse(searchByQuantity.Text);
            double PurchasePrice = string.IsNullOrEmpty(searchByPurchasePrice.Text) ? 0 : double.Parse(searchByPurchasePrice.Text);
            double RetailPrice = string.IsNullOrEmpty(searchByRetailPrice.Text) ? 0 : double.Parse(searchByRetailPrice.Text);
            int Stock = string.IsNullOrEmpty(searchByStock.Text) ? 0 : int.Parse(searchByStock.Text);
            int CategoryId = string.IsNullOrEmpty(searchByCategory.SelectedValue?.ToString()) ? 0 : Int32.Parse(searchByCategory.SelectedValue.ToString());
            Search(id, name, Quantity, PurchasePrice, RetailPrice, Stock, CategoryId);

        }
        public void Search(string id, string name, double Quantity, double PurchasePrice, double RetailPrice, int Stock, int CategoryId)
        {
            var products = _context.Products.ToList();

            products = products.Where(c =>
                (string.IsNullOrEmpty(id) || c.Id.ToUpper().Contains(id.ToUpper())) &&
                (string.IsNullOrEmpty(name) || c.Name.Contains(name)) &&
                (Quantity == 0 || c.Quantity == Quantity) &&
                (PurchasePrice == 0 || c.PurchasePrice == PurchasePrice) &&
                (RetailPrice == 0 || c.RetailPrice == RetailPrice) &&
                (Stock == 0 || c.Stock == Stock) &&
                (CategoryId == 0 || c.CategoryId == CategoryId)
            ).ToList();
            listView.ItemsSource = products;
        }


        private void Button_OpenCreate(object sender, RoutedEventArgs e)
        {
            Product product = GetData();
            if (product != null)
            {
                var oldInfor = _context.Products.FirstOrDefault(c => c.Id == product.Id);
                if (oldInfor == null)
                {
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Add successfully!!!");
                }
                else
                {
                    MessageBox.Show("The MedicineId " + product.Id.ToString() + " is exited!");
                }
            }
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            Product product = GetData();
            if (product != null)
            {
                var oldInfor = _context.Products.FirstOrDefault(c => c.Id == product.Id);
                if (oldInfor != null)
                {
                    oldInfor.Name = product.Name;
                    oldInfor.Quantity = product.Quantity;
                    oldInfor.CategoryId = product.CategoryId;
                    oldInfor.PurchasePrice = product.PurchasePrice;
                    oldInfor.RetailPrice = product.RetailPrice;
                    oldInfor.Stock = product.Stock;
                    _context.Products.Update(oldInfor);
                    _context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Update successfully!!!");
                }
                else
                {
                    MessageBox.Show("The MedicineId " + product.Id.ToString() + " is not exited!");
                }
            }
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            Product product = GetData();
            if (product != null)
            {
                var oldInfor = _context.Products.FirstOrDefault(c => c.Id == product.Id);
                if (oldInfor != null)
                {
                    var resul = MessageBox.Show("are you sure to delete " + product.Id.ToString(), "Question?", MessageBoxButton.YesNoCancel);
                    if (resul == MessageBoxResult.Yes)
                    {
                        _context.Products.Remove(oldInfor);
                        _context.SaveChanges();
                        MessageBox.Show("Delete successfully!!!");
                    }
                    LoadData();
                }
                else
                {
                    MessageBox.Show("The Account Id " + product.Id.ToString() + " is not exited!");
                }
            }
        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

    }
    public class intToCategoryName : IValueConverter
    {
        PROJECTPRN221Context db = new PROJECTPRN221Context();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int CategoryId)
            {
                var tmp = db.Categories.Where(x => x.Id == CategoryId).FirstOrDefault();
                return tmp.Name;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
