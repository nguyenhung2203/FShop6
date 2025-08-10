using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using FShop6.Data;
using FShop6.CauHinh;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Threading.Tasks;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TaiKhoanController : BaseController
    {
        private readonly ISanPhamYeuThichServices _yeuThichService; 
        private readonly AppDbContext _context;
        private readonly IKhachHangService _khachHangService;
        private readonly ILogger<TaiKhoanController> _logger;
        private readonly ITaiKhoanServices _taiKhoanServices;
        public TaiKhoanController(IHeaderServices headerServices, AppDbContext context, IKhachHangService khachHangService, ILogger<TaiKhoanController> logger, ITaiKhoanServices taiKhoanServices, ISanPhamYeuThichServices yeuThichService)
             : base(headerServices)
        {
            _taiKhoanServices = taiKhoanServices;
            _context = context;
            _khachHangService = khachHangService;
            _logger = logger;
            _taiKhoanServices = taiKhoanServices;
             _yeuThichService = yeuThichService;
        }
        
        public async Task<IActionResult> SanPhamYeuThich()
        {
            int maNguoiDung = 5; // test cứng
            var model = await _yeuThichService.LayDanhSach(maNguoiDung);
            return View(model);
        }

        // Đổi mật khẩu (từ OTP)
        [HttpPost]
        public IActionResult DoiMatKhauQuaOTP(string email, string newPassword)
        {
            try
            {
                var user = _context.NguoiDung.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tài khoản." });
                }

                user.MatKhau = newPassword; // Gợi ý: Mã hóa mật khẩu
                _context.SaveChanges();

                // Xóa session liên quan đến OTP
                HttpContext.Session.Remove("OTP");
                HttpContext.Session.Remove("OTP_Email");
                HttpContext.Session.Remove("OTP_HetHan");

                return Json(new { success = true, message = "Mật khẩu đã được cập nhật." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đổi mật khẩu.");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi cập nhật mật khẩu." });
            }
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

            var thongTin = model.ThongTinDangNhap?.Trim().ToLowerInvariant();
            var matKhau = model.MatKhau?.Trim();

            var nguoiDung = _context.NguoiDung
                .FirstOrDefault(x =>
                    (x.Email.ToLower() == thongTin ||
                     x.TaiKhoan.ToLower() == thongTin ||
                     x.SoDienThoai == thongTin)
                    && x.MatKhau == matKhau
                    && x.TTHoatDong == "Hoạt động");

            if (nguoiDung == null)
            {
                ViewBag.ThongBao = "Thông tin đăng nhập không đúng hoặc tài khoản bị khóa.";
                return View(model);
            }

            // Lưu session
            HttpContext.Session.SetInt32("MaNguoiDung", nguoiDung.MaNguoiDung);
            HttpContext.Session.SetString("HoTen", nguoiDung.HoTen);
            HttpContext.Session.SetString("VaiTro", nguoiDung.TenVaiTro);
            // Điều hướng theo vai trò
            var vaiTro = nguoiDung.TenVaiTro?.Trim().ToLower();
            if (vaiTro == "admin")
            {
                return RedirectToAction("Index", "TrangChu", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "TrangChu", new { area = "KhachHang" });
            }
        }

        [HttpGet]
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "TrangChu", new { area = "KhachHang" });
        }

        public async Task<IActionResult> HoSo()
        {
            maNguoiDung = Convert.ToInt32(HttpContext.Session.GetInt32("MaNguoiDung"));
            var moDel = await _taiKhoanServices.LayThongTinHoSo(maNguoiDung);
            return View(moDel);
        }
        [HttpPost]
        public ActionResult HuyDon(int maDonHang, string noiDungHuy)
        {
            try
            {
                bool ketQua = _taiKhoanServices.HuyDonHang(maDonHang, maNguoiDung, noiDungHuy);
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
                bool ketQua = _taiKhoanServices.DoiMatKhau(maNguoiDung, matKhauCu, matKhauMoi);
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
        [ValidateAntiForgeryToken]
        public IActionResult DangKy(DangKyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra trùng tài khoản
            bool taiKhoanTonTai = _context.NguoiDung.Any(x => x.TaiKhoan.ToLower() == model.TaiKhoan.Trim().ToLower());
            if (taiKhoanTonTai)
            {
                ViewBag.ThongBao = "Tài khoản đã tồn tại. Vui lòng chọn tên khác.";
                return View(model);
            }

            // Tạo mới người dùng với dữ liệu tạm
            var nguoiDungMoi = new NguoiDungModel
            {
                TaiKhoan = model.TaiKhoan.Trim(),
                MatKhau = model.MatKhau.Trim(),
                HoTen = "Khách " + model.TaiKhoan,            // Tên tạm
                Email = model.TaiKhoan + "@gmail.com",         // Email tạm dựa trên tài khoản
                SoDienThoai = "0000000000",                   // Số điện thoại tạm
                DiaChi = "Chưa cập nhật",                     // Địa chỉ tạm
                TTHoatDong = "Hoạt động",
                TenVaiTro = "Khách hàng",
                ThoiGianTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };
            _context.NguoiDung.Add(nguoiDungMoi);
            _context.SaveChanges();

            TempData["ThongBao"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            TempData["LoaiThongBao"] = "success";
            return RedirectToAction("DangNhap");
        }

        // GET: Quên mật khẩu
        [HttpGet]
        public IActionResult QuenMatKhau() => View();      
        public IActionResult SanPhamYeuThich()
        {
            if (maNguoiDung <= 0)
                return RedirectToAction("DangNhap");

            return View(); 
        }

        [HttpPost]
        public ActionResult ThemYeuThich(int maSanPham, string giaoDien)
        {
            maNguoiDung = Convert.ToInt32(HttpContext.Session.GetInt32("MaNguoiDung"));
            if (maNguoiDung <= 0)
            {
                TempData["ThongBao"] = "Vui lòng đăng nhập để thêm sản phẩm yêu thích.";
                TempData["LoaiThongBao"] = "warning";
                if (!string.IsNullOrEmpty(giaoDien))
                {
                    return RedirectToAction("ChiTietSanPham", "CuaHang", new { area = "KhachHang", maSanPham = maSanPham });
                }
                return RedirectToAction("Index", "TrangChu", new { area = "KhachHang" });
            }
            try
            {
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
            if (giaoDien == "sanpham")
            {
                return RedirectToAction("SanPham", "CuaHang", new { area = "KhachHang" });
            }
            else if (giaoDien == "chitiet")
            {
                return RedirectToAction("ChiTietSanPham", "CuaHang", new { area = "KhachHang", maSanPham = maSanPham });
            }
            return RedirectToAction("Index", "TrangChu", new { area = "KhachHang" });

        }
        [HttpPost]
        public async Task<IActionResult> GuiMaOTP(string email)
        {
            // Kiểm tra giới hạn thời gian gửi lại
            var lanGuiGanNhat = HttpContext.Session.GetString("ThoiGianGuiGanNhat");
            if (DateTime.TryParse(lanGuiGanNhat, out var lanTruoc))
            {
                if ((DateTime.Now - lanTruoc).TotalSeconds < 60)
                {
                    return Json(new { success = false, message = "Vui lòng chờ ít nhất 1 phút để gửi lại mã." });
                }
            }

            // Tìm người dùng theo email
            var nguoiDung = _context.NguoiDung.FirstOrDefault(u => u.Email == email);
            if (nguoiDung == null)
            {
                return Json(new { success = false, message = "Email không tồn tại." });
            }

            // Tạo mã OTP
            var otp = new Random().Next(100000, 999999).ToString();

            // Lưu session
            HttpContext.Session.SetString("OTP", otp);
            HttpContext.Session.SetString("OTP_Email", email);
            HttpContext.Session.SetString("ThoiGianGuiGanNhat", DateTime.Now.ToString());
            HttpContext.Session.SetString("OTP_HetHan", DateTime.Now.AddMinutes(15).ToString());

            // Gửi email
            try
            {
                await EmailHelper.SendEmailAsync(email, "Mã xác thực FShop",
                    $"Mã xác thực của bạn là: {otp}\nMã này có hiệu lực trong 15 phút.");

                return Json(new { success = true, message = "Mã OTP đã được gửi về email của bạn." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gửi email.");
                return Json(new { success = false, message = "Không thể gửi email. Vui lòng thử lại." });
            }
        }
        [HttpPost]
        [HttpPost]
        public IActionResult XacNhanOTP(XacNhanOTPViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin." });
            }

            var otpSession = HttpContext.Session.GetString("OTP");
            var emailSession = HttpContext.Session.GetString("OTP_Email");
            var hetHan = HttpContext.Session.GetString("OTP_HetHan");
            if (otpSession == null || emailSession == null || hetHan == null)
            {
                return Json(new { success = false, message = "OTP không hợp lệ hoặc đã hết hạn." });
            }

            if (emailSession != model.Email.Trim())
            {
                return Json(new { success = false, message = "Email không khớp với mã OTP đã gửi." });
            }

            if (DateTime.TryParse(hetHan, out var thoiGian) && DateTime.Now > thoiGian)
            {
                HttpContext.Session.Clear();
                return Json(new { success = false, message = "Mã OTP đã hết hạn." });
            }

            if (otpSession != model.MaOTP.Trim())
            {
                return Json(new { success = false, message = "Mã OTP không chính xác." });
            }

            return Json(new { success = true, message = "Mã OTP hợp lệ. Vui lòng đặt lại mật khẩu." });
        }
    }
}
