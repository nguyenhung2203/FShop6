namespace FShop6.Areas.KhachHang.Models
{
    public class SanPham
    {
        public int MaSanPham { get; set; }
        public int MaDanhMucSP { get; set; }

        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public string HinhAnhDaiDien { get; set; }

        public DanhMuc DanhMuc { get; set; }
        public ICollection<BienTheModels> BienThes { get; set; }
    }

}
