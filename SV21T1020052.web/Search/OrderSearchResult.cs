using SV21T1020052.DomainModels;
using SV21T1020052.Web.Models;

namespace SV21T1020052.Web.Search
{
    public class OrderSearchResult : PaginationSearchResult
    {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }
}
