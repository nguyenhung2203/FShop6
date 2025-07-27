using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class GioHangController : Controller
    {
        public IActionResult GioHang()
        {
            return View();
        }

        public IActionResult ThanhToan()
        {
            return View();
        }
    }
}
