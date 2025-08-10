using FShop6.Areas.KhachHang.Models;
using System.Collections.Generic;

namespace FShop6.Areas.Admin.Models
{
    public class NguoiDungViewModel
    {
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string MatKhau { get; set; }       // ✅ Thêm mật khẩu
        public string TTHoatDong { get; set; }
        public string DiaChi { get; set; }

        public int SoLuongDonDat { get; set; }
        public decimal TongTienDaDat { get; set; }
        public List<DonHangModel> DonHang { get; set; }
    }
}
