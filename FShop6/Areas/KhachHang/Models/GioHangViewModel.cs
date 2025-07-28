namespace FShop6.Areas.KhachHang.Models
{
    public class GioHangViewModel
    {
        public List<GioHangItemModel> GioHang{ get; set; } = new List<GioHangItemModel>();
        public DiaChiViewModel DiaChi { get; set; }
        public int? MaNguoiDung { get; set; } = 5; // Mặc định cho người dùng đã đăng nhập
        public string? GhiChu { get; set; }
        public bool PhuongThucThanhToan { get; set; } // true: Chuyển khoản, false: Thanh toán khi nhận hàng
    }
}
