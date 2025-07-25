using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class CuaHangController : Controller
    {
        private readonly ICuaHangServices _cuaHangService;

        public CuaHangController(ICuaHangServices cuaHangService)
        {
            _cuaHangService = cuaHangService;
        }

        public async Task<IActionResult> SanPham(int? maDanhMuc, int? loai, int? loaiXapXep, decimal? khoangGia, int trang = 1)
        {
            if (trang < 1) trang = 1;

            PhanTrangSanPhamViewModel model;

            if (loai == 1 && maDanhMuc.HasValue && maDanhMuc.Value > 0)
            {
                model = await _cuaHangService.LaySanPhamDanhMuc(maDanhMuc.Value, trang);
            }
            else if (loai == 2)
            {
                model = await _cuaHangService.LaySanPhamXapXep(loaiXapXep.Value, trang);
            }
            else if (loai == 3)
            {
                model = await _cuaHangService.LaySanPhamTheoGia(khoangGia.Value, trang);
            }    
            else
            {
                model = await _cuaHangService.LaySanPhamTatCa(trang);
            }

            ViewBag.MaDanhMuc = maDanhMuc;
            ViewBag.Loai = loai;
            ViewBag.KhoangGia = khoangGia;

            return View(model);
        }
    }
}