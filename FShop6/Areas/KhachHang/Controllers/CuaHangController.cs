using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class CuaHangController : Controller
    {
        public IActionResult SanPham()
        {
            return View();
        }
    }
}
