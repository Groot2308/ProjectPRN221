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
    /// Interaction logic for AdminManager.xaml
    /// </summary>
    public partial class AdminManager : Window
    {
        public AdminManager()
        {
            InitializeComponent();
        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsPopup.IsOpen = !OptionsPopup.IsOpen;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow(); 
            mainWindow.Show();
            OptionsPopup.IsOpen = false;
            
        }
        private void CustomerManagerButton_Click(object sender, RoutedEventArgs e)
        {
           CustomerManager customerManager = new CustomerManager();
           customerManager.Show(); 
        }

        private void OrderManagerButton_Click(object sender, RoutedEventArgs e)
        {
            OrderManager orderManager = new OrderManager();
            orderManager.Show(); 
        }

        private void ProductManagerButton_Click(object sender, RoutedEventArgs e)
        {
            ProductManager productManager = new ProductManager();
            productManager.Show(); 
        }
    }
}
