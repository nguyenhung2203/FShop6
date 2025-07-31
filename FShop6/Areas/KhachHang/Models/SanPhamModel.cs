using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.KhachHang.Models
{
    [Table("SanPham")]
    public class SanPhamModel
    {
        [Key]
        public int MaSanPham { get; set; } // ✅ Khóa chính đúng

        [Required]
        public int MaDanhMucSP { get; set; }

        [Required]
        [StringLength(255)]
        public string TenSanPham { get; set; }

        [DataType(DataType.MultilineText)]
        public string MoTa { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string HinhAnhDaiDien { get; set; } // ✅ Thuộc tính dùng cho ảnh

        [Display(Name = "Ngày tạo")]
        [Column("ThoiGianTao")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [ForeignKey("MaDanhMucSP")]
        public DanhMucModel DanhMuc { get; set; }

        public ICollection<BienTheModels> BienThes { get; set; } = new List<BienTheModels>();
    }
}
