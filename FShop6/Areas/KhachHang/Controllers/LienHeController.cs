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
                ViewBag.ThongBao = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            try
            {
                string noiDungMail = $@"
                    <h3>Khách hàng liên hệ</h3>
                    <p><b>Tên:</b> {ten}</p>
                    <p><b>Email:</b> {email}</p>
                    <p><b>Nội dung:</b><br/>{noidung}</p>
                ";

                await EmailHelper.SendEmailAsync(
                    "taiptpk04158@gmail.com", // Gmail của bạn
                    "Khách hàng liên hệ từ website",
                    noiDungMail
                );

                ViewBag.ThongBao = "Gửi thành công! Chúng tôi sẽ phản hồi sớm.";
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Gửi email thất bại: " + ex.Message;
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
