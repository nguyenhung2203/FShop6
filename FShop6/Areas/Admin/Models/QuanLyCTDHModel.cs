using FShop6.Areas.KhachHang.Models;

namespace FShop6.Areas.Admin.Models
{
    public class QuanLyCTDHModel
    {
        public ChiTietDonHangModel ChiTietDonHang { get; set; } = new ChiTietDonHangModel();
        public string TenSanPham { get; set; }
        public string LoaiBienThe { get; set; }
        public string MaSku { get; set; }
    }
}
