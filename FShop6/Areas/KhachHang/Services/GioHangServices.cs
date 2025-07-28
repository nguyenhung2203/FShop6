using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace FShop6.Areas.KhachHang.Services
{
    public interface IGioHang
    {
        bool ThemVaoGioHang(int maNguoiDung, int maBienThe, int soLuong);
        Task<GioHangViewModel> LayGioHang(int maNguoiDung);
    }
    public interface IThanhToan
    {
        bool ThanhToan(GioHangViewModel sanPham);
    }

    public interface IGioHangServices : IGioHang, IThanhToan
    {
        
    }
    public class GioHangServices : IGioHangServices
    {
        private readonly AppDbContext _context;
        public GioHangServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GioHangViewModel> LayGioHang(int maNguoiDung)
        {
            var gioHang = await _context.GioHang
                .Where(x => x.MaNguoiDung == maNguoiDung)
                .Include(x => x.BienThe)
                .Select(x => new GioHangItemModel
                {
                    MaBienThe = x.MaBienThe,
                    DuocChon = true,
                    TenSanPham = x.BienThe.SanPham.TenSanPham,
                    HinhAnhDaiDien = x.BienThe.SanPham.HinhAnhDaiDien,
                    GiaBan = x.BienThe.GiaBan,
                    SoLuong = x.SoLuong,
                    LoaiBienThe = x.BienThe.LoaiBienThe
                }).ToListAsync();
            return new GioHangViewModel
            {
                GioHang = gioHang
            };
        }

        public bool ThemVaoGioHang(int maNguoiDung, int maBienThe, int soLuong)
        {
            try
            {
                var gioHang = _context.GioHang.FirstOrDefault(x => x.MaNguoiDung == maNguoiDung && x.MaBienThe == maBienThe);

                if (gioHang != null)
                {
                    gioHang.SoLuong += soLuong;
                }
                else
                {
                    gioHang = new GioHangModel
                    {
                        MaNguoiDung = maNguoiDung,
                        MaBienThe = maBienThe,
                        SoLuong = soLuong,
                        NgayThem = DateTime.Now
                    };
                    _context.GioHang.Add(gioHang);
                }
                _context.SaveChanges();
                return true;
            }
            catch (SqlException ex)
            {               
                return false;
            }
        }

        public bool KiemTraGioHang(int maNguoiDung, int maBienThe)
        {
            return _context.GioHang.Any(x => x.MaNguoiDung == maNguoiDung && x.MaBienThe == maBienThe);
        }

        public bool ThanhToan(GioHangViewModel sanPham)
        {
            var gioHang = sanPham.GioHang.ToList();
            var diaChi = sanPham.DiaChi;
            string diaChiChiTiet = diaChi.CuThe;
            decimal tongTien = gioHang.Sum(x => x.GiaBan * x.SoLuong);
            var thanhToan = sanPham.PhuongThucThanhToan;
            string phuongThucThanhToan = "";
            if (thanhToan)
            {
                phuongThucThanhToan = "Chuyển khoản";
            }
            else
            {
                phuongThucThanhToan = "Thanh toán khi nhận hàng";
            }
            try
            {
                var donHang = new DonHangModel
                {
                    MaDonHang = Guid.NewGuid().ToString(),
                    MaNguoiDung = sanPham.MaNguoiDung = 5,
                    DiaChiGiaoHang = diaChiChiTiet,
                    PhuongThucThanhToan = phuongThucThanhToan,
                    TongTien = tongTien,
                    TrangThai = "Chờ xử lý",
                    ThoiGianDatHang = DateTime.Now,
                    NgayCapNhat = DateTime.Now,
                    GhiChu = sanPham.GhiChu ?? string.Empty,
                };
                _context.DonHang.Add(donHang);
                _context.SaveChanges();
                foreach (var item in gioHang)
                {
                    var chiTietDonHang = new ChiTietDonHangModel
                    {
                        IDDonHang = donHang.ID,
                        MaBienThe = item.MaBienThe,
                        SoLuong = item.SoLuong,
                        DonGia = item.GiaBan
                    };
                    _context.ChiTietDonHang.Add(chiTietDonHang);
                }
                foreach (var item in gioHang)
                {
                    var gioHangDaThanhToan = _context.GioHang.FirstOrDefault(x => x.MaBienThe == item.MaBienThe && x.MaNguoiDung == sanPham.MaNguoiDung);
                    if (gioHangDaThanhToan != null)
                        _context.GioHang.RemoveRange(gioHangDaThanhToan);
                }

                foreach (var item in gioHang)
                {
                    var bienThe = _context.BienThe.FirstOrDefault(x => x.MaBienThe == item.MaBienThe);
                    if (bienThe != null)
                    {
                        bienThe.SoLuongConLai -= item.SoLuong;
                        _context.BienThe.Update(bienThe);
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }
    }
}
