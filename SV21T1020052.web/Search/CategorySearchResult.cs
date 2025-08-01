using SV21T1020052.DomainModels;
using SV21T1020052.Web.Models;

namespace SV21T1020052.Web.Search
{
    public class CategorySearchResult :PaginationSearchResult
    {
        public required List<Category> Data { get; set; }
    }
}
