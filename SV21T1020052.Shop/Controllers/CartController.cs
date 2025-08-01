using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SV21T1020052.BusinessLayers;
using SV21T1020052.DomainModels;
using SV21T1020052.Shop.Appcodes;
using SV21T1020052.Shop.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace SV21T1020052.Shop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.Get<List<Cart>>("Cart") ?? new List<Cart>();
            var model = new CartModel
            {
                Cart = cartItems,
                TotalAmount = cartItems.Sum(item => item.Price * item.Quantity)
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            if (quantity < 1) quantity = 1;

            var cartItems = HttpContext.Session.Get<List<Cart>>("Cart") ?? new List<Cart>();

            // Lấy thông tin sản phẩm từ database
            var product = ProductDataService.GetProduct(productId);

            if (product != null)
            {
                var cartItem = cartItems.FirstOrDefault(item => item.Id == productId);
                if (cartItem != null)
                {
                    cartItem.Quantity += quantity; // ✅ Cộng số lượng đúng
                }
                else
                {
                    cartItems.Add(new Cart
                    {
                        Id = product.ProductID,
                        Name = product.ProductName,
                        Price = product.Price,
                        Quantity = quantity,
                        Photo = product.Photo
                    });
                }


                // Lưu lại giỏ hàng vào session
                HttpContext.Session.Set("Cart", cartItems);

                return Json(new
                {
                    success = true,
                    cartCount = cartItems.Sum(x => x.Quantity), // ✅ Đếm tổng số lượng thay vì số sản phẩm khác nhau
                    message = "Đã thêm sản phẩm vào giỏ hàng!"
                });

            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Sản phẩm không tồn tại!"
                });
            }
        }


        public IActionResult UpdateQuantity(int productId, int change)
        {
            var cartItems = HttpContext.Session.Get<List<Cart>>("Cart") ?? new List<Cart>();
            var cartItem = cartItems.FirstOrDefault(item => item.Id == productId);

            if (cartItem != null)
            {
                int newQuantity = cartItem.Quantity + change;

                // Cập nhật số lượng nếu hợp lệ
                if (newQuantity >= 1)
                {
                    cartItem.Quantity = newQuantity;
                    HttpContext.Session.Set("Cart", cartItems);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Số lượng sản phẩm không thể nhỏ hơn 1!"
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Không tìm thấy sản phẩm trong giỏ hàng!"
            });
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            var cartItems = HttpContext.Session.Get<List<Cart>>("Cart") ?? new List<Cart>();
            var cartItem = cartItems.FirstOrDefault(item => item.Id == productId);

            if (cartItem != null)
            {
                cartItems.Remove(cartItem);
                HttpContext.Session.Set("Cart", cartItems);
                return Json(new { success = true });
            }

            return Json(new
            {
                success = false,
                message = "Không tìm thấy sản phẩm trong giỏ hàng!"
            });
        }
    }
}
