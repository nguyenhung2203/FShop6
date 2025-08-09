using System.ComponentModel.DataAnnotations;

namespace FShop6.Areas.KhachHang.Models
{
    public class XacNhanOTPViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã OTP")]
        public string MaOTP { get; set; }
    }

}
