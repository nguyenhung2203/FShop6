namespace FShop6.Areas.KhachHang.Models
{
    public class GioHangItemModel
    {
        public int MaBienThe { get; set; }
        public bool DuocChon { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnhDaiDien { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public string LoaiBienThe { get; set; }
    }

    public class DiaChiViewModel
    {
        public string Tinh { get; set; }      // Code tỉnh
        public string TenTinh { get; set; }   // Tên tỉnh
        public string Huyen { get; set; }     // Code huyện  
        public string TenHuyen { get; set; }  // Tên huyện
        public string Xa { get; set; }        // Code xã
        public string TenXa { get; set; }     // Tên xã
        public string CuThe { get; set; }     // Địa chỉ cụ thể
    }
}
