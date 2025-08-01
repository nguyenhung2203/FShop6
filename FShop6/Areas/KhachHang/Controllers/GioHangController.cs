using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class GioHangController : BaseController
    {
        private readonly IGioHangServices _gioHangServices;
        public GioHangController(IHeaderServices headerServices, IGioHangServices gioHangServices)
            : base(headerServices)
        {
            _gioHangServices = gioHangServices;
        }
        public async Task<IActionResult> GioHang()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
                return RedirectToAction("DangNhap", "TaiKhoan", new { area = "KhachHang"});
            var model = await _gioHangServices.LayGioHang((int)maNguoiDung);
            if (model == null)
            {
                TempData["ThongBao"] = "Giỏ hàng của bạn hiện đang trống.";
                TempData["LoaiThongBao"] = "warning";
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThanhToan(GioHangViewModel model)
        {
            var gioCanThanhToan = model.GioHang?.ToList();
            if (gioCanThanhToan == null || gioCanThanhToan.Count == 0)
            {
                TempData["ThongBao"] = "Bạn chưa chọn sản phẩm nào để thanh toán.";
                TempData["LoaiThongBao"] = "warning";
                return RedirectToAction("GioHang");
            }
            var thongTinNguoiNhan = _gioHangServices.ThongTinNguoiNhan(5).Result;
            var viewModel = new GioHangViewModel
            {
                MaNguoiDung = 5, 
                GioHang = gioCanThanhToan,
                DiaChi = model.DiaChi,
                SoDienThoai = thongTinNguoiNhan.SoDienThoai,
                TenNguoiNhan = thongTinNguoiNhan.TenNguoiNhan,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SanPhamThanhToan(GioHangViewModel model)
        {
            try
            {
                var gioCanThanhToan = model.GioHang?.Where(x => x.DuocChon).ToList();
                if (model == null || gioCanThanhToan.Count == 0)
                {
                    TempData["ThongBao"] = "Không có sản phẩm nào được chọn để thanh toán.";
                    TempData["LoaiThongBao"] = "warning";
                    return RedirectToAction("GioHang");
                }
                model.GioHang = gioCanThanhToan;
                var ketQua = _gioHangServices.ThanhToan(model);
                if (ketQua)
                {
                    TempData["ThongBao"] = "Đặt hàng thành công!";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Thanh toán thất bại. Vui lòng thử lại.";
                    TempData["LoaiThongBao"] = "warning";
                }
                return RedirectToAction("GioHang");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                TempData["ThongBao"] = "Có lỗi xảy ra: " + ex.Message;
                TempData["LoaiThongBao"] = "error";
                return RedirectToAction("GioHang");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemGioHang(int maNguoiDung, int maBienThe, int soLuong, int maSanPham)
        {
            try
            {
                maNguoiDung = 5;

                bool ketQua = _gioHangServices.ThemVaoGioHang(maNguoiDung, maBienThe, soLuong);
                if (ketQua)
                {
                    TempData["ThongBao"] = "Thêm sản phẩm vào giỏ hàng thành công.";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Thêm sản phẩm thất bại";
                    TempData["LoaiThongBao"] = "warning";
                }
            }
            catch (SqlException ex)
            {
                TempData["ThongBao"] = "Có lỗi xảy ra: " + ex.Message;
                TempData["LoaiThongBao"] = "error";
            }
            return RedirectToAction("ChiTietSanPham", "CuaHang", new { area = "KhachHang", maSanPham = maSanPham });
        }

        [HttpPost]
        public ActionResult XoaGioHang(int maNguoiDung, int maBienThe)
        {
            try
            {
                bool ketQua = _gioHangServices.xoaGioHang(maNguoiDung = 5, maBienThe);
                if (ketQua)
                {
                    TempData["ThongBao"] = "Xóa sản phẩm khỏi giỏ hàng thành công.";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Xóa sản phẩm thất bại";
                    TempData["LoaiThongBao"] = "warning";
                }
            }
            catch (SqlException ex)
            {
                TempData["ThongBao"] = "Có lỗi xảy ra: " + ex.Message;
                TempData["LoaiThongBao"] = "error";
            }
            return RedirectToAction("GioHang");
        }
    }
}
