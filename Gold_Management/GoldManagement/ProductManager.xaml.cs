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

using System.Data;
using System.IO;
using ClosedXML.Excel;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;

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
            product.Quantity = int.Parse(searchByQuantity.Text);
            product.PurchasePrice = double.Parse(searchByPurchasePrice.Text);
            product.RetailPrice = double.Parse(searchByRetailPrice.Text);
            product.Date = searchByDate.SelectedDate;
            product.CategoryId = Int32.Parse(searchByCategory.SelectedValue.ToString());
            product.Stock = 10;
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
            DateTime? slDate = searchByDate.SelectedDate != null ? searchByDate.SelectedDate.Value : (DateTime?)null;
            int CategoryId = string.IsNullOrEmpty(searchByCategory.SelectedValue?.ToString()) ? 0 : Int32.Parse(searchByCategory.SelectedValue.ToString());
            Search(id, name, Quantity, PurchasePrice, RetailPrice, slDate, CategoryId);

        }
        public void Search(string id, string name, double Quantity, double PurchasePrice, double RetailPrice, DateTime? slDate, int CategoryId)
        {
            var products = _context.Products.ToList();

            products = products.Where(c =>
                (string.IsNullOrEmpty(id) || c.Id.ToUpper().Contains(id.ToUpper())) &&
                (string.IsNullOrEmpty(name) || c.Name.Contains(name)) &&
                (Quantity == 0 || c.Quantity == Quantity) &&
                (PurchasePrice == 0 || c.PurchasePrice == PurchasePrice) &&
                (RetailPrice == 0 || c.RetailPrice == RetailPrice) &&
                (slDate == null || c.Date == slDate) &&
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
                    MessageBox.Show("The product " + product.Id.ToString() + " is exited!");
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
                    List<Product> existingProducts = new List<Product>();
                    if (File.Exists("OldGold.json"))
                    {
                        string existingJson = File.ReadAllText("OldGold.json");
                        existingProducts = JsonConvert.DeserializeObject<List<Product>>(existingJson);
                        //foreach (Product newProduct in oldInfor)
                        //{
                            Product existingProduct = existingProducts.Find(p => p.Id == oldInfor.Id);
                            if (existingProduct != null)
                            {
                                existingProduct.Name = oldInfor.Name;
                                existingProduct.Quantity = oldInfor.Quantity;
                                existingProduct.CategoryId = oldInfor.CategoryId;
                                existingProduct.PurchasePrice = oldInfor.PurchasePrice;
                                existingProduct.RetailPrice = oldInfor.RetailPrice;
                                existingProduct.Date = oldInfor.Date;
                            }
                            else
                            {
                                existingProducts.Add(oldInfor);
                            }
                        //}
                    }
                    else
                    {
                        //existingProducts = oldInfor;
                    }
                    //string updatedJson = JsonConvert.SerializeObject(existingProducts);
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    string updatedJson = JsonConvert.SerializeObject(existingProducts, settings);
                    File.WriteAllText("OldGold.json", updatedJson);
                }

                if (oldInfor != null)
                {

                    oldInfor.Name = product.Name;
                    oldInfor.Quantity = product.Quantity;
                    oldInfor.CategoryId = product.CategoryId;
                    oldInfor.PurchasePrice = product.PurchasePrice;
                    oldInfor.RetailPrice = product.RetailPrice;
                    oldInfor.Date = DateTime.Now;
                    _context.Products.Update(oldInfor);
                    _context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Update successfully!!!");
                }
                else
                {
                    MessageBox.Show("The product Id " + product.Id.ToString() + " is not exited!");
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
                    MessageBox.Show("The product Id " + product.Id.ToString() + " is not exited!");
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

        private void Button_Import(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                List<Product> productsFromExcel = ReadProductsFromExcel(openFileDialog.FileName);
                ImportProductsToDatabase(productsFromExcel);
            }
        }

        private List<Product> ReadProductsFromExcel(string filePath)
        {
            List<Product> products = new List<Product>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    // Đọc dữ liệu từ các ô trong mỗi dòng
                    string id = row.Cell(1).Value.ToString();
                    string name = row.Cell(2).Value.ToString();
                    int quantity = row.Cell(3).GetValue<int>();
                    double purchasePrice = row.Cell(5).GetValue<double>();
                    double retailPrice = row.Cell(6).GetValue<double>();
                    int stock = row.Cell(7).GetValue<int>();
                    int categoryId = row.Cell(4).GetValue<int>();

                    Product product = new Product
                    {
                        Id = id,
                        Name = name,
                        Quantity = quantity,
                        PurchasePrice = purchasePrice,
                        RetailPrice = retailPrice,
                        Stock = stock,
                        CategoryId = categoryId,
                        Date = DateTime.Now,
                    };
                    products.Add(product);
                }
            }

            return products;
        }

        private void ImportProductsToDatabase(List<Product> products)
        {
            //if (products != null)
            //{
            //    string json = JsonConvert.SerializeObject(products);
            //    string jsonFilePath = "OldGold.json";
            //    File.WriteAllText(jsonFilePath, json);
            //}

            if (products != null)
            {
                List<Product> existingProducts = new List<Product>();
                if (File.Exists("OldGold.json"))
                {
                    string existingJson = File.ReadAllText("OldGold.json");
                    existingProducts = JsonConvert.DeserializeObject<List<Product>>(existingJson);
                    foreach (Product newProduct in products)
                    {
                        var oldInfor = _context.Products.FirstOrDefault(c => c.Id == newProduct.Id);
                        Product existingProduct = existingProducts.Find(p => p.Id == newProduct.Id);
                        if (existingProduct != null)
                        {
                            existingProduct.Name = oldInfor.Name;
                            existingProduct.Quantity = oldInfor.Quantity;
                            existingProduct.CategoryId = oldInfor.CategoryId;
                            existingProduct.PurchasePrice = oldInfor.PurchasePrice;
                            existingProduct.RetailPrice = oldInfor.RetailPrice;
                            existingProduct.Date = oldInfor.Date;
                        }
                        else
                        {
                            existingProducts.Add(newProduct);
                        }
                    }
                }
                else
                {
                    existingProducts = products;
                }
                string updatedJson = JsonConvert.SerializeObject(existingProducts);
                File.WriteAllText("OldGold.json", updatedJson);
            }

            foreach (var product in products)
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Quantity = product.Quantity;
                    existingProduct.PurchasePrice = product.PurchasePrice;
                    existingProduct.RetailPrice = product.RetailPrice;
                    existingProduct.Stock = product.Stock;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.Date = product.Date;
                    _context.Products.Update(existingProduct);
                    _context.SaveChanges();
                }
                else
                {
                    _context.Products.Add(product);
                    _context.SaveChanges();
                }
            }
            LoadData();

            MessageBox.Show("Nhập sản phẩm thành công!");
        }

        private void Button_Export(object sender, RoutedEventArgs e)
        {
            List<Product> products = _context.Products.ToList();
            List<Category> categories = _context.Categories.ToList();
            DataTable productTable = ConvertToProductsDataTable(products);
            DataTable CategoryTable = ConvertToCategoryDataTable(categories);
            ExportToExcel(productTable, CategoryTable);
        }

        private void ExportToExcel(DataTable productTable, DataTable CategoryTable)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(productTable, "Product");
                wb.Worksheets.Add(CategoryTable, "Category");
                // Tạo SaveFileDialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveFileDialog.FileName = "Products.xlsx";
                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    string filePath = saveFileDialog.FileName;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        byte[] bytes = stream.ToArray();
                        File.WriteAllBytes(filePath, bytes);
                    }

                    MessageBox.Show("File đã được lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private DataTable ConvertToCategoryDataTable(List<Category> categories)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CategoryId");
            dt.Columns.Add("CategoryName");

            foreach (var detail in categories)
            {
                dt.Rows.Add(detail.Id, detail.Name);
            }
            return dt;
        }

        private DataTable ConvertToProductsDataTable(List<Product> products)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Category");
            dt.Columns.Add("PurchasePrice");
            dt.Columns.Add("RetailPrice");
            dt.Columns.Add("Stock");
            dt.Columns.Add("Date");

            foreach (var detail in products)
            {
                dt.Rows.Add(detail.Id, detail.Name, detail.Quantity, detail.CategoryId, detail.PurchasePrice, detail.RetailPrice, detail.Stock, detail.Date);
            }

            return dt;
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
