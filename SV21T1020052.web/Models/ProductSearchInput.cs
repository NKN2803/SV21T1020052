namespace SV21T1020052.Web.Models
{
    public class ProductSearchInput : PaginationSearchInput
    {
        public int categoryID { get; set; } = 0;
        public int supplierID { get; set; } = 0;
        public decimal minPrice { get; set; } = 0;
        public decimal maxPrice { get; set; } = 0;
    }
}
