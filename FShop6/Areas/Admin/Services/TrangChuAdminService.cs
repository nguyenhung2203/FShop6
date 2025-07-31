using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FShop6.Areas.Admin.Services
{
    public class TrangChuAdminService : ITrangChuAdminServices
    {
        private readonly AppDbContext _context;

        public TrangChuAdminService(AppDbContext context)
        {
            _context = context;
        }

        public TrangChuAdminViewModel LayThongTinTrangChu()
        {
            var model = new TrangChuAdminViewModel();

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // 1. Doanh số bán (2 tuần gần nhất)
            DateTime twoWeeksAgo = DateTime.Now.AddDays(-14);
            model.DoanhSoBan = _context.ChiTietDonHang
                .Include(c => c.DonHang)
                .Where(c => c.DonHang.ThoiGianDatHang >= twoWeeksAgo)
                .Sum(c => c.SoLuong);

            // 2. Bán chạy nhất
            var bestSeller = _context.ChiTietDonHang
                .Include(c => c.BienThe)
                .ThenInclude(bt => bt.SanPham)
                .GroupBy(c => c.MaBienThe)
                .Select(g => new
                {
                    MaBienThe = g.Key,
                    TongSoLuong = g.Sum(c => c.SoLuong),
                    TenSanPham = g.First().BienThe.SanPham.TenSanPham
                })
                .OrderByDescending(x => x.TongSoLuong)
                .FirstOrDefault();

            model.BanChayNhat = bestSeller?.TongSoLuong ?? 0;
            model.TenBanChayNhat = bestSeller?.TenSanPham ?? "-";

            // 3. Tổng số sản phẩm
            model.TongSanPham = _context.SanPham.Count();

            // 4. Doanh thu theo tháng (năm hiện tại)
            model.DoanhThuTheoThang = _context.ChiTietDonHang
                .Include(c => c.DonHang)
                .Where(c => c.DonHang.ThoiGianDatHang.Year == currentYear)
                .GroupBy(c => c.DonHang.ThoiGianDatHang.Month)
                .Select(g => new DoanhThuThangModel
                {
                    Thang = g.Key,
                    TongTien = g.Sum(x => x.SoLuong * x.DonGia)
                })
                .OrderBy(x => x.Thang)
                .ToList();

            // 5. Tỷ lệ tăng trưởng doanh thu (so với tháng trước)
            var doanhThu = _context.ChiTietDonHang
    .Include(c => c.DonHang)
    .Where(c => c.DonHang != null && c.DonHang.ThoiGianDatHang.Year == currentYear)
    .GroupBy(c => c.DonHang.ThoiGianDatHang.Month)
    .Select(g => new DoanhThuThangModel
    {
        Thang = g.Key,
        TongTien = g.Sum(x => x.SoLuong * x.DonGia)
    })
    .ToList();

            // Bổ sung các tháng không có đơn hàng
            for (int i = 1; i <= 12; i++)
            {
                if (!doanhThu.Any(x => x.Thang == i))
                {
                    doanhThu.Add(new DoanhThuThangModel
                    {
                        Thang = i,
                        TongTien = 0
                    });
                }
            }

            // Sắp xếp lại
            model.DoanhThuTheoThang = doanhThu.OrderBy(x => x.Thang).ToList();
            // 5. Tỷ lệ tăng trưởng doanh thu (so với tháng trước)
            var doanhThuThangNay = doanhThu.FirstOrDefault(x => x.Thang == currentMonth)?.TongTien ?? 0;
            var doanhThuThangTruoc = doanhThu.FirstOrDefault(x => x.Thang == currentMonth - 1)?.TongTien ?? 0;

            if (doanhThuThangTruoc > 0)
            {
                model.TyLeTangTruong = (float)((doanhThuThangNay - doanhThuThangTruoc) / doanhThuThangTruoc * 100);
            }
            else
            {
                model.TyLeTangTruong = 0;
            }
            // 6. Khách hàng mới tháng này
            model.DanhSachKhachHangMoi = _context.NguoiDung
                .Where(nd => nd.ThoiGianTao.Month == currentMonth
                          && nd.ThoiGianTao.Year == currentYear
                          && nd.TenVaiTro.Trim().ToLower() == "Khách hàng")
                .Select(nd => new NguoiDungModel // hoặc KhachHangMoiViewModel nếu dùng ViewModel riêng
                {
                    HoTen = nd.HoTen,
                    Email = nd.Email,
                    ThoiGianTao = nd.ThoiGianTao
                }).ToList();

            return model;
        }
    }
}
