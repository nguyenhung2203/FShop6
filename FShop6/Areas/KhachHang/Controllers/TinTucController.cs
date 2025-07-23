using FShop6.Areas.KhachHang.Models;
using Microsoft.AspNetCore.Mvc;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TinTucController : Controller
    {
        public IActionResult TinTuc()
        {
            return View();
        }

        public IActionResult ChiTietTinTuc()
        {
            return View();
        }
    }
}
