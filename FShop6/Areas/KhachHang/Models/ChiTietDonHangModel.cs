using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.KhachHang.Models
{
    public class ChiTietDonHangModel
    {
        [Key]
        public int MaChiTietDH { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DonGia { get; set; }

        
        // Khóa ngoại đến DonHang
        public int? DonHang_ID { get; set; }

        [ForeignKey("DonHang_ID")]
        public DonHangModel DonHang { get; set; }

        // Khóa ngoại đến BienThe
        public int? MaBienThe { get; set; }

        [ForeignKey("MaBienThe")]
        public BienTheModels BienThe { get; set; }
    }
}
