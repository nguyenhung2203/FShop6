using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.Admin.Models
{
    public class GioHangModel
    {
        [Key]
        public int Id { get; set; } 

        public int MaNguoiDung { get; set; }
        public int MaBienThe { get; set; }
        [Required]
        public int SoLuong { get; set; }
        public DateTime NgayThem { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MaNguoiDung")]
        public NguoiDungModel NguoiDung { get; set; }

        [ForeignKey("MaBienThe")]
        public BienTheModels BienThe { get; set; }
    }
}
