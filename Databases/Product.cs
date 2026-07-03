using System;
using System.Collections.Generic;

namespace CeramicShopMasterApi.Databases
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int CategoriesId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool? IsEnable { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Category Categories { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
