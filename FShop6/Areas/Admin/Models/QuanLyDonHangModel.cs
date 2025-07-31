namespace FShop6.Areas.Admin.Models
{
    public class QuanLyDonHangModel
    {
        public int Id { get; set; }
        public string MaDonHang { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public DateTime ThoiGianDatHang { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string GhiChu { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }


        public List<ChiTietDonHangModel> ChiTietDonHangs { get; set; } = new List<ChiTietDonHangModel>();
    }
}
