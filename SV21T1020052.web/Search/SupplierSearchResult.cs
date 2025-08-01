using SV21T1020052.DomainModels;
using SV21T1020052.Web.Models;

namespace SV21T1020052.Web.Search
{
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }
    }
}
