using FShop6.Areas.Admin.Models;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.Admin.Services
{
    public interface IQuanLyDonHangServices
    {
        Task<QuanLyDonHangViewModel> LayTatCaDonHangAsync();
    }

    public class QuanLyDonHangServices : IQuanLyDonHangServices
    {
        private readonly AppDbContext _context;
        public QuanLyDonHangServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuanLyDonHangViewModel> LayTatCaDonHangAsync()
        {
            var donHang = await _context.DonHang
                .Where(dh => dh.NguoiDung.TenVaiTro == "Khách hàng")
                .Include(dh => dh.NguoiDung)
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.BienThe)
                .ThenInclude(bt => bt.SanPham)
                .ToListAsync();

            var dsDonHang = donHang.Select(dh => new QuanLyDonHangModel
            {
                Id = dh.ID,
                MaDonHang = dh.MaDonHang,
                DiaChiGiaoHang = dh.DiaChiGiaoHang,
                TongTien = dh.TongTien,
                TrangThai = dh.TrangThai,
                PhuongThucThanhToan = dh.PhuongThucThanhToan,
                ThoiGianDatHang = dh.ThoiGianDatHang,
                NgayCapNhat = dh.NgayCapNhat,
                GhiChu = dh.GhiChu,
                TenKhachHang = dh.NguoiDung.HoTen,
                SoDienThoai = dh.NguoiDung.SoDienThoai,
                ChiTietDonHangs = dh.ChiTietDonHangs.Select(ct => new ChiTietDonHangModel
                {
                    MaSanPham = ct.BienThe.MaSanPham,
                    TenSanPham = ct.BienThe.SanPham.TenSanPham,
                    GiaBan = ct.BienThe.GiaBan,
                    LoaiBienThe = ct.BienThe.LoaiBienThe,
                    SoLuong = ct.SoLuong
                }).ToList()
            }).ToList();

            return new QuanLyDonHangViewModel
            {
                DSDonHang = dsDonHang
            };
        }
    }
}

