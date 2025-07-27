using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TaiKhoanController : Controller
    {
        private readonly ITaiKhoanServices _taiKhoanServices;
        public TaiKhoanController(ITaiKhoanServices taiKhoanServices)
        {
            _taiKhoanServices = taiKhoanServices;
        }

        private int GetCurrentUserId()
        {

            // Cách 2: Từ Authentication Cookie
            if (User.Identity.IsAuthenticated)
            {
                // Giả sử lưu UserId trong Claims
                var userId = User.Identity.Name; // hoặc User.FindFirst("UserId").Value
                return Convert.ToInt32(userId);
            }

            // Chưa đăng nhập
            throw new UnauthorizedAccessException("Vui lòng đăng nhập để sử dụng tính năng này");
        }

        public IActionResult HoSo(string? tab)
        {
            ViewBag.SelectedTab = tab;
            return View();
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
        public ActionResult ThemYeuThich()
        {
            try
            {
                int nguoiDungId = 6;
                bool ketQua = _taiKhoanServices.ThemSanPham(nguoiDungId, 6); 
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

            return RedirectToAction("Index", "TrangChu", new { area = "KhachHang" });
        }
    }
}