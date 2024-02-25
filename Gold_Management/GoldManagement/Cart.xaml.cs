using GoldManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
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
    /// Interaction logic for Cart.xaml
    /// </summary>
    public partial class Cart : Window
    {
        private readonly Home home;
        public readonly PROJECTPRN221Context _context;
        public Cart()
        {
            InitializeComponent();
            _context = new PROJECTPRN221Context();
            this.home = home;
            if (Session.carts == null)
            {
                Session.carts = new List<OrderDetail>();
            }
            updateCarts();
        }
        private void updateCarts()
        {

           
            if (Session.mode == 1)
            {
                listView.ItemsSource = Session.carts.ToList();
                TxtBoxTotalPrice.Text = Session.carts.Sum(cart => (cart.QuantitySell * cart.Product.RetailPrice)).ToString();
            }
            else
            {
                listViewX.ItemsSource = Session.carts.ToList();
                TxtBoxTotalPriceX.Text = Session.carts.Sum(cart => (cart.QuantityPurchased * cart.Product.PurchasePrice)).ToString();
            }
            //home.UpdateCartQuantity();
        }

        private void Button_Checkout(object sender, RoutedEventArgs e)
        {
            if (Session.mode == 1)
            {
                PROJECTPRN221Context context = new PROJECTPRN221Context();
                string customerName = txtName.Text;

                var member = _context.Accounts.FirstOrDefault(a => a.UserName == Session.Account.UserName);
                if (member == null)
                {
                    MessageBox.Show("Not found user");
                    this.Close();
                    home.Close();
                    return;
                }
                List<OrderDetail> orderDetails = Session.carts.Select(cart =>
                {
                    cart.Product = null;
                    return cart;
                }).ToList();
                Order order = new Order();
                order.OrderDate = DateTime.Now;
                order.OrderDetails = orderDetails;
                order.UserId = member.Id;
                order.CustomerName = customerName;
                order.Amount = double.Parse(TxtBoxTotalPrice.Text);
                order.StatusId = 1;
                context.Orders.Add(order);
                foreach (var cart in Session.carts)
                {
                    var product = context.Products.FirstOrDefault(p => p.Id == cart.ProductId);
                    if (product != null)
                    {
                        product.Stock -= cart.QuantitySell;
                    }
                }
                context.SaveChanges();
                MessageBox.Show("done");
                Session.carts = new List<OrderDetail>();
                updateCarts();
            }
            else
            {
                PROJECTPRN221Context context = new PROJECTPRN221Context();
                string customerName = txtName.Text;

                var member = _context.Accounts.FirstOrDefault(a => a.UserName == Session.Account.UserName);
                if (member == null)
                {
                    MessageBox.Show("Not found user");
                    this.Close();
                    home.Close();
                    return;
                }
                List<OrderDetail> orderDetails = Session.carts.Select(cart =>
                {
                    cart.Product = null;
                    return cart;
                }).ToList();
                Order order = new Order();
                order.OrderDate = DateTime.Now;
                order.OrderDetails = orderDetails;
                order.UserId = member.Id;
                order.CustomerName = customerName;
                order.Amount = double.Parse(TxtBoxTotalPriceX.Text);
                order.StatusId = 2;
                context.Orders.Add(order);
                foreach (var cart in Session.carts)
                {
                    var product = context.Products.FirstOrDefault(p => p.Id == cart.ProductId);
                    if (product != null)
                    {
                        product.Stock += cart.QuantityPurchased;
                    }
                }
                context.SaveChanges();
                MessageBox.Show("done");
                Session.carts = new List<OrderDetail>();
                updateCarts();
            }
        }


        private void Button_Plus(object sender, RoutedEventArgs e)
        {
            if (Session.mode == 1)
            {
                Button? button = sender as Button;
                if (button != null)
                {
                    var tag = button.Tag;
                    if (!string.IsNullOrEmpty(tag.ToString()))
                    {
                        string id = tag.ToString();
                        int index = Session.carts.FindIndex(cart => cart.ProductId == id);
                        if (index != -1)
                        {
                            Session.carts[index].QuantitySell += 1;
                            updateCarts();
                        }
                    }
                }
            }
            else
            {
                Button? button = sender as Button;
                if (button != null)
                {
                    var tag = button.Tag;
                    if (!string.IsNullOrEmpty(tag.ToString()))
                    {
                        string id = tag.ToString();
                        int index = Session.carts.FindIndex(cart => cart.ProductId == id);
                        if (index != -1)
                        {
                            Session.carts[index].QuantityPurchased += 1;
                            updateCarts();
                        }
                    }
                }
            }
        }

        private void Button_Minus(object sender, RoutedEventArgs e)
        {
            if (Session.mode == 1)
            {
                Button? button = sender as Button;
                if (button != null)
                {
                    var tag = button.Tag;
                    if (!string.IsNullOrEmpty(tag.ToString()))
                    {
                        string id = tag.ToString();
                        int index = Session.carts.FindIndex(cart => cart.ProductId == id);
                        if (index != -1)
                        {
                            Session.carts[index].QuantitySell -= 1;
                            if (Session.carts[index].QuantitySell <= 0)
                            {
                                Session.carts.RemoveAt(index);
                            }
                            updateCarts();
                        }
                    }
                }
            }
            else
            {
                Button? button = sender as Button;
                if (button != null)
                {
                    var tag = button.Tag;
                    if (!string.IsNullOrEmpty(tag.ToString()))
                    {
                        string id = tag.ToString();
                        int index = Session.carts.FindIndex(cart => cart.ProductId == id);
                        if (index != -1)
                        {
                            Session.carts[index].QuantityPurchased -= 1;
                            if (Session.carts[index].QuantityPurchased <= 0)
                            {
                                Session.carts.RemoveAt(index);
                            }
                            updateCarts();
                        }
                    }
                }
            }

        }

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                var tag = button.Tag;
                if (!string.IsNullOrEmpty(tag.ToString()))
                {
                    string id = tag.ToString();
                    int index = Session.carts.FindIndex(cart => cart.ProductId == id);
                    if (index != -1)
                    {
                        Session.carts.RemoveAt(index);
                        updateCarts();
                    }
                }
            }
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
