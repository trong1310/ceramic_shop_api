using System;
using System.Collections.Generic;

namespace CeramicShopMasterApi.Databases
{
    public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public bool? IsEnable { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
