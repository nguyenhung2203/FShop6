namespace FShop6.Areas.KhachHang.Models
{
    public class SanPhamYeuThichViewModel
    {
        public int MaNguoiDung { get; set; }
        public int MaSanPham { get; set; }
        public DateTime NgayThem { get; set; } = DateTime.Now;
        public ICollection<SanPhamTrangChuViewModel> BienTheSanPham { get; set; } = new List<SanPhamTrangChuViewModel>();
    }
}
