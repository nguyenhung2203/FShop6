using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.KhachHang.Models
{
    [Table("ChiTietDonHang")]
    public class ChiTietDonHangModel
    {
        [Key]
        public int MaChiTietDH { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }
        [Column("DonHang_ID")]
        public int? IDDonHang { get; set; }
        [ForeignKey("IDDonHang")]
        public virtual DonHangModel DonHang { get; set; }

        // Khóa ngoại đến BienThe
        public int? MaBienThe { get; set; }

        [ForeignKey("MaBienThe")]
        public virtual BienTheModels BienThe { get; set; }
    }
}
