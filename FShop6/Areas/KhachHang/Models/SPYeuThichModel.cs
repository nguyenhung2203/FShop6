using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.KhachHang.Models
{
    [Table("SPYeuThich")]
    public class SPYeuThichModel
    {
        [Key]
        public int Id { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaSanPham { get; set; }
        public DateTime NgayThem { get; set; } = DateTime.Now;
        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDungModel NguoiDung { get; set; }
        [ForeignKey("MaSanPham")]
        public virtual SanPhamModel SanPham { get; set; }

    }
}
