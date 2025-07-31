using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.KhachHang.Models
{
    public class DonHangModel
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50)]
        public string? MaDonHang { get; set; }

        [Required]
        [StringLength(500)]
        public string? DiaChiGiaoHang { get; set; }

        [Required]
        public decimal TongTien { get; set; }

        [StringLength(100)]
        public string? TrangThai { get; set; }

        [StringLength(100)]
        public string? PhuongThucThanhToan { get; set; }

        public DateTime ThoiGianDatHang { get; set; } = DateTime.Now;

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        [StringLength(1000)]
        public string? GhiChu { get; set; }

        // Foreign key
        public int? MaNguoiDung { get; set; }

        [ForeignKey("MaNguoiDung")]
        public NguoiDungModel NguoiDung { get; set; }

        // Quan hệ 1-n với ChiTietDonHang
        public ICollection<ChiTietDonHangModel> ChiTietDonHangs { get; set; }
    }
}
