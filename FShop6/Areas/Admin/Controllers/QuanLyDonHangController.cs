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
            return View(dsDonHang);
        }

        [HttpPost]
        public async Task<IActionResult> DuyetDonHang(int id)
        {
            await _quanLyDonHangServices.DuyetDonHang(id);
            return RedirectToAction("QuanLyDonHang");
        }

        [HttpPost]
        public async Task<IActionResult> SuaDonHang(IFormCollection form)
        {
            await _quanLyDonHangServices.SuaDonHang(form);
            return RedirectToAction("QuanLyDonHang");
        }
    } 
}
