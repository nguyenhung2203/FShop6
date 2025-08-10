using System.ComponentModel.DataAnnotations;

namespace FShop6.Areas.KhachHang.Models
{
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email / tài khoản / số điện thoại")]
        public string ThongTinDangNhap { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
    }
}
