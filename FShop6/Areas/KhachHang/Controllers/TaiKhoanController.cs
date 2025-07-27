using Microsoft.AspNetCore.Mvc;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TaiKhoanController : Controller
    {
        public IActionResult HoSo(string? tab)
        {
            ViewBag.SelectedTab = tab;
            return View();
        }
        public IActionResult DangNHap()
        {
            return View();
        }

        public IActionResult DangKy()
        {
            return View();
        }

        public IActionResult QuenMatKhau()
        {
            return View();
        }

        public IActionResult SanPhamYeuThich()
        {
            return View();
        }
    }
}