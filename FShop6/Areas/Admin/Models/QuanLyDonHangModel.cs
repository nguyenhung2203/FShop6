using FShop6.Areas.KhachHang.Models;

namespace FShop6.Areas.Admin.Models
{
    public class QuanLyDonHangModel
    {
        public DonHangModel DonHang { get; set; } = new DonHangModel();
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }


        public List<QuanLyCTDHModel> ChiTietDonHangs { get; set; } = new List<QuanLyCTDHModel>();
    }
}
