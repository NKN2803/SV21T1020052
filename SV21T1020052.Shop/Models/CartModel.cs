using SV21T1020052.DomainModels;

namespace SV21T1020052.Shop.Models
{
    public class CartModel
    {
        public List<Cart> Cart { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
