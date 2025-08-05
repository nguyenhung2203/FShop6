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
        public ActionResult ThemDanhMuc(string TenDanhMuc)
        {
            var KetQua = _quanLySanPhamServices.ThemDanhMuc(TenDanhMuc);
            if(KetQua)
            {
            }
            return RedirectToAction("QuanLySanPham");

        }

        [HttpPost]
        public ActionResult SuaDanhMuc(int MaDanhMuc, string TenDanhMuc)
        {
            var KetQua = _quanLySanPhamServices.SuaDanhMucAsync(MaDanhMuc, TenDanhMuc);
            if (KetQua)
            {

            }
            return RedirectToAction("QuanLySanPham");
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
        public async Task<IActionResult> XoaDanhMuc(int MaDanhMuc)
        {
            var KetQua = await _quanLySanPhamServices.XoaDanhMucAsync(MaDanhMuc);
            if (KetQua)
            { 
            }
            return RedirectToAction("QuanLySanPham");
        }
        //[HttpPost]
        //public async Task<IActionResult> ThemSanPham(string TenSanPham, int DanhMucID)
        //{
        //    await _quanLySanPhamServices.ThemSanPhamAsync(form, AnhDaiDien);
        //    var dsSanPham = await _quanLySanPhamServices.LayTatCaSanPhamAsync();
        //    return View("QuanLySanPham", dsSanPham);
        //}
        public async Task<IActionResult> ThemBienThe(IFormCollection form, List<IFormFile> AnhBienThe)
        {
            await _quanLySanPhamServices.ThemBienTheAsync(form, AnhBienThe);
            return RedirectToAction("QuanLySanPham");
        }

        [HttpPost]
        public async Task<IActionResult> SuaBienThe(IFormCollection form, IFormFile AnhBienThe)
        {
            await _quanLySanPhamServices.SuaBienTheAsync(form, AnhBienThe);
            return RedirectToAction("QuanLySanPham");
        }

        [HttpPost]
        public async Task<IActionResult> XoaBienThe(int MaBienThe)
        {
            await _quanLySanPhamServices.XoaBienTheAsync(MaBienThe);
            return RedirectToAction("QuanLySanPham");
        }
    }
}
