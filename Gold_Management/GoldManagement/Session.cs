using GoldManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldManagement
{
    internal class Session
    {
        public static Account Account { get; set; }
        public static string? Username { get; set; } = null;
        public static List<OrderDetail> carts { get; set; } = null;

        public static int? mode { get; set; } = null;
    }
}
