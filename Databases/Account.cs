using System;
using System.Collections.Generic;

namespace CeramicShopMasterApi.Databases
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Uuid { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public bool? Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsEnable { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
