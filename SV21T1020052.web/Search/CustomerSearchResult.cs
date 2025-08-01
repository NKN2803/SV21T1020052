using SV21T1020052.DomainModels;
using SV21T1020052.Web.Models;

namespace SV21T1020052.Web.Search
{
    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List<Customer> Data { get; set; }
    }
}
