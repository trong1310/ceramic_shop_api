using System;
using System.Collections.Generic;

namespace CeramicShopMasterApi.Databases
{
    public partial class Images
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public string? Owner { get; set; }
        public string Uuid { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool? IsEnable { get; set; }
    }
}
