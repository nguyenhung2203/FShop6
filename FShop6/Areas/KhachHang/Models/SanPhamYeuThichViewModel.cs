namespace FShop6.Areas.KhachHang.Models
{
    public class SanPhamYeuThichViewModel
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTaNgan { get; set; }
        public string HinhAnh { get; set; }
        public decimal GiaBan { get; set; }
        public bool ConHang { get; set; }
//         public int MaNguoiDung { get; set; }
//         public int MaSanPham { get; set; }
//         public DateTime NgayThem { get; set; } = DateTime.Now;
//         public ICollection<SanPhamTrangChuViewModel> BienTheSanPham { get; set; } = new List<SanPhamTrangChuViewModel>();
    }
}
