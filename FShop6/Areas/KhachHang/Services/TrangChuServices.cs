using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ITrangChuService
    {
        Task<TrangChuViewModel> LayDuLieuTrangChuDataAsync();
        Task<int> SoLuongDaBan(int maSanPham);
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
        public async Task<TrangChuViewModel> LayDuLieuTrangChuDataAsync()
        {
            // Danh mục phổ biến
            var danhMucPhoBien = await _context.ChiTietDonHang
                .Include(ct => ct.BienThe)
                    .ThenInclude(bt => bt.SanPham)
                        .ThenInclude(sp => sp.DanhMuc)
                .GroupBy(ct => ct.BienThe.SanPham.DanhMuc.TenDanhMuc)
                .OrderByDescending(g => g.Sum(x => x.SoLuong))
                .Take(6)
                .Select(g => new DanhMucModel
                {
                    Id = g.First().BienThe.SanPham.DanhMuc.Id,
                    TenDanhMuc = g.Key
                }).ToListAsync();

            // Sản phẩm nổi bật
            var sanPhamNoiBat = await _context.SanPham
                .Where(sp => sp.TrangThai == true && sp.NoiBat == true)
                .Take(12)
                .Select(sp => new SanPhamTrangChuViewModel
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    HinhAnhDaiDien = sp.HinhAnhDaiDien,
                    MoTaNgan = sp.MoTa,
                    GiaBan = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan,
                    GiamGia = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiamGia
                })
                .ToListAsync();
            foreach (var sp in sanPhamNoiBat)
            {
                sp.SoLuongDaBan = await SoLuongDaBan(sp.MaSanPham);
            }

            // Sản phẩm mới (dựa trên ngày tạo)
            var sanPhamMoi = await _context.SanPham
            .Where(sp => sp.TrangThai == true)
            .OrderByDescending(sp => sp.NgayTao)
            .Take(12)
            .Select(sp => new SanPhamTrangChuViewModel
            {
                MaSanPham = sp.MaSanPham,
                TenSanPham = sp.TenSanPham,
                HinhAnhDaiDien = sp.HinhAnhDaiDien,
                GiaBan = sp.BienThes.OrderBy(bt => bt.GiaBan).Select(bt => bt.GiaBan).FirstOrDefault(),
                MoTaNgan = sp.MoTa,
                GiamGia = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiamGia
            })
            .ToListAsync();

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
            // Sản phẩm theo danh mục
            var sanPhamTheoDanhMuc = await _context.SanPham
            .Include(sp => sp.DanhMuc)
            .GroupBy(sp => sp.DanhMuc.TenDanhMuc)
            .Take(4)
            .Select(group => new DanhMucSanPhamViewModel
            {
                TenDanhMuc = group.Key,
                SanPhams = group.Take(3).Select(sp => new SanPhamTrangChuViewModel
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    HinhAnhDaiDien = sp.HinhAnhDaiDien,
                    GiaBan = sp.BienThes.FirstOrDefault().GiaBan
                }).ToList()
            }).ToListAsync();

            // Trả về ViewModel tổng hợp
            return new TrangChuViewModel
            {
                DanhMucPhoBien = danhMucPhoBien,
                SanPhamNoiBat = sanPhamNoiBat,
                SanPhamMoi = sanPhamMoi,
                SanPhamPhoBien = sanPhamPhoBien,
                TinTuc = tinTuc,
                DanhMucSanPhamHienThi = sanPhamTheoDanhMuc,
            };
        }
    }
}