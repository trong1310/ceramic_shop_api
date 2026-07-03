using System;
using System.Collections.Generic;

namespace CeramicShopMasterApi.Databases
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public string OrderUuid { get; set; } = null!;
        public string SlugProduct { get; set; } = null!;
        /// <summary>
        /// số tiền/1 sản phẩm 
        /// </summary>
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Uuid { get; set; } = null!;
        public bool? IsEnable { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Orders OrderUu { get; set; } = null!;
        public virtual Products SlugProductNavigation { get; set; } = null!;
    }
}
