namespace SV21T1020052.Shop.Models
{
    public class PaymentModel
    {
        public int OrderId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
    }
}
