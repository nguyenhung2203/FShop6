namespace FShop6.Areas.KhachHang.Models
{
    public class DanhMuc
    {
        public int Id { get; set; }
        public string TenDanhMuc { get; set; }

        public ICollection<SanPham> SanPhams { get; set; }
    }
}
