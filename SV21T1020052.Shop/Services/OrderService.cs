using SV21T1020052.Shop.Models;

namespace SV21T1020052.Shop.Services
{
    public class OrderService
    {
        public int CreateOrder(OrderInitModel orderModel)
        {
            // Logic tạo đơn hàng (lưu vào cơ sở dữ liệu, tính toán tổng tiền, v.v.)
            return 123;  // Trả về ID đơn hàng đã tạo
        }

        public OrderTrackingViewModel GetOrderTrackingInfo(int orderId)
        {
            // Logic lấy thông tin theo dõi đơn hàng từ cơ sở dữ liệu
            var model = new OrderTrackingViewModel
            {
                Order = new Order { OrderId = orderId, CustomerName = "Customer Name", TotalAmount = 1000.00m },
                TrackingInfo = new List<TrackingInfo>
                {
                    new TrackingInfo { Timestamp = DateTime.Now, Status = "Shipped", Details = "Order is shipped" }
                }
            };

            return model;
        }
    }
}
