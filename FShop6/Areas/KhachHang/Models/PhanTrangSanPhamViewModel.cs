namespace FShop6.Areas.KhachHang.Models
{
    public class PhanTrangSanPhamViewModel
    {
        public List<BienTheTrangChuViewModel> DanhSachSanPham { get; set; } = new List<BienTheTrangChuViewModel>();
        public List<DanhMucModel> DanhSachDanhMuc { get; set; } = new List<DanhMucModel>();
        public int TrangHienTai { get; set; }
        public int TongSoTrang { get; set; }
    }
}
