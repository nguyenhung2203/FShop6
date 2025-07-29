using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class CuaHangController : BaseController
    {
        private readonly IShopService _shopService;
        public CuaHangController(IHeaderServices headerServices, IShopService shopService)
            : base(headerServices)
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
            else if (loai == 2 && loaiXapXep.HasValue)
            {
                model = await _shopService.LaySanPhamXapXep(loaiXapXep.Value, trang);
            }
            else if (loai == 3 && khoangGia.HasValue)
            {
                model = await _shopService.LaySanPhamTheoGia(khoangGia.Value, trang);
            }
            else
            {
                model = await _shopService.LaySanPhamTatCa(trang);
            }
            ViewBag.loaiXapXep = loaiXapXep;
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

        [HttpGet]
        public async Task<IActionResult> TimKiem(string tuKhoa)
        {
            var ketQua = await _shopService.TimKiem(tuKhoa);
            return Json(ketQua);
        }

    }
}