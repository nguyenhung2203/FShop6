using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")] // Đánh dấu đây là controller trong khu vực Admin
    public class QuanLySanPhamController : Controller
    {
        private readonly IQuanLySanPhamServices _quanLySanPhamServices;
        public QuanLySanPhamController(IQuanLySanPhamServices quanLySanPhamServices)
        {
            _quanLySanPhamServices = quanLySanPhamServices;
        }

        public async Task<IActionResult> QuanLySanPham()
        {
            var dsSanPham = await _quanLySanPhamServices.LayTatCaSanPhamAsync();
            return View(dsSanPham);
        }

        [HttpPost]
        public async Task<IActionResult> ThemSanPham(IFormCollection form, IFormFile AnhDaiDien)
        {
            await _quanLySanPhamServices.ThemSanPhamAsync(form, AnhDaiDien);
            return RedirectToAction("QuanLySanPham");
        }

        [HttpPost]
        public async Task<IActionResult> SuaSanPham(IFormCollection form, IFormFile AnhDaiDien)
        {
            await _quanLySanPhamServices.SuaSanPhamAsync(form, AnhDaiDien);
            return RedirectToAction("QuanLySanPham");
        }

        [HttpPost]
        public async Task<IActionResult> XoaSanPham(int MaSanPham)
        {
            await _quanLySanPhamServices.XoaSanPhamAsync(MaSanPham);
            return RedirectToAction("QuanLySanPham");
        }

        [HttpPost]
        public async Task<IActionResult> ThemBienThe(IFormCollection form, List<IFormFile> AnhBienThe)
        {
            await _quanLySanPhamServices.ThemBienTheAsync(form, AnhBienThe);
            return RedirectToAction("QuanLySanPham");
        }
    }
}
