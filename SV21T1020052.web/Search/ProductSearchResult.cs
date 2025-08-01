using SV21T1020052.DomainModels;
using SV21T1020052.Web.Models;

namespace SV21T1020052.Web.Search
{
    public class ProductSearchResult : PaginationSearchResult
    {
        public int categoryID { get; set; } = 0;
        public int supplierID { get; set; } = 0;
        public decimal minPrice { get; set; } = 0;
        public decimal maxPrice { get; set; } = 0;

        public required List<Product> Data { get; set; }
    }
}
