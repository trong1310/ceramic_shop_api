using System.ComponentModel.DataAnnotations;
using CeramicsShopMasterApi.Base.BaseMessages;

namespace CeramicShopMasterApi.Models.RequestModel
{
    public class ProductRequestModels : BaseRequestMessageKeyword
    {
    }

    public class CreateProductRequest
    {
        public string? Name { get; set; } = null!;
        public List<string>? Images { get; set; } = null!;
        public int? CategoriesId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }

    public class UpdateProductRequest
    {
        public string? Slug { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public List<string>? Images { get; set; }
        public int? CategoriesId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}
