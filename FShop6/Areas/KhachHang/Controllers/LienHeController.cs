using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class LienHeController : BaseController
    {
        private readonly IAdminService _adminService;

        // Cập nhật constructor để inject IAdminService
        public LienHeController(IHeaderServices headerServices, IAdminService adminService)
            : base(headerServices)
        {
            _adminService = adminService;
        }

        public IActionResult LienHe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LienHe(string ten, string email, string noidung)
        {
            if (string.IsNullOrWhiteSpace(ten) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(noidung))
            {
                TempData["ThongBao"] = "Vui lòng nhập đầy đủ thông tin.";
                TempData["LoaiThongBao"] = "warning";
                return View();
            }

            try
            {
                // Lấy email admin từ database thay vì gán cứng
                string adminEmail = await _adminService.GetAdminEmailAsync();

                string noiDungMail = $@"
                    Khách hàng liên hệ
                    Tên: {ten}
                    Email: {email}
                    Nội dung: {noidung}
                ";

                await EmailHelper.SendEmailAsync(
                    adminEmail, // Sử dụng email từ database
                    "Khách hàng liên hệ từ website",
                    noiDungMail
                );

                TempData["ThongBao"] = "Gửi thành công! Chúng tôi sẽ phản hồi sớm.";
                TempData["LoaiThongBao"] = "success";
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = "Gửi email thất bại: " + ex.Message;
                TempData["LoaiThongBao"] = "error";
            }

            return View();
        }

        public IActionResult GioiThieu()
        {
            return View();
        }

        public IActionResult CauHoi()
        {
            return View();
        }
    }
}