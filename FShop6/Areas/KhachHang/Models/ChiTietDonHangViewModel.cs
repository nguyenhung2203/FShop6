namespace FShop6.Areas.KhachHang.Models
{
    public class ChiTietDonHangViewModel
    {
        public int MaDonHang { get; set; }
        public string AnhSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public string? LoaiBienThe { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
    }
}
