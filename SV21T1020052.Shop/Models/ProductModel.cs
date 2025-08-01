using SV21T1020052.DomainModels;

namespace SV21T1020052.Shop.Models
{
        public class ProductModel
        {
            public List<Product> Products { get; set; } = new List<Product>();
            public List<Category> Categories { get; set; } = new List<Category>();
            public ProductSearchInput SearchInput { get; set; } = new ProductSearchInput();
            public int TotalPages { get; set; }
            public int DefaultPageSize { get; set; }
    }
}

