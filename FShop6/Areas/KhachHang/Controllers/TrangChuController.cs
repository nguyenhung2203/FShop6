using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TrangChuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
