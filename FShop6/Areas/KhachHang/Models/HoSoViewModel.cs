namespace FShop6.Areas.KhachHang.Models
{
    public class HoSoViewModel
    {
        public NguoiDungModel nguoiDungModels { get; set; }
        public List<DonHangModel> donHangModels { get; set; } = new List<DonHangModel>();
        public List<ChiTietDonHangViewModel> ChiTietDonHang { get; set; } = new List<ChiTietDonHangViewModel>();
        public int TrangHienTai { get; set; }
        public int TongSoTrang { get; set; }
    }
}
