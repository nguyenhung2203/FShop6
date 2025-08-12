using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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

        [HttpGet]
        public async Task<IActionResult> QuanLySanPham(string? tuKhoa, string? trangThai)
        {
            var dsSanPham = await _quanLySanPhamServices.LayTatCaSanPhamAsync(tuKhoa, trangThai);
            ViewBag.TuKhoa = tuKhoa;
            ViewBag.TrangThai = trangThai;
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
            var ketQua = await _quanLySanPhamServices.ThemSanPhamAsync(form, AnhDaiDien);
            TempData["ThongBao"] = ketQua.ThongBao;
            TempData["LoaiThongBao"] = ketQua.ThanhCong ? "success" : "warning";
            return RedirectToAction("QuanLySanPham");
        }

        [HttpPost]
        public async Task<IActionResult> SuaSanPham(IFormCollection form, IFormFile AnhDaiDien)
        {
            var ketQuan = await _quanLySanPhamServices.SuaSanPhamAsync(form, AnhDaiDien);
            TempData["ThongBao"] = ketQuan.ThongBao;
            TempData["LoaiThongBao"] = ketQuan.ThanhCong ? "success" : "warning";
            return RedirectToAction("QuanLySanPham");
        }

        [HttpPost]
        public async Task<IActionResult> XoaSanPham(int MaSanPham)
        {
            var ketQua = await _quanLySanPhamServices.XoaSanPhamAsync(MaSanPham);
            TempData["ThongBao"] = ketQua.ThongBao;
            TempData["LoaiThongBao"] = ketQua.ThanhCong ? "success" : "warning";
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
        [HttpPost]
        public async Task<IActionResult> ThemBienThe(IFormCollection form, IFormFile AnhBienThe)
        {
            var ketQua = await _quanLySanPhamServices.ThemBienTheAsync(form, AnhBienThe);
            TempData["ThongBao"] = ketQua.ThongBao;
            TempData["LoaiThongBao"] = ketQua.ThanhCong ? "success" : "warning";
            return RedirectToAction("QuanLySanPham");
        }

        [HttpPost]
        public async Task<IActionResult> SuaBienThe(IFormCollection form, IFormFile AnhBienThe)
        {
            var ketQua = await _quanLySanPhamServices.SuaBienTheAsync(form, AnhBienThe);
            TempData["ThongBao"] = ketQua.ThongBao;
            TempData["LoaiThongBao"] = ketQua.ThanhCong ? "success" : "warning";
            return RedirectToAction("QuanLySanPham");
        }

        [HttpPost]
        public async Task<IActionResult> XoaBienThe(int MaBienThe)
        {
            var ketQua = await _quanLySanPhamServices.XoaBienTheAsync(MaBienThe);
            TempData["ThongBao"] = ketQua.ThongBao;
            TempData["LoaiThongBao"] = ketQua.ThanhCong ? "success" : "warning";
            return RedirectToAction("QuanLySanPham");
        }
    }
}
