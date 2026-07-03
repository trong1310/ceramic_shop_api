namespace CeramicShopMasterApi.Models.ResponseModel
{
    public class ProductResponeMessage
    {
        public string? Slug { get; set; }
        public string? Name { get; set; }
        public DataObjectModels? Categories { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

    }
}
