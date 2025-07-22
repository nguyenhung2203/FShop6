using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.KhachHang.Models
{
    [Table("BienThe")]
    public class BienTheModels
    {
        [Key]
        public int MaBienThe { get; set; }

        [Required]
        public int MaSanPham { get; set; }

        [Required]
        [StringLength(100)]
        public string MaSKU { get; set; }

        [Required]
        [StringLength(255)]
        public string LoaiBienThe { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaNhap { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaBan { get; set; }

        [Range(0, int.MaxValue)]
        public int SoLuongConLai { get; set; }

        [StringLength(100)]
        public string TinhTrang { get; set; }

        public bool NoiBat { get; set; }

        // Navigation property
        [ForeignKey("MaSanPham")]
        public SanPhamModel SanPham { get; set; }
    }

}
