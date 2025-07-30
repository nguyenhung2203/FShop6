using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TinTucController : BaseController
    {
        public TinTucController(IHeaderServices headerServices)
        : base(headerServices)
        {
        }
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
