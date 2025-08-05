using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyTaiKhoanController : Controller
    {
        private readonly ITaiKhoanService _taiKhoanService;

        public QuanLyTaiKhoanController(ITaiKhoanService taiKhoanService)
        {
            _taiKhoanService = taiKhoanService;
        }

        public IActionResult QuanLyTaiKhoan()
        {
            var danhSach = _taiKhoanService.GetAll();
            return View(danhSach);
        }
        public IActionResult CapNhatTrangThai(int MaNguoiDung, string TTHoatDong, string? MatKhau)
        {
            var thanhCong = _taiKhoanService.CapNhatTrangThai(MaNguoiDung, TTHoatDong, MatKhau);

            TempData[thanhCong ? "Success" : "Error"] = thanhCong ?
                "Cập nhật thành công!" :
                "Không tìm thấy người dùng.";

            return RedirectToAction("QuanLyTaiKhoan");
        }

        [HttpPost]
        public IActionResult XoaTaiKhoan(int MaNguoiDung)
        {
            var thanhCong = _taiKhoanService.XoaTaiKhoan(MaNguoiDung);
            if (thanhCong)
                TempData["Success"] = "Xóa tài khoản thành công!";
            else
                TempData["Error"] = "Không thể xóa tài khoản (có thể không tồn tại).";

            return RedirectToAction("QuanLyTaiKhoan");
        }
    }
}
