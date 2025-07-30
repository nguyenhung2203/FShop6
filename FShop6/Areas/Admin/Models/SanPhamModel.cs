using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.Admin.Models
{
    [Table("SanPham")]
    public class SanPhamModel
    {
        [Key]
        public int MaSanPham { get; set; }

        [Required]
        public int MaDanhMucSP { get; set; }

        [Required]
        [StringLength(255)]
        public string TenSanPham { get; set; }

        [DataType(DataType.MultilineText)]
        public string MoTa { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string HinhAnhDaiDien { get; set; }

        [Display(Name = "Ngày tạo")]
        [Column("ThoiGianTao")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Liên kết đến bảng DanhMuc (1 sản phẩm thuộc 1 danh mục)
        [ForeignKey("MaDanhMucSP")]
        public DanhMucModel DanhMuc { get; set; }

        // Danh sách các biến thể (màu sắc, size, v.v.) của sản phẩm
        public ICollection<BienTheModels> BienThes { get; set; } = new List<BienTheModels>();
    }

}
