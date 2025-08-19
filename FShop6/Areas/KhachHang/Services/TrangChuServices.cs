using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ITrangChuService
    {
        Task<TrangChuViewModel> LayDuLieuTrangChuDataAsync(int maNguoiDung);
        Task<int> SoLuongDaBan(int maSanPham);
        Task<bool> TinhTrangYeuThich(int maSanPham, int maNguoiDung);
    }

    public class TrangChuServices : ITrangChuService
    {
        private readonly AppDbContext _context;
        public TrangChuServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> SoLuongDaBan(int maSanPham)
        {
            return await _context.ChiTietDonHang
                .Where(ct => ct.BienThe.SanPham.MaSanPham == maSanPham && ct.DonHang.TrangThai == "Đã giao")
                .SumAsync(ct => ct.SoLuong);
        }
        public async Task<bool> TinhTrangYeuThich(int maSanPham, int maNguoiDung)
        {
            return await _context.SPYeuThich
                .AnyAsync(yt => yt.MaSanPham == maSanPham && yt.MaNguoiDung == maNguoiDung);
        }
        public async Task<TrangChuViewModel> LayDuLieuTrangChuDataAsync(int maNguoiDung)
        {
            // Danh mục phổ biến
            var danhMucPhoBien = await _context.ChiTietDonHang
                .Include(ct => ct.BienThe)
                    .ThenInclude(bt => bt.SanPham)
                        .ThenInclude(sp => sp.DanhMuc)
                .Where(ct => ct.BienThe.SanPham.TrangThai == true)
                .GroupBy(ct => new {
                    DanhMucId = ct.BienThe.SanPham.DanhMuc.Id,
                    TenDanhMuc = ct.BienThe.SanPham.DanhMuc.TenDanhMuc
                })
                .OrderByDescending(g => g.Sum(x => x.SoLuong))
                .Take(6)
                .Select(g => new DanhMucModel
                {
                    Id = g.Key.DanhMucId,
                    TenDanhMuc = g.Key.TenDanhMuc
                })
                .ToListAsync();

            // Sản phẩm nổi bật
            var sanPhamNoiBat = await _context.SanPham
                .Where(sp => sp.TrangThai == true && sp.NoiBat == true && sp.BienThes.Any())
                .Take(12)
                .Select(sp => new SanPhamTrangChuViewModel
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    HinhAnhDaiDien = sp.HinhAnhDaiDien,
                    MoTaNgan = sp.MoTa,
                    GiaBan = sp.BienThes.OrderByDescending(bt => bt.GiaBan).First().GiaBan,
                    GiamGia = sp.BienThes.OrderByDescending(bt => bt.GiamGia).First().GiamGia,
                })
                .ToListAsync();
            foreach (var sp in sanPhamNoiBat)
            {
                sp.SoLuongDaBan = await SoLuongDaBan(sp.MaSanPham);
                sp.TinhTrangYeuThich = await TinhTrangYeuThich(sp.MaSanPham, maNguoiDung);
            }

            // Sản phẩm mới (dựa trên ngày tạo)
            var sanPhamMoi = await _context.SanPham
            .Where(sp => sp.TrangThai == true)
            .OrderByDescending(sp => sp.NgayTao)
            .Take(4)
            .Select(sp => new SanPhamTrangChuViewModel
            {
                MaSanPham = sp.MaSanPham,
                TenSanPham = sp.TenSanPham,
                HinhAnhDaiDien = sp.HinhAnhDaiDien,
                GiaBan = sp.BienThes.OrderByDescending(bt => bt.GiaBan).Select(bt => bt.GiaBan).FirstOrDefault(),
                MoTaNgan = sp.MoTa,
                GiamGia = sp.BienThes.OrderByDescending(bt => bt.GiamGia).FirstOrDefault().GiamGia
            })
            .ToListAsync();
            foreach (var sp in sanPhamMoi)
            {
                sp.TinhTrangYeuThich = await TinhTrangYeuThich(sp.MaSanPham, maNguoiDung);
            }

            // Sản phẩm phổ biến (dựa trên số lượng đã bán)
            var sanPhamPhoBien = await _context.ChiTietDonHang
                .Include(ct => ct.BienThe)
                    .ThenInclude(bt => bt.SanPham)
                 .Where(g => g.BienThe.SanPham.TrangThai == true)
                .GroupBy(ct => ct.BienThe.MaSanPham)
                .OrderByDescending(g => g.Sum(x => x.SoLuong))
                .Take(12)
                .Select(g => new SanPhamTrangChuViewModel
                {
                    MaSanPham = g.First().BienThe.SanPham.MaSanPham,
                    TenSanPham = g.First().BienThe.SanPham.TenSanPham,
                    HinhAnhDaiDien = g.First().BienThe.SanPham.HinhAnhDaiDien,
                    GiaBan = g.First().BienThe.GiaBan,
                    MoTaNgan = g.First().BienThe.SanPham.MoTa,
                    GiamGia = g.First().BienThe.GiamGia
                }).ToListAsync();
            foreach (var sp in sanPhamPhoBien)
            {
                sp.SoLuongDaBan = await SoLuongDaBan(sp.MaSanPham);
                sp.TinhTrangYeuThich = await TinhTrangYeuThich(sp.MaSanPham, maNguoiDung);
            }
            //Tin tức
            var tinTuc = await _context.TinTuc
                .OrderByDescending(t => t.ThoiGianTao)
                .Take(3)
                .Select(t => new TinTucModel
                {
                    MaTinTuc = t.MaTinTuc,
                    TieuDe = t.TieuDe,
                    MoTaNgan = t.MoTaNgan,
                    NoiDung = t.NoiDung,
                    ThoiGianTao = t.ThoiGianTao
                }).ToListAsync();

            // Trả về ViewModel tổng hợp
            return new TrangChuViewModel
            {
                DanhMucPhoBien = danhMucPhoBien,
                SanPhamNoiBat = sanPhamNoiBat,
                SanPhamMoi = sanPhamMoi,
                SanPhamPhoBien = sanPhamPhoBien,
                TinTuc = tinTuc
            };
        }
    }
}