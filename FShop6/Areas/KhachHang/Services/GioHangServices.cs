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
        //bool ThanhToan(int maNguoiDung, int maBienThe, int soLuong);
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

        //public bool ThanhToan(int maNguoiDung, List<int> maBienThe, int soLuong, bool phuongThucThanhToan)
        //{
        //    string phuongThuc = phuongThucThanhToan ? "Chuyển khoản" : "Thanh toán khi nhận hàng";
        //    try
        //    {
        //        var gioHang = _context.GioHang.Select(x => x.MaNguoiDung == maNguoiDung && x.MaBienThe == maBienThe);
        //        var nguoiDung = _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == maNguoiDung);
        //        var donHang = new DonHangModel
        //        {
        //            MaNguoiDung = maNguoiDung,
        //            DiaChiGiaoHang = nguoiDung.DiaChi,
        //            PhuongThucThanhToan = phuongThuc,
        //            TongTien = gioHang.Sum(x => x. * x.BienThe.GiaBan),

        //        };
        //    }
        //    catch (SqlException ex)
        //    {
        //        return false;
        //    }
        //}
    }
}
