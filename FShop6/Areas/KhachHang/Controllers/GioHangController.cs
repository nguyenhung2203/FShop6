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
        int maNguoiDung;
        public async Task<IActionResult> GioHang()
        {
            if (maNguoiDung == 0)
            {
                maNguoiDung = Convert.ToInt32(HttpContext.Session.GetInt32("MaNguoiDung"));
            }
            if (maNguoiDung < 0)
                return RedirectToAction("DangNhap", "TaiKhoan", new { area = "KhachHang"});
            var model = await _gioHangServices.LayGioHang(maNguoiDung);
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
            if (maNguoiDung == 0)
            {
                maNguoiDung = Convert.ToInt32(HttpContext.Session.GetInt32("MaNguoiDung"));
            }
            var gioCanThanhToan = model.GioHang?.ToList();
            if (gioCanThanhToan == null || gioCanThanhToan.Count == 0)
            {
                TempData["ThongBao"] = "Bạn chưa chọn sản phẩm nào để thanh toán.";
                TempData["LoaiThongBao"] = "warning";
                return RedirectToAction("GioHang");
            }
            var thongTinNguoiNhan = _gioHangServices.ThongTinNguoiNhan(maNguoiDung).Result;
            var viewModel = new GioHangViewModel
            {
                MaNguoiDung = maNguoiDung, 
                GioHang = gioCanThanhToan,
                DiaChi = model.DiaChi,
                SoDienThoai = thongTinNguoiNhan.SoDienThoai,
                TenNguoiNhan = thongTinNguoiNhan.TenNguoiNhan,
                DiaChiMacDinh = thongTinNguoiNhan.DiaChiMacDinh,
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
        public ActionResult ThemGioHang(int maBienThe, int soLuong, int maSanPham)
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("MaNguoiDung"));
            try
            {
                bool ketQua = _gioHangServices.ThemVaoGioHang(id, maBienThe, soLuong);
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
        public ActionResult XoaGioHang(int maBienThe)
        {
            try
            {
                bool ketQua = _gioHangServices.xoaGioHang(maNguoiDung, maBienThe);
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
