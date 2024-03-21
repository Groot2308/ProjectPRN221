using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class AdminManager : Window, INotifyPropertyChanged
    {
        private UserControl _productManagerView;
        private UserControl _OrderManagerView;
        private UserControl _MemberManagerView;

        public event PropertyChangedEventHandler PropertyChanged;
        public UserControl ProductManagerView
        {
            get { return _productManagerView; }
            set
            {
                _productManagerView = value;
                OnPropertyChanged(nameof(ProductManagerView));
            }
        }
        public UserControl OrderManagerView
        {
            get { return _OrderManagerView; }
            set
            {
                _productManagerView = value;
                OnPropertyChanged(nameof(OrderManagerView));
            }
        }
        public UserControl MemberManagerView
        {
            get { return _MemberManagerView; }
            set
            {
                _productManagerView = value;
                OnPropertyChanged(nameof(MemberManagerView));
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AdminManager()
        {
            InitializeComponent();
            DataContext = this;
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
            ProductManagerView = new MemberManager();
        }

        private void OrderManagerButton_Click(object sender, RoutedEventArgs e)
        {
            ProductManagerView = new OrderManager();
        }

        private void ProductManagerButton_Click(object sender, RoutedEventArgs e)
        {
                ProductManagerView = new ProductManager();
        }
    }
}
