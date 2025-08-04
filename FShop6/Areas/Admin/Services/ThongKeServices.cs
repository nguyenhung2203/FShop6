using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace FShop6.Areas.Admin.Services
{
    public interface IThongKeServices
    {
        Task<ThongKeViewModel> LayThongKeAsync();
    }
    public class ThongKeServices : IThongKeServices
    {
        private readonly AppDbContext _context;
        public ThongKeServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ThongKeViewModel> LayThongKeAsync()
        {
            var homNay = DateTime.Today;
            var dauTuan = homNay.AddDays(-((int)(homNay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)homNay.DayOfWeek) - 1));
            var dauThang = new DateTime(homNay.Year, homNay.Month, 1);
            var dauNam = new DateTime(homNay.Year, 1, 1);

            int tongSoSanPham = await _context.SanPham.CountAsync();
            int tongSoSanPhamSapHet = await _context.BienThe
                                        .Where(bt => bt.SoLuongConLai <= 5)
                                        .CountAsync();


            List<string> nhanHomNay = new List<string>();
            List<long> doanhThuHomNay = new List<long>();
            for (int i = 0; i < 7; i++)
            {
                int gioBatDau = 6 + i * 3;
                DateTime batDau = homNay.AddHours(gioBatDau);
                DateTime ketThuc = batDau.AddHours(3);

                var doanhThu = await _context.DonHang
                    .Where(dh => (dh.ThoiGianDatHang >= batDau && dh.ThoiGianDatHang < ketThuc) && dh.TrangThai == "Đã giao")
                    .SumAsync(dh => dh.TongTien);

                nhanHomNay.Add($"{gioBatDau}h");
                doanhThuHomNay.Add((long)doanhThu);
            }
            List<SanPhamBanChayVM> sanPhamBanChayNgay = await _context.ChiTietDonHang
                .Where(ct => ct.DonHang.ThoiGianDatHang.Date == homNay.Date && ct.DonHang.TrangThai == "Đã giao")
                .GroupBy(ct => ct.BienThe.SanPham.TenSanPham)
                .Select(g => new SanPhamBanChayVM
                {
                    Ten = g.Key,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    DoanhThu = (long)g.Sum(ct => ct.BienThe.GiaBan * ct.SoLuong)
                })
                .OrderByDescending(sp => sp.DoanhThu)
                .Take(5)
                .ToListAsync();
            List<SanPhamBanChamVM> sanPhamBanChayCham = await _context.ChiTietDonHang
                .Where(ct => ct.DonHang.ThoiGianDatHang.Date == homNay.Date && ct.DonHang.TrangThai == "Đã giao")
                .GroupBy(ct => ct.BienThe.SanPham.TenSanPham)
                .Select(g => new SanPhamBanChamVM
                {
                    Ten = g.Key,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    SoLuongConLai = g.Sum(ct => ct.BienThe.SoLuongConLai)
                })
                .OrderBy(sp => sp.SoLuongBan)
                .Take(5)
                .ToListAsync();
            int tongSoDonHangNgay = await _context.DonHang
                .CountAsync(dh => dh.ThoiGianDatHang.Date == homNay.Date);
            int tongSoKhachHangNgay = await _context.NguoiDung
                .Where(nd => nd.ThoiGianTao.Date == homNay.Date)
                .Distinct()
                .CountAsync();
            var trangThaiDonHangNgay = new int[]
            {
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang.Date == homNay.Date && dh.TrangThai == "Đã giao"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang.Date == homNay.Date && dh.TrangThai == "Đang vận chuyển"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang.Date == homNay.Date && dh.TrangThai == "Chờ xác nhận"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang.Date == homNay.Date && dh.TrangThai == "Đã hủy")
            };





            List<string> nhanTuan = new List<string>();
            List<long> doanhThuTuan = new List<long>();
            for (int i = 0; i < 7; i++)
            {
                DateTime ngay = dauTuan.AddDays(i);
                var doanhThu = await _context.DonHang
                    .Where(dh => dh.ThoiGianDatHang.Date == ngay.Date && dh.TrangThai == "Đã giao")
                    .SumAsync(dh => (decimal?)dh.TongTien ?? 0);
                if (i == 6)
                {
                    nhanTuan.Add($"Chủ nhật");
                }
                else
                {
                    nhanTuan.Add($"Thứ {i + 2}");
                }
                doanhThuTuan.Add((long)doanhThu);
                Console.WriteLine($"Ngày: {ngay.ToShortDateString()}, Doanh thu: {doanhThu}");
            }
            List<SanPhamBanChayVM> sanPhamBanChayTuan = await _context.ChiTietDonHang
                .Where(ct => ct.DonHang.ThoiGianDatHang >= dauTuan && ct.DonHang.ThoiGianDatHang < dauTuan.AddDays(7) && ct.DonHang.TrangThai == "Đã giao")
                .GroupBy(ct => ct.BienThe.SanPham.TenSanPham)
                .Select(g => new SanPhamBanChayVM
                {
                    Ten = g.Key,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    DoanhThu = (long)g.Sum(ct => ct.BienThe.GiaBan * ct.SoLuong)
                })
                .OrderByDescending(sp => sp.DoanhThu)
                .Take(5)
                .ToListAsync();
            List<SanPhamBanChamVM> sanPhamBanChamTuan = await _context.ChiTietDonHang
                .Where(ct => ct.DonHang.ThoiGianDatHang >= dauTuan && ct.DonHang.ThoiGianDatHang < dauTuan.AddDays(7) && ct.DonHang.TrangThai == "Đã giao")
                .GroupBy(ct => ct.BienThe.SanPham.TenSanPham)
                .Select(g => new SanPhamBanChamVM
                {
                    Ten = g.Key,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    SoLuongConLai = g.Sum(ct => ct.BienThe.SoLuongConLai)
                })
                .OrderBy(sp => sp.SoLuongBan)
                .Take(5)
                .ToListAsync();
            int tongSoDonHangTuan = await _context.DonHang
                .CountAsync(dh => dh.ThoiGianDatHang >= dauTuan && dh.ThoiGianDatHang < dauTuan.AddDays(7) );
            int tongSoKhachHangTuan = await _context.NguoiDung
                .Where(nd => nd.ThoiGianTao >= dauTuan && nd.ThoiGianTao < dauTuan.AddDays(7))
                .Distinct()
                .CountAsync();
            var trangThaiDonHangTuan = new int[]
            {
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauTuan && dh.ThoiGianDatHang < dauTuan.AddDays(7) && dh.TrangThai == "Đã giao"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauTuan && dh.ThoiGianDatHang < dauTuan.AddDays(7) && dh.TrangThai == "Đang vận chuyển"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauTuan && dh.ThoiGianDatHang < dauTuan.AddDays(7) && dh.TrangThai == "Chờ xác nhận"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauTuan && dh.ThoiGianDatHang < dauTuan.AddDays(7) && dh.TrangThai == "Đã hủy")
            };


            List<string> nhanThang = new List<string>();
            List<long> doanhThuThang = new List<long>();
            int soNgayTrongThang = DateTime.DaysInMonth(homNay.Year, homNay.Month);
            for (int i = 1; i <= soNgayTrongThang; i++)
            {
                DateTime ngay = new DateTime(homNay.Year, homNay.Month, i);
                var doanhThu = await _context.DonHang
                    .Where(dh => dh.ThoiGianDatHang.Date == ngay.Date && dh.TrangThai == "Đã giao")
                    .SumAsync(dh => (decimal?)dh.TongTien ?? 0);
                nhanThang.Add($"{i}");
                doanhThuThang.Add((long)doanhThu);
            }
            List<SanPhamBanChayVM> sanPhamBanChayThang = await _context.ChiTietDonHang
                .Where(ct => ct.DonHang.ThoiGianDatHang >= dauThang && ct.DonHang.ThoiGianDatHang < dauThang.AddMonths(1) && ct.DonHang.TrangThai == "Đã giao")
                .GroupBy(ct => ct.BienThe.SanPham.TenSanPham)
                .Select(g => new SanPhamBanChayVM
                {
                    Ten = g.Key,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    DoanhThu = (long)g.Sum(ct => ct.BienThe.GiaBan * ct.SoLuong)
                })
                .OrderByDescending(sp => sp.DoanhThu)
                .Take(5)
                .ToListAsync();
            List<SanPhamBanChamVM> sanPhamBanChamThang = await _context.ChiTietDonHang
                .Where(ct => ct.DonHang.ThoiGianDatHang >= dauThang && ct.DonHang.ThoiGianDatHang < dauThang.AddMonths(1) && ct.DonHang.TrangThai == "Đã giao")
                .GroupBy(ct => ct.BienThe.SanPham.TenSanPham)
                .Select(g => new SanPhamBanChamVM
                {
                    Ten = g.Key,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    SoLuongConLai = g.Sum(ct => ct.BienThe.SoLuongConLai)
                })
                .OrderBy(sp => sp.SoLuongBan)
                .Take(5)
                .ToListAsync();
            int tongSoDonHangThang = await _context.DonHang
                .CountAsync(dh => dh.ThoiGianDatHang >= dauThang && dh.ThoiGianDatHang < dauThang.AddMonths(1));
            int tongSoKhachHangThang = await _context.NguoiDung
                .Where(nd => nd.ThoiGianTao >= dauThang && nd.ThoiGianTao < dauThang.AddMonths(1))
                .Distinct()
                .CountAsync();
            var trangThaiDonHangThang = new int[]
            {
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauThang && dh.ThoiGianDatHang < dauThang.AddMonths(1) && dh.TrangThai == "Đã giao"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauThang && dh.ThoiGianDatHang < dauThang.AddMonths(1) && dh.TrangThai == "Đang vận chuyển"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauThang && dh.ThoiGianDatHang < dauThang.AddMonths(1) && dh.TrangThai == "Chờ xác nhận"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauThang && dh.ThoiGianDatHang < dauThang.AddMonths(1) && dh.TrangThai == "Đã hủy")
            };



            List<string> nhanNam = new List<string>();
            List<long> doanhThuNam = new List<long>();
            for (int thang = 1; thang <= 12; thang++)
            {
                var doanhThu = await _context.DonHang
                    .Where(dh => dh.ThoiGianDatHang.Year == homNay.Year && dh.ThoiGianDatHang.Month == thang && dh.TrangThai == "Đã giao")
                    .SumAsync(dh => (decimal?)dh.TongTien ?? 0);
                nhanNam.Add($"Tháng {thang}");
                doanhThuNam.Add((long)doanhThu);
            }
            List<SanPhamBanChayVM> sanPhamBanChayNam = await _context.ChiTietDonHang
                .Where(ct => ct.DonHang.ThoiGianDatHang.Year == homNay.Year && ct.DonHang.TrangThai == "Đã giao")
                .GroupBy(ct => ct.BienThe.SanPham.TenSanPham)
                .Select(g => new SanPhamBanChayVM
                {
                    Ten = g.Key,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    DoanhThu = (long)g.Sum(ct => ct.BienThe.GiaBan * ct.SoLuong)
                })
                .OrderByDescending(sp => sp.DoanhThu)
                .Take(5)
                .ToListAsync();
            List<SanPhamBanChamVM> sanPhamBanChamNam = await _context.ChiTietDonHang
                .Where(ct => ct.DonHang.ThoiGianDatHang.Year == homNay.Year && ct.DonHang.TrangThai == "Đã giao")
                .GroupBy(ct => ct.BienThe.SanPham.TenSanPham)
                .Select(g => new SanPhamBanChamVM
                {
                    Ten = g.Key,
                    SoLuongBan = g.Sum(ct => ct.SoLuong),
                    SoLuongConLai = g.Sum(ct => ct.BienThe.SoLuongConLai)
                })
                .OrderBy(sp => sp.SoLuongBan)
                .Take(5)
                .ToListAsync();
            int tongSoDonHangNam = await _context.DonHang
                .CountAsync(dh => dh.ThoiGianDatHang.Year == homNay.Year);
            int tongSoKhachHangNam = await _context.NguoiDung
                .Where(nd => nd.ThoiGianTao.Year == homNay.Year)
                .Distinct()
                .CountAsync();
            var trangThaiDonHangNam = new int[]
            {
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauNam && dh.ThoiGianDatHang < dauNam.AddYears(1) && dh.TrangThai == "Đã giao"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauNam && dh.ThoiGianDatHang < dauNam.AddYears(1) && dh.TrangThai == "Đang vận chuyển"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauNam && dh.ThoiGianDatHang < dauNam.AddYears(1) && dh.TrangThai == "Chờ xác nhận"),
                await _context.DonHang.CountAsync(dh => dh.ThoiGianDatHang >= dauNam && dh.ThoiGianDatHang < dauNam.AddYears(1) && dh.TrangThai == "Đã hủy")
            };


            var thongKe = new ThongKeViewModel
            {
                TongSoSanPham = tongSoSanPham,
                TongSoSanPhamSapHet = tongSoSanPhamSapHet,

                nhanHomNay = nhanHomNay,
                doanhThuHomNay = doanhThuHomNay,
                sanPhamBanChayNgay = sanPhamBanChayNgay,
                SanPhamBanChamNgay = sanPhamBanChayCham,
                TongSoDonHangNgay = tongSoDonHangNgay,
                TongSoKhachHangNgay = tongSoKhachHangNgay,
                TrangThaiNgay = trangThaiDonHangNgay,

                nhanTuan = nhanTuan,
                doanhThuTuan = doanhThuTuan,
                sanPhamBanChayTuan = sanPhamBanChayTuan,
                SanPhamBanChamTuan = sanPhamBanChamTuan,
                TongSoDonHangTuan = tongSoDonHangTuan,
                TongSoKhachHangTuan = tongSoKhachHangTuan,
                TrangThaiTuan = trangThaiDonHangTuan,

                nhanThang = nhanThang,
                doanhThuThang = doanhThuThang,
                sanPhamBanChayThang = sanPhamBanChayThang,
                SanPhamBanChamThang = sanPhamBanChamThang,
                TongSoDonHangThang = tongSoDonHangThang,
                TongSoKhachHangThang = tongSoKhachHangThang,
                TrangThaiThang = trangThaiDonHangThang,

                nhanNam = nhanNam,
                doanhThuNam = doanhThuNam,
                sanPhamBanChayNam = sanPhamBanChayNam,
                SanPhamBanChamNam = sanPhamBanChamNam,
                TongSoDonHangNam = tongSoDonHangNam,
                TongSoKhachHangNam = tongSoKhachHangNam,
                TrangThaiNam = trangThaiDonHangNam
            };
            return thongKe;
        }
    }
}
