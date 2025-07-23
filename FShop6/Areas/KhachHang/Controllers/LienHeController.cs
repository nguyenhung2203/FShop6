using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class LienHeController : Controller
    {
        public IActionResult LienHe()
        {
            return View();
        }

        public IActionResult GioiThieu()
        {
            return View();
        }

        public IActionResult CauHoi()
        {
            return View();
        }
    }
}
