using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.Admin.Services
{
    public interface IThongKeServices
    {
        Task<ThongKeViewModel> LayThongKe();
        Task<ThongKeViewModel> LayThongKe(int month, int year);
    }
    public class ThongKeServices : IThongKeServices
    {
        private readonly AppDbContext _context;
        public ThongKeServices(AppDbContext context)
        {
            _context = context;
        }
        // Phương thức lấy tất cả thống kê (không theo tháng và năm)
        public async Task<ThongKeViewModel> LayThongKe()
        {
            var yearList = await _context.Set<DonHangModel>()
                .Select(dh => dh.ThoiGianDatHang.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToListAsync();

            var monthList = await _context.Set<DonHangModel>()
                .Select(dh => dh.ThoiGianDatHang.Month)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();

            return new ThongKeViewModel
            {
                ThongKeTheoThang = new List<ThongKeTheoThangModel>(), // Khởi tạo danh sách thống kê theo tháng rỗng
                ThongKeTheoNam = new List<ThongKeTheoNamModel>(), // Khởi tạo danh sách thống kê theo năm rỗng
                Year = yearList,
                Month = monthList
            };
        }

        // Phương thức lấy thống kê theo tháng và năm
        public async Task<ThongKeViewModel> LayThongKe(int month, int year)
        {
            var thongKeTheoNam = await _context.Set<DonHangModel>()
                .Where(dh => dh.ThoiGianDatHang.Year == year && dh.TrangThai == "Đã giao hàng") // Lọc theo năm
                .GroupBy(dh => dh.ThoiGianDatHang.Month)
                .Select(g => new ThongKeTheoNamModel
                {
                    Thang = g.Key,
                    TongDoanhThu = g.Sum(dh => dh.TongTien)
                }).ToListAsync();

            var thongKeTheoThang = await _context.Set<DonHangModel>()
                .Where(dh => dh.ThoiGianDatHang.Year == year && dh.ThoiGianDatHang.Month == month && dh.TrangThai == "Đã giao hàng") // Lọc theo tháng và năm
                .GroupBy(dh => dh.ThoiGianDatHang.Date)  // Nhóm theo ngày
                .Select(g => new ThongKeTheoThangModel
                {
                    Ngay = g.Key,
                    TongDoanhThu = g.Sum(dh => dh.TongTien),
                    TongSoDonHang = g.Count()
                }).OrderBy(dh => dh.Ngay)  // Sắp xếp theo ngày
                .ToListAsync();

            var yearList = await _context.Set<DonHangModel>()
                .Select(dh => dh.ThoiGianDatHang.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToListAsync();

            var monthList = await _context.Set<DonHangModel>()
                .Select(dh => dh.ThoiGianDatHang.Month)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();

            return new ThongKeViewModel
            {
                ThongKeTheoNam = thongKeTheoNam,
                ThongKeTheoThang = thongKeTheoThang,
                Year = yearList,
                Month = monthList
            };
        }
    }
}
