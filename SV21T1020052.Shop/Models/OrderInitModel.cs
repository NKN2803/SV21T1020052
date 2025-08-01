namespace SV21T1020052.Shop.Models
{
    public class OrderInitModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string DeliveryProvince { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
