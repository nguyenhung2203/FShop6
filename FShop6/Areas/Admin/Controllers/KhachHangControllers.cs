using Microsoft.AspNetCore.Mvc;
using FShop6.Areas.Admin.Services;
using FShop6.Areas.Admin.Models; // ViewModel nếu có
using FShop6.Areas.KhachHang.Models;

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

        // ✅ 1. Hiển thị danh sách khách hàng
        public IActionResult QuanLyKhachHang()
        {
            var danhSach = _khachHangService.LayDanhSach();
            return View(danhSach);
        }

        // ✅ 2. Hiển thị form sửa trạng thái
        [HttpGet]
        public IActionResult ChinhSua(int id)
        {
            var khachHang = _khachHangService.TimTheoId(id);
            if (khachHang == null)
                return NotFound();

            return View(khachHang);
        }

        // ✅ 3. Xử lý cập nhật trạng thái
        [HttpPost]
        public IActionResult CapNhatTrangThai(int id, string TTHoatDong)
        {
            if (_khachHangService.ChinhSua(id, TTHoatDong))
            {
                TempData["ThongBao"] = "Cập nhật trạng thái thành công!";
            }
            else
            {
                TempData["Loi"] = "Không thể cập nhật trạng thái!";
            }

            return RedirectToAction("QuanLyKhachHang");
        }

        // ✅ 4. Xóa khách hàng theo Email
        [HttpPost]
        public IActionResult XoaNguoiDung(int maNguoiDung)
        {
            if (_khachHangService.XoaNguoiDung(maNguoiDung))
            {
                TempData["ThongBao"] = "Đã xóa khách hàng.";
            }
            else
            {
                TempData["Loi"] = "Không thể xóa khách hàng.";
            }

            return RedirectToAction("QuanLyKhachHang");
        }


    }
}
