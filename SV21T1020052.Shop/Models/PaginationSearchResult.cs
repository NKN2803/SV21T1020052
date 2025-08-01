using SV21T1020052.DomainModels;

namespace SV21T1020052.Shop.Models
{
    public class PaginationSearchResult
    {
        public int Page { get; set; }
        public int PageSize {  get; set; }
        public string SearchValue { get; set; } = "";
        public int RowCount { get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;
                int c = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    c += 1;
                return c;            
            }
        }
    }
    public class CustomerSearcValueResult : PaginationSearchResult
    { 
        public required List<Customer> Data { get; set; }

    }
	public class CategorySearchValueResult : PaginationSearchResult
	{
		public required List<Category> Data { get; set; }

	}
	public class EmployeeSearchValueResult : PaginationSearchResult
	{
		public required List<Employee> Data { get; set; }

	}
	public class ProductSearchValueResult : PaginationSearchResult
	{
		public required List<Product> Data { get; set; }

	}
	public class ShipperSearchValueResult : PaginationSearchResult
	{
		public required List<Shipper> Data { get; set; }

	}
	public class SupplierSearchValueResult : PaginationSearchResult
	{
		public required List<Supplier> Data { get; set; }

	}
}
