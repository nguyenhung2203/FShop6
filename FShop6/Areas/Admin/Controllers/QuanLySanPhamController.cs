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

        //[HttpPost]
        //public async Task<IActionResult> ThemSanPham(string TenSanPham, int DanhMucID)
        //{
        //    await _quanLySanPhamServices.ThemSanPhamAsync(form, AnhDaiDien);
        //    var dsSanPham = await _quanLySanPhamServices.LayTatCaSanPhamAsync();
        //    return View("QuanLySanPham", dsSanPham);
        //}
    }
}
