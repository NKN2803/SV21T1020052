namespace SV21T1020052.Shop.Models
{
    public class OrderTrackingViewModel
    {
        public Order Order { get; set; }
        public List<TrackingInfo> TrackingInfo { get; set; }
        public List<Order> OrderList { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class TrackingInfo
    {
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
    }
}
    