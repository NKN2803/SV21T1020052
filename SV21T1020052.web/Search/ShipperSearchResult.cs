using SV21T1020052.DomainModels;
using SV21T1020052.Web.Models;

namespace SV21T1020052.Web.Search
{
    public class ShipperSearchResult : PaginationSearchResult
    {
        public required List<Shipper> Data { get; set; }
    }
}
