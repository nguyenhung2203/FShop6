using FShop6.Areas.Admin.Models;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.Admin.Services
{
    public interface IQuanLyDonHangServices
    {
        Task<QuanLyDonHangViewModel> LayTatCaDonHangAsync();
        Task DuyetDonHang(int id);
        Task SuaDonHang(IFormCollection form);
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
                .Include(dh => dh.NguoiDung)
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.BienThe)
                .ThenInclude(bt => bt.SanPham)
                .ToListAsync();

            var dsDonHang = donHang.Select(dh => new QuanLyDonHangModel
            {
                DonHang = dh,
                HoTen = dh.NguoiDung.HoTen,
                SoDienThoai = dh.NguoiDung.SoDienThoai,
                ChiTietDonHangs = dh.ChiTietDonHangs.Select(ct => new QuanLyCTDHModel
                {
                    ChiTietDonHang = ct,
                    TenSanPham = ct.BienThe.SanPham.TenSanPham,
                    LoaiBienThe = ct.BienThe.LoaiBienThe,
                    MaSku = ct.BienThe.MaSKU
                }).ToList()
            }).ToList();

            return new QuanLyDonHangViewModel
            {
                DSDonHang = dsDonHang
            };
        }
        public async Task DuyetDonHang(int id)
        {
            try
            {
                var donHang = await _context.DonHang.FindAsync(id);
                if (donHang != null)
                {
                    donHang.TrangThai = "Đang vận chuyển";
                    donHang.NgayCapNhat = DateTime.Now;
                    _context.DonHang.Update(donHang);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                Console.WriteLine("Lỗi khi duyệt đơn hàng: " + id);
            }
        }
        public async Task SuaDonHang(IFormCollection form)
        {
            try
            {
                int ID = int.Parse(form["ID"]);
                var donHang = await _context.DonHang.FindAsync(ID);
                if (donHang != null)
                {
                    donHang.TrangThai = form["TrangThai"];
                    donHang.GhiChu = form["GhiChu"];
                    donHang.NgayCapNhat = DateTime.Now;
                    _context.DonHang.Update(donHang);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi sửa đơn hàng: " + ex.Message);
            }
        }
    }
}

