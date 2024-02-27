using GoldManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GoldManagement
{
    /// <summary>
    /// Interaction logic for MemberManager.xaml
    /// </summary>
    public partial class MemberManager : UserControl
    {
        public readonly PROJECTPRN221Context _context;
        public MemberManager()
        {
            InitializeComponent();
            _context = new PROJECTPRN221Context();
            LoadData();
        }

        public void LoadData()
        {
            listView.ItemsSource = _context.Accounts.Include(a => a.Role).ToList();
            searchByRole.ItemsSource = _context.Roles.ToList();
        }
        private Account getData()
        {
            Account account = new Account();
            account.Id = searchById.Text;
            account.UserName = searchByName.Text;
            account.Address = searchByAddress.Text;
            account.Phone = searchByPhone.Text;
            account.RoleId = Int32.Parse(searchByRole.SelectedValue.ToString()); ;
            return account;
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            string id = searchById.Text;
            string name = searchByName.Text;
            string address = searchByAddress.Text;
            string phone = searchByPhone.Text;
            int? roleId = string.IsNullOrEmpty(searchByRole.SelectedValue?.ToString()) ? (int ?)null : Int32.Parse(searchByRole.SelectedValue.ToString());
            Search(id, name, address, phone, roleId);
        }
        public void Search(string id, string name, string address, string phone, int? roleId)
        {

            var account = _context.Accounts.ToList();

            account = account.Where(c =>
             (string.IsNullOrEmpty(id) || c.Id.ToUpper().Contains(id.ToUpper())) &&
             (string.IsNullOrEmpty(name) || c.UserName.Contains(name)) &&
             (string.IsNullOrEmpty(address) || c.Address.Contains(address)) &&
             (string.IsNullOrEmpty(phone) || c.Phone.Contains(phone)) &&
             (!roleId.HasValue || c.RoleId == roleId)
             ).ToList();

            listView.ItemsSource = account;
        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            Account account = getData();
            if (account != null)
            {
                var oldInfor = _context.Accounts.FirstOrDefault(c => c.Id == account.Id);
                if (oldInfor != null)
                {
                    var resul = MessageBox.Show("are you sure to delete " + account.Id.ToString(), "Question?", MessageBoxButton.YesNoCancel);
                    if (resul == MessageBoxResult.Yes)
                    {
                        _context.Accounts.Remove(oldInfor);
                        _context.SaveChanges();
                        MessageBox.Show("Delete successfully!!!");
                    }
                    LoadData();
                }
                else
                {
                    MessageBox.Show("The Account Id " + account.Id.ToString() + " is not exited!");
                }
            }
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            Account account = getData();
            if (account != null)
            {
                var oldInfor = _context.Accounts.FirstOrDefault(c => c.Id == account.Id);
                if (oldInfor != null)
                {
                    oldInfor.UserName = account.UserName;
                    oldInfor.Address = account.Address;
                    oldInfor.Phone = account.Phone;
                    oldInfor.RoleId = account.RoleId;   
                    _context.Accounts.Update(oldInfor);
                    _context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Update successfully!!!");
                }
                else
                {
                    MessageBox.Show("The Account Id " + account.Id.ToString() + " is not exited!");
                }
            }
        }

        private void Button_OpenCreate(object sender, RoutedEventArgs e)
        {
            Account account = getData();
            if (account != null)
            {
                var oldInfor = _context.Accounts.FirstOrDefault(c => c.Id == account.Id);
                if (oldInfor == null)
                {
                    _context.Accounts.Add(account);
                    _context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Add successfully!!!");
                }
                else
                {
                    MessageBox.Show("The Account Id " + account.Id.ToString() + " is exited!");
                }
            }
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }

    public class intToRole : IValueConverter
    {
        PROJECTPRN221Context db = new PROJECTPRN221Context();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int RoleId)
            {
                var tmp = db.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
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
