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

        public async Task<IActionResult> SanPham(int? maDanhMuc, int? loai, int? loaiXapXep, decimal? khoangGia, int trang = 1)
        {
            if (trang < 1) trang = 1;

            PhanTrangSanPhamViewModel model;

            if (loai == 1 && maDanhMuc.HasValue && maDanhMuc.Value > 0)
            {
                model = await _shopService.LaySanPhamDanhMuc(maDanhMuc.Value, trang);
            }
            else if (loai == 2)
            {
                model = await _shopService.LaySanPhamXapXep(loaiXapXep.Value, trang);
            }
            else if (loai == 3)
            {
                model = await _shopService.LaySanPhamTheoGia(khoangGia.Value, trang);
            }    
            else
            {
                model = await _shopService.LaySanPhamTatCa(trang);
            }

            ViewBag.MaDanhMuc = maDanhMuc;
            ViewBag.Loai = loai;
            ViewBag.KhoangGia = khoangGia;

            return View(model);
        }
        public async Task<IActionResult> ChiTietSanPham(int maSanPham)
        {
            var model = await _shopService.LayChiTietSanPhamAsync(maSanPham);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public IActionResult DatHangThanhCong()
        {
            return View();
        }
    }
}