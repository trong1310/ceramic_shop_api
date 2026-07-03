using System;
using System.Collections.Generic;

namespace CeramicShopMasterApi.Models.ResponseModel
{
    public class OrderResponseMessage
    {
        public string Uuid { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public byte State { get; set; }
        public bool? IsEnable { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string CreatedBy { get; set; } = null!;
        public List<OrderDetailResponseMessage> OrderDetails { get; set; } = new();
    }

    public class OrderDetailResponseMessage
    {
        public string Uuid { get; set; } = null!;
        public string SlugProduct { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
