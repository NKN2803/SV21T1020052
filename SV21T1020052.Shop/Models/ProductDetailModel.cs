using SV21T1020052.DomainModels;

namespace SV21T1020052.Shop.Models
{
    public class ProductDetailModel
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
