using System;
using System.Collections.Generic;

namespace CeramicShopMasterApi.Databases
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Uuid { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 1 chờ thanh toán, 2 đã thanh toán
        /// </summary>
        public byte State { get; set; }
        public bool? IsEnable { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string CreatedBy { get; set; } = null!;

        public virtual Account CreatedByNavigation { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
