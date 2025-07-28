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

        public async Task<IActionResult> QuanLyDonHang()
        {
            var dsDonHang = await _quanLyDonHangServices.LayTatCaDonHangAsync();
            if (dsDonHang == null)
            {
                // Kiểm tra nếu dữ liệu null, trả về view thông báo lỗi hoặc xử lý khác
                return View("Error");
            }
            return View(dsDonHang);
        }
    }
}
