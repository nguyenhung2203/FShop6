namespace FShop6.Areas.KhachHang.Models
{
    public class BienTheChiTietViewModel
    {
        public List<AnhBienTheModel> AnhBienThe { get; set; } = new List<AnhBienTheModel>();
        public List<BienTheChiTietModel> BienTheChiTiet { get; set; } = new List<BienTheChiTietModel>();
        public List<DanhMucModel> DanhMuc { get; set; } = new List<DanhMucModel>();
        public List<SanPhamModel> SanPham { get; set; } = new List<SanPhamModel>();
    }
}
