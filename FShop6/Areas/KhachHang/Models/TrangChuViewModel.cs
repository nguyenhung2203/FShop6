namespace FShop6.Areas.KhachHang.Models
{
    public class TrangChuViewModel
    {
        public List<DanhMucModel> DanhMucPhoBien { get; set; }
        public List<BienTheTrangChuViewModel> SanPhamMoi { get; set; }
        public List<BienTheTrangChuViewModel> SanPhamNoiBat { get; set; }
        public List<BienTheTrangChuViewModel> SanPhamPhoBien { get; set; }
        public List<TinTucModel> TinTuc { get; set; }
        public List<DanhMucSanPhamViewModel> DanhMucSanPhamHienThi { get; set; }
    }
}
