using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.KhachHang.Models
{
    [Table("SPYeuThich")]
    public class SanPhamYeuThichModel
    {
        [Key]
        public int Id { get; set; }

        public int MaNguoiDung { get; set; }
        public int MaSanPham { get; set; }

        public DateTime NgayThem { get; set; } = DateTime.Now;
    }
}
