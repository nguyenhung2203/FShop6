using FShop6.Areas.Admin.Models;
using FShop6.Areas.Admin.Services;
using FShop6.Areas.KhachHang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // Hiển thị danh sách
        public IActionResult QuanLyTaiKhoan()
        {
            var danhSach = _taiKhoanService.GetAll();
            return View(danhSach);
        }

        // Cập nhật trạng thái
        public IActionResult CapNhatTrangThai(int MaNguoiDung, string TTHoatDong, string? MatKhau)
        {
            var thanhCong = _taiKhoanService.CapNhatTrangThai(MaNguoiDung, TTHoatDong, MatKhau);

            TempData[thanhCong ? "Success" : "Error"] = thanhCong ?
                "Cập nhật thành công!" :
                "Không tìm thấy người dùng.";

            return RedirectToAction("QuanLyTaiKhoan");
        }

        // Xóa tài khoản
        public IActionResult XoaTaiKhoan(int MaNguoiDung)
        {
            bool result = _taiKhoanService.XoaTaiKhoan(MaNguoiDung);
            if (result)
            {
                TempData["Success"] = "Xóa tài khoản thành công!";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy tài khoản để xóa!";
            }
            return RedirectToAction("QuanLyTaiKhoan");
        }

        // Thêm tài khoản
        public IActionResult ThemTaiKhoan(TaiKhoanViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = _taiKhoanService.ThemTaiKhoan(model);
                if (result)
                {
                    TempData["Success"] = "Thêm tài khoản thành công!";
                }
                else
                {
                    TempData["Error"] = "Thêm tài khoản thất bại!";
                }
                return RedirectToAction("QuanLyTaiKhoan"); 
            }

            TempData["Error"] = "Dữ liệu nhập chưa hợp lệ!";
            return View(model);
        }
    }
}
