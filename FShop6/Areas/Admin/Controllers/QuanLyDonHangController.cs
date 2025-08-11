using FShop6.Areas.Admin.Models;
using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyDonHangController : Controller
    {
        private readonly IQuanLyDonHangServices _quanLyDonHangServices;
        public QuanLyDonHangController(IQuanLyDonHangServices quanLyDonHangServices)
        {
            _quanLyDonHangServices = quanLyDonHangServices;
        }

        [HttpGet]
        public async Task<IActionResult> QuanLyDonHang(string? tuKhoa, DateTime? tuNgay, DateTime? denNgay)
        {
            var dsDonHang = await _quanLyDonHangServices.LayTatCaDonHangAsync(tuKhoa, tuNgay, denNgay);
            ViewBag.TuKhoa = tuKhoa;
            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");
            return View(dsDonHang);
        }

        [HttpPost]
        public async Task<IActionResult> DuyetDonHang(int id)
        {
            try
            {
                bool ketQua = await _quanLyDonHangServices.DuyetDonHang(id);
                if (ketQua)
                {
                    TempData["ThongBao"] = "Đơn hàng đã được duyệt thành công.";
                    TempData["LoaiThongBao"] = "success"; // Thông báo thành công
                }
                else
                {
                    TempData["ThongBao"] = "Lỗi khi duyệt đơn hàng. Vui lòng thử lại sau.";
                    TempData["LoaiThongBao"] = "warning";
                }

                return RedirectToAction("QuanLyDonHang");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi duyệt đơn hàng: " + ex.Message);
                return RedirectToAction("QuanLyDonHang");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SuaDonHang(IFormCollection form)
        {
            try
            {
                bool ketQua = await _quanLyDonHangServices.SuaDonHang(form);
                if (ketQua)
                {
                    TempData["ThongBao"] = "Cập nhật đơn hàng thành công.";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Lỗi khi cập nhật đơn hàng.";
                    TempData["LoaiThongBao"] = "warning";
                }
                return RedirectToAction("QuanLyDonHang");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật đơn hàng: " + ex.Message);
                return RedirectToAction("QuanLyDonHang");
            }
        }
    }
}
