namespace FShop6.Areas.KhachHang.Models
{
    public class GioHangItemModel
    {
        public int MaBienThe { get; set; }
        public bool DuocChon { get; set; }
        public string? TenSanPham { get; set; }
        public string? HinhAnhDaiDien { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public string? LoaiBienThe { get; set; }
        public int SoLuongTon { get; set; }
    }

    public class DiaChiViewModel
    {
        public string Tinh { get; set; }
        public string Huyen { get; set; }
        public string Xa { get; set; }
        public string CuThe { get; set; }
    }
}
