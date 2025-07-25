using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class CuaHangController : Controller
    {
        private readonly IShopService _shopService;

        public CuaHangController(IShopService shopService)
        {
            _shopService = shopService;
        }

        public async Task<IActionResult> SanPham(int? maDanhMuc, int? loai, int trang = 1)
        {
            if (trang < 1) trang = 1;

            PhanTrangSanPhamViewModel model;

            if (loai == 1 && maDanhMuc.HasValue && maDanhMuc.Value > 0)
            {
                model = await _shopService.LaySanPhamDanhMuc(maDanhMuc.Value, trang);
            }
            else
            {
                model = await _shopService.LaySanPhamTatCa(trang);
            }

            ViewBag.MaDanhMuc = maDanhMuc;
            ViewBag.Loai = loai;

            return View(model);
        }

        public async Task<IActionResult> ChiTietSanPham(int maSanPham)
        {
            maSanPham = 1;
            var model = await _shopService.LayChiTietSanPhamAsync(maSanPham);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ThemVaoGioHang(int maSanPham, int maBienThe, int soLuong)
        {

            int maNguoiDung = 1;

            if (maBienThe <= 0 || soLuong <= 0)
            {
                return BadRequest("Thông tin không hợp lệ.");
            }

            try
            {
                await _shopService.ThemVaoGioHang(maNguoiDung, maBienThe, soLuong);
                return RedirectToAction("ChiTietSanPham", new { maSanPham = maSanPham });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi: " + ex.Message);
            }
        }
    }
}