namespace CeramicShopMasterApi.Models.ResponseModel
{
    public class ProductResponeMessage
    {
        public int Id { get; set; }
        public string? Slug { get; set; }
        public string? Name { get; set; }
        public List<string?>? Images { get; set; }
        public DataObjectModels? Categories { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool? IsEnable { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
