
namespace CeramicShopMasterApi.Databases
{
    public partial class Products
    {

        public virtual ICollection<Images> Images { get; set; } = new HashSet<Images>();

    }
}
