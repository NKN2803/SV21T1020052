using SV21T1020052.DomainModels;

namespace SV21T1020052.Shop.Models
{
    public class ProductSearchInput : PaginationSearchResult
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public List<Product> Data { get; set; }
        public int CategoryID { get; set; } = 0;

        public int SupplierID { get; set; } = 0;

        public decimal MinPrice { get; set; } = 0;

        public decimal MaxPrice { get; set; } = 0;
        public string? SearchValue { get; set; } = "";
        public string SortOrder { get; set; } = "default";
    }
}
