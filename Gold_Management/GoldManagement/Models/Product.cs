using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GoldManagement.Models
{
    //[XmlRoot("Products")]
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public int? CategoryId { get; set; }
        public double? PurchasePrice { get; set; }
        public double? RetailPrice { get; set; }
        public string? Image { get; set; }
        public DateTime? Date { get; set; }
        public int? Stock { get; set; }
        [XmlIgnore]
        public virtual Category? Category { get; set; }
        [XmlIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
