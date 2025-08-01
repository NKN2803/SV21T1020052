using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SV21T1020052.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Nhapdulieu()
        {
            var model = new NhanVien()
            {
                HoTen = "Trần Nguyên Phong",
                NgaySinh = new DateTime(1976, 10, 20),
                Luong = 15000000
            };
            return View();
        }

        public IActionResult Nhandulieu(NhanVien nv, string _ngaysinh)
        {
            DateTime? d = toDateTime(_ngaysinh);
            if (d != null) //dữ liệu hợp lệ
            {
                nv.NgaySinh = d.Value;
            }
            return Content($"Họ tên: {nv.HoTen}. Ngày sinh: {nv.NgaySinh}. Lương: {nv.Luong}");
        }

        public DateTime? toDateTime(string input, string formats = "d/M/yyyy;d-M-yyyy;d.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(input, formats.Split(';'), CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }

    //Sử dụng model để nhận dữ liệu
    //Sử dụng class có các thuộc tính trùng tên với tên tham số

    
}

namespace SV21T1020052.Web
{
    public class NhanVien
    {
        public string HoTen { get; set; } = "";
        public DateTime NgaySinh { get; set; }
        public int Luong { get; set; }
    }

    }
