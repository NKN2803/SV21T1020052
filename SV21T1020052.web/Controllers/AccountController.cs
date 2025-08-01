using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020052.BusinessLayers;
using SV21T1020052.Web.AppCodes;
using System.Security.Claims;

namespace SV21T1020052.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
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

            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập đầy đủ tên và mật khẩu");
                return View();
            }

            // Kiểm tra username và password
            var userAccount = UserAccountService.Authorize(UserTypes.Employee, username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }

            // Đăng nhập thành công
            WebUserData userData = new WebUserData()
            {
                UserId = userAccount.UserId,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
                Photo = userAccount.Photo,
                Roles = userAccount.RoleNames.Split(',').ToList()
            };

            // Ghi nhận trạng thái đăng nhập
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userData.CreatePrincipal());

            // Quay về trang chủ
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
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

            // Kiểm tra mật khẩu mới không trùng mật khẩu cũ
            if (newPassword == oldPassword)
            {
                ModelState.AddModelError("Password", "Mật khẩu mới không được trùng với mật khẩu cũ.");
                return View();
            }

            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("", "Không tìm thấy tên người dùng.");
                return View();
            }

            var userType = UserTypes.Employee;
            var userAccount = UserAccountService.Authorize(userType, username, oldPassword);
            if (userAccount == null)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                return View();
            }

            bool isPasswordChanged = UserAccountService.ChangePassword(userType, username, oldPassword, newPassword);
            if (isPasswordChanged)
            {
                ViewBag.Message = "Đổi mật khẩu thành công!";
                return View();
            }

            ModelState.AddModelError("", "Đã xảy ra lỗi khi đổi mật khẩu. Vui lòng thử lại.");
            return View();
        }



        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult AccessDenined()
        {
            return View();
        }
    }
}
