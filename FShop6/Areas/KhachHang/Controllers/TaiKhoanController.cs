using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using FShop6.Data;
using FShop6.CauHinh; // Giả định EmailHelper nằm ở đây
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Threading.Tasks;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TaiKhoanController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IKhachHangService _khachHangService;
        private readonly ILogger<TaiKhoanController> _logger;

        public TaiKhoanController(AppDbContext context, IKhachHangService khachHangService, ILogger<TaiKhoanController> logger)
        {
            _context = context;
            _khachHangService = khachHangService;
            _logger = logger;
        }

        // Đổi mật khẩu (từ OTP)
        [HttpPost]
        public IActionResult DoiMatKhau(string email, string newPassword)
        {
            try
            {
                var user = _context.NguoiDung.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return Json(new { success = false, message = "Email không tồn tại." });
                }

                // Gợi ý: nên mã hóa mật khẩu
                user.MatKhau = newPassword;
                _context.SaveChanges();

                HttpContext.Session.Remove("OTP");
                HttpContext.Session.Remove("OTP_Email");

                return Json(new { success = true, message = "Mật khẩu đã được cập nhật." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đổi mật khẩu.");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi cập nhật mật khẩu." });
            }
        }

        // Hồ sơ người dùng
        public IActionResult HoSo()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
            {
                return RedirectToAction("DangNhap");
            }

            var nguoiDung = _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == maNguoiDung);
            if (nguoiDung == null)
            {
                return RedirectToAction("DangNhap");
            }

            return View(nguoiDung);
        }

        // GET: Đăng nhập
        [HttpGet]
        public IActionResult DangNhap() => View();

        // POST: Đăng nhập
        [HttpPost]
        public IActionResult DangNhap(DangNhapViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var nguoiDung = _context.NguoiDung
                .FirstOrDefault(x => x.Email == model.Email && x.MatKhau == model.MatKhau && x.TTHoatDong == "Hoạt động");

            if (nguoiDung == null)
            {
                ViewBag.ThongBao = "Email hoặc mật khẩu không đúng.";
                return View(model);
            }

            HttpContext.Session.SetInt32("MaNguoiDung", nguoiDung.MaNguoiDung);
            HttpContext.Session.SetString("HoTen", nguoiDung.HoTen);
            HttpContext.Session.SetString("VaiTro", nguoiDung.TenVaiTro);

            return RedirectToAction("DangKy", "TaiKhoan", new { area = "KhachHang" });
            
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
        [HttpPost]
        public ActionResult DoiMatKhau(string? matKhauCu, string? matKhauMoi, string? matKhauNhapLai)
        {
            if (string.IsNullOrEmpty(matKhauCu) || string.IsNullOrEmpty(matKhauMoi) || string.IsNullOrEmpty(matKhauNhapLai))
            {
                TempData["ThongBao"] = "Vui lòng nhập đầy đủ thông tin.";
                TempData["LoaiThongBao"] = "warning";
                return RedirectToAction("HoSo", "TaiKhoan");
            }
            if (matKhauMoi != matKhauNhapLai)
            {
                TempData["ThongBao"] = "Mật khẩu mới và mật khẩu nhập lại không khớp.";
                TempData["LoaiThongBao"] = "warning";
                return RedirectToAction("HoSo", "TaiKhoan");
            }
            try
            {
                bool ketQua = _taiKhoanServices.DoiMatKhau(5, matKhauCu, matKhauMoi);
                if (ketQua)
                {
                    TempData["ThongBao"] = "Đổi mật khẩu thành công.";
                    TempData["LoaiThongBao"] = "success";
                }
                else
                {
                    TempData["ThongBao"] = "Mật khẩu cũ không đúng hoặc có lỗi xảy ra.";
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

        // GET: Đăng ký
        [HttpGet]
        public IActionResult DangKy() => View();

        // POST: Đăng ký
        [HttpPost]
        public IActionResult DangKy(DangKyViewModel model)
        {
            Console.WriteLine($"Đăng ký với Email: {model.Email}, Họ tên: {model.HoTen}");
            if (!ModelState.IsValid)
                return View(model);

            var emailExists = _context.NguoiDung.Any(x => x.Email == model.Email);
            if (emailExists)
            {
                ViewBag.ThongBao = "Email đã được sử dụng.";
                return View(model);
            }

            var nguoiDungMoi = new NguoiDungModel
            {
                HoTen = model.HoTen,
                Email = model.Email,
                MatKhau = model.MatKhau, // Gợi ý: mã hóa
                SoDienThoai = null,
                DiaChi = null,
                TTHoatDong = "Hoạt động",
                TenVaiTro = "Khách hàng",
                ThoiGianTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };

            _context.NguoiDung.Add(nguoiDungMoi);
            _context.SaveChanges();

            ViewBag.ThongBao = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap");
        }

        // GET: Quên mật khẩu
        [HttpGet]
        public IActionResult QuenMatKhau() => View();

        // POST: Gửi mã OTP
        [HttpPost]
        public IActionResult GuiMaOTP(string email)
        {
            try
            {
                var user = _context.NguoiDung.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return Json(new { success = false, message = "Email không tồn tại trong hệ thống." });
                }

                var otp = new Random().Next(100000, 999999).ToString();
                HttpContext.Session.SetString("OTP", otp);
                HttpContext.Session.SetString("OTP_Email", email);

                EmailHelper.GuiMaOTP(email, otp);

                return Json(new { success = true, message = "Mã OTP đã được gửi đến email của bạn." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi gửi OTP");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi gửi OTP." });
            }
        }

        // POST: Xác nhận OTP
        [HttpPost]
        public IActionResult XacNhanOTP(string email, string otp)
        {
            var sessionOtp = HttpContext.Session.GetString("OTP");
            var sessionEmail = HttpContext.Session.GetString("OTP_Email");

            if (sessionOtp == null || sessionEmail == null)
            {
                return Json(new { success = false, message = "Phiên làm việc đã hết hạn, vui lòng thử lại." });
            }

            if (sessionEmail != email || sessionOtp != otp)
            {
                return Json(new { success = false, message = "Mã OTP không đúng." });
            }

            return Json(new { success = true });
        }

        // GET: Sản phẩm yêu thích
        public IActionResult SanPhamYeuThich()
        {
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");
            if (maNguoiDung == null)
                return RedirectToAction("DangNhap");

            return View(); // TODO: Load danh sách sản phẩm yêu thích
        }

        // GET: Đăng xuất
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap");
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
