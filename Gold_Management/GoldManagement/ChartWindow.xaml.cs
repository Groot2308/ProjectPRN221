using DocumentFormat.OpenXml.Drawing.Charts;
using GoldManagement.Models;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace GoldManagement
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        PROJECTPRN221Context context = new PROJECTPRN221Context();
        public ChartWindow(Models.Product product)
        {
            InitializeComponent();
            string filename = "OldGold.json";
            string data = File.ReadAllText(filename);
            List<Product> s;
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                s = JsonSerializer.Deserialize<List<Product>>(data);
            }
            Product oldProduct = null; 
            foreach (var item in s)
            {
               if(item.Id == product.Id)
                {
                    oldProduct = item;
                }
            }
            double oldpurchasePrice = 0;
            double oldretailPrice = 0;
            if (oldProduct != null)
            {
                oldpurchasePrice = oldProduct?.PurchasePrice ?? 0;
                oldretailPrice = oldProduct?.RetailPrice ?? 0;
            }

            var productFromDb = context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (productFromDb != null)
            {
                double purchasePrice = productFromDb.PurchasePrice ?? 0;
                double retailPrice = productFromDb.RetailPrice ?? 0;
                SeriesCollection = new SeriesCollection()
        {
            new ColumnSeries
            {
                Title = "Purchase Price",
                Values = new ChartValues<double> { oldpurchasePrice / 1000000, purchasePrice / 1000000 }
            },
            new ColumnSeries
            {
                Title = "Retai lPrice",
                Values = new ChartValues<double> { oldretailPrice / 1000000, retailPrice / 1000000 }
            }
        };
                if (oldProduct != null && productFromDb != null)
                {
                    string label = oldProduct.Date.ToString();
                    string label1 = productFromDb.Date.ToString();
                    Labels = new[] { label, label1 };
                }
                else
                {
                    string label1 = productFromDb.Date.ToString();
                    Labels = new[] { " ", label1 };
                }
                DataContext = this;
            }
        }
    }
}
