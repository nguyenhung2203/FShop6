namespace FShop6.Areas.KhachHang.Models
{
    public class PhanTrangSanPhamViewModel
    {
        public List<SanPhamTrangChuViewModel> DanhSachSanPham { get; set; } = new List<SanPhamTrangChuViewModel>();
        public List<DanhMucModel> DanhSachDanhMuc { get; set; } = new List<DanhMucModel>();
        public int TrangHienTai { get; set; }
        public int TongSoTrang { get; set; }
    }
}
