namespace FShop6.Areas.KhachHang.Models
{
    public class TrangChuViewModel
    {
        public List<DanhMuc> DanhMucPhoBien { get; set; }
        public List<SanPham> SanPhamMoi { get; set; }
        public List<SanPham> SanPhamNoiBat { get; set; }
        public List<SanPham> SanPhamPhoBien { get; set; }
        public List<TinTuc> TinTuc { get; set; }
        public List<DanhMucSanPhamViewModel> DanhMucSanPhamHienThi { get; set; }
    }
}
