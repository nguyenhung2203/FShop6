namespace FShop6.Areas.KhachHang.Models
{
    public class BienTheModels
    {
        public int MaBienThe { get; set; }
        public int MaSanPham { get; set; }

        public string MaSKU { get; set; }
        public string LoaiBienThe { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongConLai { get; set; }
        public string TinhTrang { get; set; }

        public SanPham SanPham { get; set; }
    }

}
