using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using static FShop6.Areas.KhachHang.Services.ChiTietSanPhamServices;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class CuaHangController : Controller
    {
        public IActionResult SanPham()
        {
            return View();
        }

        private readonly IChiTietSanPhamService _chiTietSanPhamService;

        public CuaHangController(IChiTietSanPhamService chiTietSanPhamService)
        {
            _chiTietSanPhamService = chiTietSanPhamService;
        }

        public async Task<IActionResult> ChiTietSanPham(int maSanPham)
        {
            maSanPham = 1;
            var model = await _chiTietSanPhamService.LayChiTietSanPhamAsync(maSanPham);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
