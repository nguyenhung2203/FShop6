using System.ComponentModel.DataAnnotations;

namespace FShop6.Areas.KhachHang.Models
{
    public class DangKyViewModel
    {

        [Required(ErrorMessage = "Tài khoản là bắt buộc")]
        public string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 ký tự")]
        public string MatKhau { get; set; }

        [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string XacNhanMatKhau { get; set; }
    }
}
