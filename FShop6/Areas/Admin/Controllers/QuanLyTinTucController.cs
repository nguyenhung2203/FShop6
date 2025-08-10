using FShop6.Areas.Admin.Models;
using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyTinTucController : Controller
    {
        private readonly IQuanLyTinTucService _quanLyTinTucService;

        public QuanLyTinTucController(IQuanLyTinTucService quanLyTinTucService)
        {
            _quanLyTinTucService = quanLyTinTucService;
        }

        // GET: Danh sách tin tứ
        public async Task<IActionResult> QuanLyTinTuc()
        {
            var dsTinTuc =await _quanLyTinTucService.LayTatCa();
            return View("~/Areas/Admin/Views/TinTuc/QuanLyTinTuc.cshtml",dsTinTuc); 
        }

        [HttpPost]
        public async Task<IActionResult> ThemTinTuc(IFormCollection form, IFormFile AnhDaiDien)
        {
            var ketQua = await _quanLyTinTucService.Them(form, AnhDaiDien);
            return Json(new
            {
                thanhCong = ketQua.ThanhCong,
                thongBao = ketQua.ThongBao
            });
            return View(QuanLyTinTuc);
        }


        [HttpPost]
        public async Task<IActionResult> SuaTinTuc(IFormCollection form, IFormFile AnhDaiDien)
        {
            var ketQua = await _quanLyTinTucService.Sua(form, AnhDaiDien);
            TempData["ThongBao"] = ketQua.ThongBao;
            TempData["LoaiThongBao"] = ketQua.ThanhCong ? "success" : "warning";
            return RedirectToAction(nameof(Index));
        }

    }

}

