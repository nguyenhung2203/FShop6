namespace FShop6.Areas.KhachHang.Models
{
    public class BienTheViewModel
    {
        // Từ bảng BienThe
        public int MaBienThe { get; set; }
        public string MaSKU { get; set; }
        public string LoaiBienThe { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongConLai { get; set; }
        public string TinhTrang { get; set; }

        // Từ bảng SanPham
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public string HinhAnhDaiDien { get; set; }

        // Từ bảng DanhMuc
        public string TenDanhMuc { get; set; }
    }
}
