using System.ComponentModel.DataAnnotations;

namespace FShop6.Areas.KhachHang.Models
{
    public class NguoiDungModel
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required]
        [StringLength(255)]
        public string HoTen { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; } // Nên lưu mật khẩu đã được mã hóa

        [Phone]
        [StringLength(20)]
        public string SoDienThoai { get; set; }

        [StringLength(500)]
        public string DiaChi { get; set; }

        [StringLength(100)]
        public string TTHoatDong { get; set; }

        public DateTime ThoiGianTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string TenVaiTro { get; set; }

        // Quan hệ
        public ICollection<GioHangModel> GioHangs { get; set; }
        public ICollection<DonHangModel> DonHangs { get; set; }
    }
}
