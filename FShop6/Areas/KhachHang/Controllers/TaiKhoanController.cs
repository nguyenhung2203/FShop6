using Microsoft.AspNetCore.Mvc;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TaiKhoanController : Controller
    {
        public IActionResult HoSo()
        {
            return View();
        }

        public IActionResult SanPhamYeuThich()
        {
            return View();
        }
    }
}