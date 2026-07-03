using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CeramicsShopMasterApi.Base.BaseMessages;

namespace CeramicShopMasterApi.Models.RequestModel
{
    public class OrderRequestModels : BaseRequestMessageKeyword
    {
        public byte? State { get; set; } 

    }

    public class CreateOrderRequest
    {
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public byte? State { get; set; } 
        public List<OrderItemRequest> Items { get; set; } = new();
    }

    public class OrderItemRequest
    {
        public string? SlugProduct { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public class UpdateOrderRequest
    {
        public string Uuid { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public byte? State { get; set; }
        public List<OrderItemRequest>? Items { get; set; }
    }
}
