namespace FShop6.Areas.KhachHang.Models
{
    public class SanPhamTrangChuViewModel
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnhDaiDien { get; set; }
        public decimal GiaBan { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTaNgan { get; set; }
        public byte? GiamGia { get; set; } = 0;
        public int SoLuongDaBan { get; set; }
        public bool TinhTrangYeuThich { get; set; } = false;
    }
}
