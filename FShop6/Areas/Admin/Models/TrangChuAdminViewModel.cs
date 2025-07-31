using System;
using System.Collections.Generic;
using FShop6.Areas.KhachHang.Models;

namespace FShop6.Areas.Admin.Models
{
    public class DoanhThuThangModel
    {
        public int Thang { get; set; }
        public decimal TongTien { get; set; }
    }

    public class TrangChuAdminViewModel
    {
        public int DoanhSoBan { get; set; }
        public int BanChayNhat { get; set; }
        public string TenBanChayNhat { get; set; }
        public int TongSanPham { get; set; }
        public float TyLeTangTruong { get; set; }
        public List<DoanhThuThangModel> DoanhThuTheoThang { get; set; }
        public int KhachHangMoi { get; set; }
        public List<NguoiDungModel> DanhSachKhachHangMoi { get; set; }
    }
}
