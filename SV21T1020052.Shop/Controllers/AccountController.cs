using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020052.BusinessLayers;
using SV21T1020052.DomainModels;
using SV21T1020052.Shop.Appcodes;
using System.Reflection;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020052.Shop.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }




        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.Username = username;

            //Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập đầy đủ tên và mật khẩu");
                return View();
            }

            //Kiểm tra xem username và password (của Employee) có đúng hay không?
            var userAccount = UserAccountService.Authorize(UserTypes.Customer, username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }

            //Đăng nhập thành công

            //1. Tạo ra thông tin "định danh" người dùng
            WebUserData userData = new WebUserData()
            {
                UserId = userAccount.UserId,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
                Photo = userAccount.Photo,
                Roles = userAccount.RoleNames.Split(',').ToList()
            };

            //2. Ghi nhận trạng thái đăng nhập
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());

            //3. Quay về trang chủ
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var username = User.FindFirstValue(nameof(WebUserData.UserName));

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("Password", "Mật khẩu mới và xác nhận mật khẩu không khớp.");
                return View();
            }

            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("", "Không tìm thấy tên người dùng.");
                return View();
            }

            // Xác thực người dùng
            var userType = UserTypes.Customer;
            var userAccount = UserAccountService.Authorize(userType, username, oldPassword);
            if (userAccount == null)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                return View();
            }

            // Thay đổi mật khẩu
            bool isPasswordChanged = UserAccountService.ChangePassword(userType, username, oldPassword, newPassword);
            if (isPasswordChanged)
            {
                ViewBag.Message = "Đổi mật khẩu thành công!";
                return View();
                /*return View("Login");*/
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đổi mật khẩu. Vui lòng thử lại.");
                return View();
            }
        }
        public IActionResult Profile()
        {
            // Lấy dữ liệu người dùng đăng nhập hiện tại
            var user = User.GetUserData();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy thông tin khách hàng từ database
            var customer = CommonDataService.GetCustomer(int.Parse(user.UserId));

            if (customer == null)
            {
                // Nếu không tìm thấy khách hàng, trả về trang lỗi hoặc profile rỗng
                customer = new Customer
                {
                    CustomerName = "Không tìm thấy thông tin",
                    ContactName = "",
                    Province = "",
                    Address = "",
                    Phone = "",
                    Email = ""
                };
            }

            return View(customer);
        }
    }
}