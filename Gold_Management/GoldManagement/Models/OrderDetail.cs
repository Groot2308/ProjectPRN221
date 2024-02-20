using System;
using System.Collections.Generic;

namespace GoldManagement.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public string? ProductId { get; set; }
        public int? QuantityPurchased { get; set; }
        public int? QuantitySell { get; set; }
        public double? Price { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
