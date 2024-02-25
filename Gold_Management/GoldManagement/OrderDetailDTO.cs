using GoldManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldManagement
{
    public partial class OrderDetailDTO
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public string? ProductId { get; set; }
        public int? QuantityPurchased { get; set; }
        public int? QuantitySell { get; set; }
        public double? Price { get; set; }

        public int count { get; set; }  

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
