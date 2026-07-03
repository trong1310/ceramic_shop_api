using System;
using System.Collections.Generic;

namespace CeramicShopMasterApi.Databases
{
    public partial class Products
    {
        public Products()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int CategoriesId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool? IsEnable { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Categories Categories { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
