using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Threading.Tasks;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TaiKhoanController : BaseController
    {
        private readonly ITaiKhoanServices _taiKhoanServices;
        public TaiKhoanController(IHeaderServices headerServices, ITaiKhoanServices taiKhoanServices)
            : base(headerServices)
        {
            _taiKhoanServices = taiKhoanServices;
        }


        public async Task<IActionResult> HoSo()
        {
            var moDel = await _taiKhoanServices.LayThongTinHoSo(5);
            return View(moDel);
        }
        [HttpPost]
        public ActionResult HuyDon(string maDonHang)
        {
            try
            {
                bool ketQua = _taiKhoanServices.HuyDonHang(maDonHang);
                if (ketQua)
                {
                    TempData["ThongBao"] = "Hủy đơn hàng thành công.";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Không thể hủy đơn hàng khi đã được xử lý.";
                    TempData["LoaiThongBao"] = "warning";
                }
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = "Có lỗi xảy ra: " + ex.Message;
                TempData["LoaiThongBao"] = "error";
            }
            return RedirectToAction("HoSo", "TaiKhoan");
        }

        [HttpPost]
        public ActionResult CapNhatHoSo(HoSoViewModel hoSo)
        {
            try
            {
                var nguoiDung = hoSo.nguoiDungModels;
                if (nguoiDung == null || nguoiDung.MaNguoiDung == 0)
                {
                    TempData["ThongBao"] = "Dữ liệu người dùng không hợp lệ.";
                    TempData["LoaiThongBao"] = "warning";
                    return RedirectToAction("HoSo", "TaiKhoan");
                }
                bool ketQua = _taiKhoanServices.CapNhatThongTinHoSo(nguoiDung);
                if (ketQua)
                {
                    TempData["ThongBao"] = "Cập nhật hồ sơ thành công.";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Cập nhật hồ sơ thất bại.";
                    TempData["LoaiThongBao"] = "warning";
                }
            }
            catch (SqlException ex)
            {
                TempData["ThongBao"] = "Có lỗi xảy ra Database: " + ex.Message;
                TempData["LoaiThongBao"] = "error";
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = "Có lỗi xảy ra: " + ex.Message;
                TempData["LoaiThongBao"] = "error";
            }
            return RedirectToAction("HoSo", "TaiKhoan");
        }
        public IActionResult DangNHap()
        {
            return View();
        }

        public IActionResult DangKy()
        {
            return View();
        }

        public IActionResult QuenMatKhau()
        {
            return View();
        }

        public IActionResult SanPhamYeuThich()
        {
            return View();
        }

         
        [HttpPost] // Chỉ nhận POST request
        public ActionResult ThemYeuThich(int maSanPham, int maNguoiDung, string giaoDien)
        {
            try
            {
                maNguoiDung = 5;
                bool ketQua = _taiKhoanServices.ThemSanPham(maNguoiDung, maSanPham); 
                if (ketQua)
                {
                    TempData["ThongBao"] = "Thêm sản phẩm yêu thích thành công.";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Sản phẩm đã tồn tại trong danh sách yêu thích.";
                    TempData["LoaiThongBao"] = "warning";
                }
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = "Có lỗi xảy ra: " + ex.Message;
                TempData["LoaiThongBao"] = "error";
            }
            if (!string.IsNullOrEmpty(giaoDien))
            {
                return RedirectToAction("ChiTietSanPham", "CuaHang", new { area = "KhachHang", maSanPham = maSanPham });
            }

            return RedirectToAction("Index", "TrangChu", new { area = "KhachHang" });
        }
    }
}