using System;
using System.Collections.Generic;

namespace CeramicShopMasterApi.Databases
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public string OrderUuid { get; set; } = null!;
        public string SlugProduct { get; set; } = null!;
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Uuid { get; set; } = null!;
        public bool? IsEnable { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Order OrderUu { get; set; } = null!;
        public virtual Product SlugProductNavigation { get; set; } = null!;
    }
}
