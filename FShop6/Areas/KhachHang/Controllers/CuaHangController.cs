using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
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
            var viewModel = await _shopService.LaySanPhamDaLoc(maDanhMuc, khoangGia, loaiXapXep, trang);

            ViewBag.MaDanhMuc = maDanhMuc;
            ViewBag.Loai = loai;
            ViewBag.LoaiXapXep = loaiXapXep;
            ViewBag.KhoangGia = khoangGia;

            return View(viewModel);
        }


        public async Task<IActionResult> ChiTietSanPham(int maSanPham)
        {
            var model = await _shopService.LayChiTietSanPhamAsync(maSanPham);
            if (model == null)
            {
                return NotFound();
            }
            int maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung") ?? 0;
            bool isFavorite = false;
            if (maNguoiDung > 0)
            {
                var taiKhoanServices = HttpContext.RequestServices.GetService<ITaiKhoanServices>();
                isFavorite = taiKhoanServices.KiemTraSanPhamYeuThich(maNguoiDung, maSanPham);
            }
            ViewBag.IsFavorite = isFavorite;
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