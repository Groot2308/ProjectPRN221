using System;
using System.Collections.Generic;

namespace GoldManagement.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public double? Quantity { get; set; }
        public int? CategoryId { get; set; }
        public double? PurchasePrice { get; set; }
        public double? RetailPrice { get; set; }
        public string? Image { get; set; }
        public int? Stock { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
