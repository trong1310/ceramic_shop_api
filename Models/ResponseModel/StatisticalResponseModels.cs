using System.Collections.Generic;

namespace CeramicShopMasterApi.Models.ResponseModel
{
    public class VolumeStatisticsResponse
    {
        public List<CategoryVolumeStat> Categories { get; set; } = new();
        public List<ProductVolumeStat> Products { get; set; } = new();
    }

    public class CategoryVolumeStat
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public int TotalQuantitySold { get; set; }
    }

    public class ProductVolumeStat
    {
        public string ProductSlug { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public int TotalQuantitySold { get; set; }
        public DataObjectModels Categories { get; set; }
    }

    public class ProductRevenueListResponse
    {
        public List<ProductRevenueResponse> Products { get; set; } = new();
        public decimal GrandTotalRevenue { get; set; }
    }

    public class ProductRevenueResponse
    {
        public string ProductSlug { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
