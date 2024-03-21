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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

using System.Data;
using System.IO;
using ClosedXML.Excel;
using Microsoft.Win32;

namespace GoldManagement
{
    /// <summary>
    /// Interaction logic for OrderManager.xaml
    /// </summary>
    public partial class OrderManager : UserControl
    {
        public readonly PROJECTPRN221Context _context;
        public OrderManager()
        {
            InitializeComponent();
            _context = new PROJECTPRN221Context();
            listView.MouseDoubleClick += ListView_MouseDoubleClick;
            LoadData();
        }
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement((ListView)sender, e.OriginalSource as DependencyObject) as ListViewItem;
            if (item == null)
                return;
            var selectedOrder = (Order)item.DataContext;
            ShowOrderDetailsWindow(selectedOrder);
        }
        private void ShowOrderDetailsWindow(Order order)
        {
            var detailsWindow = new OrderDetailManager(order);
            detailsWindow.Show();
        }


        private void LoadData()
        {
            listView.ItemsSource = _context.Orders.Include(a => a.Status)
                .OrderByDescending(order => order.OrderDate).ToList();
            searchByStatus.ItemsSource = _context.OrderStatuses.ToList();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDate.SelectedDate == null ? null : StartDate.SelectedDate.Value.Date;
            DateTime? endDate = EndDate.SelectedDate == null ? null : EndDate.SelectedDate.Value.Date;
            int? StatusId = string.IsNullOrEmpty(searchByStatus.SelectedValue?.ToString()) ? (int?)null : Int32.Parse(searchByStatus.SelectedValue.ToString());
            Search(startDate, endDate, StatusId);

        }

        private void Search(DateTime? startDate, DateTime? endDate, int? statusId)
        {
            var orders = _context.Orders.ToList();
            orders = orders.Where(order =>
                (!startDate.HasValue || order.OrderDate >= startDate) &&
                (!endDate.HasValue || order.OrderDate <= endDate) &&
                (!statusId.HasValue || order.StatusId == statusId)
            ).OrderByDescending(order => order.OrderDate).ToList();

            listView.ItemsSource = orders;
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Button_Export(object sender, RoutedEventArgs e)
        {
            List<Order> orders = listView.ItemsSource as List<Order>;
            DataTable orderTable = ConvertToDataTable(orders);
            List<OrderDetail> orderDetails = _context.OrderDetails.Include(o => o.Product).OrderByDescending(o => o.OrderId).ToList();
            DataTable orderDetailTable = ConvertToOrderDetailDataTable(orderDetails);

            // Xuất ra file Excel
           ExportToExcel(orderTable, orderDetailTable);
        }

        private DataTable ConvertToDataTable(List<Order> orders)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OrderId");
            dt.Columns.Add("Staff");
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("OrderDate");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Status");

            foreach (var order in orders)
            {
                dt.Rows.Add(order.Id, order.UserId, order.CustomerName, order.OrderDate, order.Amount, order.Status.Name);
            }

            return dt;
        }
        private DataTable ConvertToOrderDetailDataTable(List<OrderDetail> OrderDetails)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OrderId");
            dt.Columns.Add("ProductId");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("QuantityPurchased");
            dt.Columns.Add("QuantitySell");
            dt.Columns.Add("Price");

                foreach (var detail in OrderDetails)
                {
                    dt.Rows.Add(detail.OrderId, detail.ProductId, detail.Product.Name, detail.QuantityPurchased,detail.QuantitySell, detail.Price);
                }

            return dt;
        }
        private void ExportToExcel(DataTable orderTable, DataTable orderDetailTable)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(orderTable, "Orders");
                wb.Worksheets.Add(orderDetailTable, "OrderDetails");

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveFileDialog.FileName = "Orders.xlsx";
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


    }
}
