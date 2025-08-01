using SV21T1020052.DomainModels;

namespace SV21T1020052.Shop.Models
{
    public class HomeModel
    {
        public IList<HomeCategory> Categories { get; set; } 
        public IList<Product> Products { get; set; }
        public ProductSearchInput ProductSearch { get; set; }
    }
}
