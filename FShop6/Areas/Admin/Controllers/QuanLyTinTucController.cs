using Microsoft.AspNetCore.Mvc;
using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

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

        // 👉 Hiển thị danh sách tin tức
        public async Task<IActionResult> QuanLyTinTuc()
        {
            var danhSach = await _quanLyTinTucService.LayTatCa(); // Trả về List<TinTuc>
            return View("~/Areas/Admin/Views/TinTuc/QuanLyTinTuc.cshtml", danhSach);

        }

        // 👉 Thêm tin tức mới
        [HttpPost]
        public async Task<IActionResult> ThemTinTuc(IFormCollection form, IFormFile AnhDaiDien)
        {
            if (string.IsNullOrWhiteSpace(form["TieuDe"]) || string.IsNullOrWhiteSpace(form["NoiDung"]))
            {
                TempData["Loi"] = "❌ Vui lòng nhập đầy đủ tiêu đề và nội dung.";
                return RedirectToAction(nameof(QuanLyTinTuc));
            }

            var ketQua = await _quanLyTinTucService.Them(form, AnhDaiDien);
            TempData[ketQua.ThanhCong ? "ThongBao" : "Loi"] = ketQua.ThanhCong
                ? "✅ Thêm tin tức thành công!"
                : $"❌ Thêm thất bại: {ketQua.ThongBao}";

            return RedirectToAction(nameof(QuanLyTinTuc));
        }

        // 👉 Sửa tin tức
        [HttpPost]
        public async Task<IActionResult> SuaTinTuc(IFormCollection form, IFormFile AnhDaiDien)
        {
            if (string.IsNullOrWhiteSpace(form["TieuDe"]) || string.IsNullOrWhiteSpace(form["NoiDung"]))
            {
                TempData["Loi"] = "❌ Vui lòng nhập đầy đủ tiêu đề và nội dung.";
                return RedirectToAction(nameof(QuanLyTinTuc));
            }

            var ketQua = await _quanLyTinTucService.Sua(form, AnhDaiDien);
            TempData[ketQua.ThanhCong ? "ThongBao" : "Loi"] = ketQua.ThanhCong
                ? "✅ Cập nhật tin tức thành công!"
                : $"❌ Cập nhật thất bại: {ketQua.ThongBao}";

            return RedirectToAction(nameof(QuanLyTinTuc));
        }

        // 👉 Xóa tin tức
        [HttpPost]
        public async Task<IActionResult> XoaTinTuc(int MaTinTuc)
        {
            var ketQua = await _quanLyTinTucService.Xoa(MaTinTuc);
            TempData[ketQua.ThanhCong ? "ThongBao" : "Loi"] = ketQua.ThanhCong
                ? "🗑️ Đã xóa tin tức thành công."
                : $"❌ Xóa thất bại: {ketQua.ThongBao}";

            return RedirectToAction(nameof(QuanLyTinTuc));
        }
    }
}
