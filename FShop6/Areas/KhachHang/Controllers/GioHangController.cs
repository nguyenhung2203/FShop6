using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class GioHangController : Controller
    {
        private readonly IGioHangServices _gioHangServices;
        public GioHangController(IGioHangServices gioHangServices)
        {
            _gioHangServices = gioHangServices;
        }
        public async Task<IActionResult> GioHang()
        {
            var model = await _gioHangServices.LayGioHang(5);
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
            var gioCanThanhToan = model.GioHang?.Where(x => x.DuocChon).ToList();
            if (gioCanThanhToan == null || gioCanThanhToan.Count == 0)
            {
                TempData["ThongBao"] = "Bạn chưa chọn sản phẩm nào để thanh toán.";
                TempData["LoaiThongBao"] = "warning";
                return RedirectToAction("GioHang");
            }

            var viewModel = new GioHangViewModel
            {
                GioHang = gioCanThanhToan,
                DiaChi = model.DiaChi
            };

            return View(viewModel);
        }



        [HttpPost]
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
    }
}
