using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class CuaHangController : Controller
    {
        private readonly ICuaHangServices _cuaHangService;

        public CuaHangController(ICuaHangServices cuaHangService)
        {
            _cuaHangService = cuaHangService;
        }
        public async Task<IActionResult> SanPham()
        {
            var model = await _cuaHangService.laySanPhamTatCa();
            return View(model);
        }
    }
}
