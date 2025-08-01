using SV21T1020052.DomainModels;

namespace SV21T1020052.Web.Models
{
    public class OrderDetailModel
    {
        public Order? Order { get; set; }
        public required List<OrderDetail> Details { get; set; }
    }
}
