using Microsoft.AspNetCore.Mvc;
using SV21T1020052.Shop.Models;

namespace SV21T1020052.Shop.Controllers
{
    public class TrackingController : Controller
    {
        // Hiển thị danh sách đơn hàng
        [HttpGet]
        public ActionResult TrackOrder()
        {
            // Giả lập dữ liệu danh sách đơn hàng
            var model = new OrderTrackingListViewModel
            {
                Orders = new List<Order>
                {
                    new Order { OrderId = 1, CustomerName = "Nguyen A", TotalAmount = 1500.00m, Status = "Shipped" },
                    new Order { OrderId = 2, CustomerName = "Tran B", TotalAmount = 2000.00m, Status = "Pending" },
                    new Order { OrderId = 3, CustomerName = "Le C", TotalAmount = 1200.00m, Status = "Delivered" }
                }
            };

            return View(model);
        }

        // Hiển thị chi tiết theo dõi của đơn hàng
        [HttpGet]
        public ActionResult TrackDetail(int orderId)
        {
            // Lấy thông tin đơn hàng theo OrderId
            var order = new Order { OrderId = orderId, CustomerName = "Customer Name", TotalAmount = 1000.00m, Status = "Shipped" };
            var trackingInfo = new List<TrackingInfo>
            {
                new TrackingInfo { Timestamp = DateTime.Now, Status = "Shipped", Details = "Order is shipped" },
                new TrackingInfo { Timestamp = DateTime.Now.AddHours(-1), Status = "Processing", Details = "Order is being processed" }
            };

            // Tạo ViewModel cho trang chi tiết
            var model = new OrderTrackingViewModel
            {
                Order = order,
                TrackingInfo = trackingInfo
            };

            return View(model);
        }
    }
}
