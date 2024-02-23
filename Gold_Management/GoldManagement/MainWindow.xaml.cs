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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;

namespace GoldManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public readonly PROJECTPRN221Context _context;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            PROJECTPRN221Context context = new PROJECTPRN221Context();
            string username = txtUser.Text.ToString();
            string password = txtPass.Password.ToString();
            var account = context.Accounts.FirstOrDefault(a => a.UserName == username && a.Password == password);
            if (account != null)
            {
                Session.Account = account;
                if (account.RoleId == 1)
                {
                    this.Hide();
                    AdminManager adminManager = new AdminManager();
                    adminManager.Show();
                }
                else
                {
                    MessageBox.Show("Staff");
                }
            }

            else
            {
                MessageBox.Show("Please enter username and password");
                   }
            }

        public void resetFormLogin()
        {
            txtUser.Text = null;
            txtPass.Password = null;
        }
    }
}
