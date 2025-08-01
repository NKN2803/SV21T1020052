using Microsoft.AspNetCore.Mvc;
using SV21T1020052.Shop.Models;

namespace SV21T1020052.Shop.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public ActionResult OrderForm()
        {
            // Trả về view OrderForm với model mặc định
            return View(new OrderInitModel());
        }

        [HttpPost]
        public ActionResult InitOrder(OrderInitModel model)
        {
            // Xử lý khởi tạo đơn hàng
            if (ModelState.IsValid)
            {
                // Gọi dịch vụ để tạo đơn hàng (ví dụ như SaveOrderService)
                // Giả sử trả về một ID đơn hàng
                int orderId = 123;  // Thực tế bạn sẽ lấy từ DB hoặc dịch vụ
                return RedirectToAction("OrderDetails", new { id = orderId });
            }

            return View("OrderForm", model);
        }

    }
}
