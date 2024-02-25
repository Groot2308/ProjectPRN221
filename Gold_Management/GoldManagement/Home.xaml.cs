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
using GoldManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GoldManagement
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public readonly PROJECTPRN221Context _context;
        public Home()
        {
            InitializeComponent();
            _context = new PROJECTPRN221Context();
            LoadData(); 


        }
        public void LoadData()
        {
            ListProduct.ItemsSource = _context.Products.Include(p => p.Category).ToList();
            searchByCategory.ItemsSource = _context.Categories.ToList();
            cboStatus.ItemsSource = _context.OrderStatuses.ToList();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            string id = searchById.Text;
            string name = searchByName.Text;
            int CategoryId = string.IsNullOrEmpty(searchByCategory.SelectedValue?.ToString()) ? 0 : Int32.Parse(searchByCategory.SelectedValue.ToString());
            Search( id,  name,  CategoryId);
        }
        public void Search(string id, string name, int CategoryId)
        {
            var products = _context.Products.ToList();

            products = products.Where(c =>
                (string.IsNullOrEmpty(id) || c.Id.ToUpper().Contains(id.ToUpper())) &&
                (string.IsNullOrEmpty(name) || c.Name.Contains(name)) &&
                (CategoryId == 0 || c.CategoryId == CategoryId)
            ).ToList();
            ListProduct.ItemsSource = products;
        }


        private void AddToCart(object sender, RoutedEventArgs e)
        {
            int status =int.Parse(cboStatus.SelectedValue.ToString());
            if (status == 1)
            {
                Button? button = (Button)sender;
                if (button != null)
                {
                    var tag = button.Tag;
                    if (!string.IsNullOrEmpty(tag.ToString()))
                    {
                        string id = tag.ToString();
                        Product product = _context.Products.FirstOrDefault(p => p.Id == id);

                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.ProductId = product.Id;
                        orderDetail.QuantityPurchased = 0;
                        orderDetail.QuantitySell = product.Quantity;
                        orderDetail.Price = product.RetailPrice;
                        orderDetail.Product = product;
                        if (Session.carts == null)
                        {
                            Session.carts = new List<OrderDetail> { orderDetail };
                            Session.mode = 1;
                        }
                        else
                        {
                            int index = Session.carts.FindIndex(cart => cart.ProductId == orderDetail.ProductId);
                            if (index == -1)
                            {
                                Session.carts.Add(orderDetail);
                            }
                            else
                            {
                                Session.carts[index].QuantitySell++;
                            }
                        }
                    }

                }
            }
            else
            {
                Button? button = (Button)sender;
                if (button != null)
                {
                    var tag = button.Tag;
                    if (!string.IsNullOrEmpty(tag.ToString()))
                    {
                        string id = tag.ToString();
                        Product product = _context.Products.FirstOrDefault(p => p.Id == id);

                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.ProductId = product.Id;
                        orderDetail.QuantityPurchased = product.Quantity;
                        orderDetail.QuantitySell = 0;
                        orderDetail.Price = product.PurchasePrice;
                        orderDetail.Product = product;
                        if (Session.carts == null)
                        {
                            Session.carts = new List<OrderDetail> { orderDetail };
                            Session.mode = 2;
                        }
                        else
                        {
                            int index = Session.carts.FindIndex(cart => cart.ProductId == orderDetail.ProductId);
                            if (index == -1)
                            {
                                Session.carts.Add(orderDetail);
                            }
                            else
                            {
                                Session.carts[index].QuantityPurchased++;
                            }
                        }
                    }
                }
            }
        }

        private void Button_OpenOrder(object sender, RoutedEventArgs e)
        {
            Cart cart = new Cart();
            cart.Show();
        }

        private void Button_OpenMyOrder(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}