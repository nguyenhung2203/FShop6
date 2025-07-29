using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class LienHeController : BaseController
    {
        public LienHeController(IHeaderServices headerServices)
        : base(headerServices)
        {
        }
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
