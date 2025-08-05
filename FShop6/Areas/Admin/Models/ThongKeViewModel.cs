namespace FShop6.Areas.Admin.Models
{
    public class ThongKeViewModel
    {
        public int TongSoSanPham { get; set; }
        public int TongSoSanPhamSapHet { get; set; }

        public List<string> nhanHomNay { get; set; }
        public List<long> doanhThuHomNay { get; set; }
        public List<SanPhamBanChayVM> sanPhamBanChayNgay { get; set; }
        public List<SanPhamBanChamVM> SanPhamBanChamNgay { get; set; }
        public int TongSoDonHangNgay { get; set; }
        public int TongSoKhachHangNgay { get; set; }

        public List<string> nhanTuan { get; set; }
        public List<long> doanhThuTuan { get; set; }
        public List<SanPhamBanChayVM> sanPhamBanChayTuan { get; set; }
        public List<SanPhamBanChamVM> SanPhamBanChamTuan { get; set; }
        public int TongSoDonHangTuan { get; set; }
        public int TongSoKhachHangTuan { get; set; }

        public List<string> nhanThang { get; set; }
        public List<long> doanhThuThang { get; set; }
        public List<SanPhamBanChayVM> sanPhamBanChayThang { get; set; }
        public List<SanPhamBanChamVM> SanPhamBanChamThang { get; set; }
        public int TongSoDonHangThang { get; set; }
        public int TongSoKhachHangThang { get; set; }

        public List<string> nhanNam { get; set; }
        public List<long> doanhThuNam { get; set; }
        public List<SanPhamBanChayVM> sanPhamBanChayNam { get; set; }
        public List<SanPhamBanChamVM> SanPhamBanChamNam { get; set; }
        public int TongSoDonHangNam { get; set; }
        public int TongSoKhachHangNam { get; set; }

        public int[] TrangThaiNgay { get; set; }
        public int[] TrangThaiTuan { get; set; }
        public int[] TrangThaiThang { get; set; }
        public int[] TrangThaiNam { get; set; }
    }

    public class SanPhamBanChayVM
    {
        public string Ten { get; set; }
        public int SoLuongBan { get; set; }
        public long DoanhThu { get; set; }
    }

    public class SanPhamBanChamVM
    {
        public string Ten { get; set; }
        public int SoLuongBan { get; set; }
        public long SoLuongConLai { get; set; }
    }
}
