using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.KhachHang.Models
{
    public class BienTheChiTietModel
    {
        public int MaBienThe { get; set; }
        public int MaSanPham { get; set; }
        public decimal GiaBan { get; set; }
        public string MaSKU { get; set; }
        public string LoaiBienThe { get; set; }
        public int SoLuong { get; set; }

        public ICollection<AnhBienTheModel> AnhBienThe { get; set; } = new List<AnhBienTheModel>();
        [ForeignKey("MaSanPham")]
        public SanPhamModel SanPham { get; set; }
    }
}
