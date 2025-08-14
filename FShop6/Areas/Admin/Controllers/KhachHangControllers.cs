using Microsoft.AspNetCore.Mvc;
using FShop6.Areas.Admin.Services;
using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Services;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangController : Controller
    {
        private readonly IKhachHangService _khachHangService;

        public KhachHangController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        // 👉 Hiển thị danh sách khách hàng
        public IActionResult QuanLyKhachHang()
        {
            var danhSach = _khachHangService.LayDanhSachViewModel(); // Trả về List<NguoiDungViewModel>
            return View(danhSach);

        }

        // 👉 Xử lý cập nhật mật khẩu và trạng thái
        [HttpPost]
        public IActionResult ChinhSua(int MaNguoiDung, string MatKhau, string TTHoatDong)
        {
            if (string.IsNullOrWhiteSpace(MatKhau) || string.IsNullOrWhiteSpace(TTHoatDong))
            {
                TempData["Loi"] = "Vui lòng nhập đầy đủ mật khẩu và trạng thái.";
                return RedirectToAction("QuanLyKhachHang");
            }

            var thanhCong = _khachHangService.ChinhSua(MaNguoiDung, TTHoatDong, MatKhau);
            if (thanhCong)
            {
                TempData["ThongBao"] = "✅ Cập nhật khách hàng thành công!";
            }
            else
            {
                TempData["Loi"] = "❌ Không thể cập nhật khách hàng.";
            }

            return RedirectToAction("QuanLyKhachHang");
        }

        // 👉 Xóa khách hàng
        [HttpPost]
        public IActionResult XoaNguoiDung(int maNguoiDung)
        {
            var daXoa = _khachHangService.XoaNguoiDung(maNguoiDung);
            if (daXoa)
            {
                TempData["ThongBao"] = "🗑️ Đã xóa khách hàng thành công.";
            }
            else
            {
                TempData["Loi"] = "❌ Không thể xóa khách hàng.";
            }

            return RedirectToAction("QuanLyKhachHang");
        }
    }
}
