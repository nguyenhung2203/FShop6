using FShop6.Areas.Admin.Models;
using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyDonHangController : Controller
    {
        public IActionResult QuanLyDonHang()
        {
            return View();
        }
    }
}
