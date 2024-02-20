using System;
using System.Collections.Generic;

namespace GoldManagement.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? CustomerName { get; set; }
        public DateTime? OrderDate { get; set; }
        public double? Amount { get; set; }
        public int StatusId { get; set; }

        public virtual OrderStatus Status { get; set; } = null!;
        public virtual Account User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
