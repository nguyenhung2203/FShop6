namespace FShop6.Areas.KhachHang.Models
{
    public class TrangChuViewModel
    {
        public List<DanhMucModel> DanhMucPhoBien { get; set; }
        public List<SanPhamTrangChuViewModel> SanPhamMoi { get; set; }
        public List<SanPhamTrangChuViewModel> SanPhamNoiBat { get; set; }
        public List<SanPhamTrangChuViewModel> SanPhamPhoBien { get; set; }
        public List<TinTucModel> TinTuc { get; set; }
        public List<DanhMucSanPhamViewModel> DanhMucSanPhamHienThi { get; set; }
    }
}
