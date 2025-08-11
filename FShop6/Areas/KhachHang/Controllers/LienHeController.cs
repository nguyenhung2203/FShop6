using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class LienHeController : BaseController
    {
        public LienHeController(IHeaderServices headerServices)
            : base(headerServices)
        {
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
                string noiDungMail = $@"
                    Khách hàng liên hệ
                    Tên: {ten}
                    Email: {email}
                    Nội dung: {noidung}
                ";

                await EmailHelper.SendEmailAsync(
                    "hungnqpk04040@gmail.com", // Gmail của bạn
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
