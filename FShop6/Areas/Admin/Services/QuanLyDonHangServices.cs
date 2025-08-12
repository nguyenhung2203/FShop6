using FShop6.Areas.Admin.Models;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.Admin.Services
{
    public interface IQuanLyDonHangServices
    {
        Task<QuanLyDonHangViewModel> LayTatCaDonHangAsync(string? tuKhoa, DateTime? tuNgay, DateTime? denNgay);
        Task<bool> DuyetDonHang(int id);
        Task<bool> SuaDonHang(IFormCollection form);
    }

    public class QuanLyDonHangServices : IQuanLyDonHangServices
    {
        private readonly AppDbContext _context;
        public QuanLyDonHangServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuanLyDonHangViewModel> LayTatCaDonHangAsync(string? tuKhoa, DateTime? tuNgay, DateTime? denNgay)
        {
            var query = _context.DonHang
            .Include(dh => dh.NguoiDung)
            .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.BienThe)
                .ThenInclude(bt => bt.SanPham)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                query = query.Where(dh =>
                    dh.NguoiDung.HoTen.Contains(tuKhoa) ||
                    dh.NguoiDung.SoDienThoai.Contains(tuKhoa) ||
                    dh.MaDonHang.ToString().Contains(tuKhoa)
                );
            }

            // Nếu cả hai ngày có giá trị và từ ngày > đến ngày => đổi chỗ cho đúng
            if (tuNgay.HasValue && denNgay.HasValue && tuNgay > denNgay)
            {
                var tmp = tuNgay;
                tuNgay = denNgay;
                denNgay = tmp;
            }

            // lọc theo ngày đặt (ThoiGianDatHang)
            if (tuNgay.HasValue)
                query = query.Where(dh => dh.ThoiGianDatHang >= tuNgay.Value.Date);

            if (denNgay.HasValue)
            {
                var denNgayInclusive = denNgay.Value.Date.AddDays(1); // bao gồm cả cuối ngày
                query = query.Where(dh => dh.ThoiGianDatHang < denNgayInclusive);
            }
            var donHang = await query
            .OrderByDescending(dh => dh.ThoiGianDatHang)
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
        public async Task<bool> DuyetDonHang(int id)
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

                    return true;
                }

                return false; // Không tìm thấy đơn hàng
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi duyệt đơn hàng: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> SuaDonHang(IFormCollection form)
        {
            try
            {
                int ID = int.Parse(form["ID"]);
                var donHang = await _context.DonHang.FindAsync(ID);

                if (donHang == null)
                    return false;
                var trangThai = form["TrangThai"];
                if (trangThai == "Đã hủy")
                {
                    var chiTietDonHang = await _context.ChiTietDonHang
                        .Where(ct => ct.IDDonHang == ID)
                        .ToListAsync();

                    foreach (var ct in chiTietDonHang)
                    {
                        var bienThe = await _context.BienThe.FindAsync(ct.MaBienThe);
                        if (bienThe != null)
                        {
                            bienThe.SoLuongConLai += ct.SoLuong;
                            _context.BienThe.Update(bienThe);
                        }
                    }
                }
                donHang.TrangThai = trangThai;
                donHang.GhiChu = form["GhiChu"];
                donHang.NgayCapNhat = DateTime.Now;
                _context.DonHang.Update(donHang);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi sửa đơn hàng: " + ex.Message);
                return false;
            }
        }
    }
}

