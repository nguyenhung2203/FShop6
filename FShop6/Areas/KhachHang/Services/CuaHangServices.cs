using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using static FShop6.Areas.KhachHang.Models.BienTheChiTietModel.BienTheModel;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ICuaHangServices
    {
        Task<PhanTrangSanPhamViewModel> LaySanPhamDaLoc(int? maDanhMuc, decimal? giaToiDa, int? loaiXapXep, int trang = 1, int kichThuocTrang = 8);
        Task<int> SoLuongDaBan(int maSanPham);
    }

    public interface IChiTietSanPhamService
    {
        Task<BienTheChiTietModel> LayChiTietSanPhamAsync(int maSanPham);
    }

    public interface IShopService : ICuaHangServices, IChiTietSanPhamService
    {
        Task<List<object>> TimKiem(string tuKhoa);
    }


    public class ShopService : IShopService
    {
        public readonly AppDbContext _context;

        public ShopService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SoLuongDaBan(int maSanPham)
        {
            return await _context.ChiTietDonHang
                .Where(ct => ct.BienThe.SanPham.MaSanPham == maSanPham && ct.DonHang.TrangThai == "Đã giao")
                .SumAsync(ct => ct.SoLuong);
        }

        public async Task<PhanTrangSanPhamViewModel> LaySanPhamDaLoc(int? maDanhMuc, decimal? giaToiDa, int? loaiXapXep, int trang = 1, int kichThuocTrang = 8)
        {
            // 1. Bắt đầu với một câu truy vấn gốc, chưa thực thi
            IQueryable<SanPhamModel> query = _context.SanPham
                                                .Include(sp => sp.DanhMuc)
                                                .Include(sp => sp.BienThes);

            // 2. Áp dụng bộ lọc DANH MỤC nếu có
            if (maDanhMuc.HasValue && maDanhMuc.Value > 0)
            {
                query = query.Where(sp => sp.DanhMuc.Id == maDanhMuc.Value);
            }

            // 3. Áp dụng bộ lọc GIÁ nếu có
            if (giaToiDa.HasValue)
            {
                query = query.Where(sp => sp.BienThes.Any(bt => bt.GiaBan <= giaToiDa.Value));
            }

            // 4. Áp dụng SẮP XẾP nếu có
            if (loaiXapXep.HasValue)
            {
                switch (loaiXapXep.Value)
                {
                    case 1: // Mặc định - không sắp xếp thêm
                        // Giữ nguyên thứ tự mặc định
                        break;
                    case 2: // Giá tăng dần
                        query = query.OrderBy(sp => sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan);
                        break;
                    case 3: // Giá giảm dần
                        query = query.OrderByDescending(sp => sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan);
                        break;
                    case 4: // Tên A-Z
                        query = query.OrderBy(sp => sp.TenSanPham);
                        break;
                    case 5: // Tên Z-A
                        query = query.OrderByDescending(sp => sp.TenSanPham);
                        break;
                    case 6: // Mới nhất
                        query = query.OrderByDescending(sp => sp.NgayTao);
                        break;
                    case 7: // Cũ nhất
                        query = query.OrderBy(sp => sp.NgayTao);
                        break;
                }
            }

            var tongSoSanPham = await query.CountAsync();
            var tongSoTrang = (int)Math.Ceiling(tongSoSanPham / (double)kichThuocTrang);

            var danhSachSanPham = await query
                .Where(sp => sp.TrangThai == true)
                .Skip((trang - 1) * kichThuocTrang)
                .Take(kichThuocTrang)
                .Select(sp => new SanPhamTrangChuViewModel
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    HinhAnhDaiDien = sp.HinhAnhDaiDien,
                    GiaBan = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan,
                    MoTaNgan = sp.MoTa,
                    TenDanhMuc = sp.DanhMuc.TenDanhMuc,
                    GiamGia = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiamGia
                })
                .ToListAsync();
            foreach (var sp in danhSachSanPham)
            {
                sp.SoLuongDaBan = await SoLuongDaBan(sp.MaSanPham);
            }

            var danhSachDanhMuc = await LayDanhMuc();

            // 6. Trả về kết quả cuối cùng
            return new PhanTrangSanPhamViewModel
            {
                DanhSachSanPham = danhSachSanPham,
                DanhSachDanhMuc = danhSachDanhMuc,
                TrangHienTai = trang,
                TongSoTrang = tongSoTrang,
            };
        }

        public async Task<List<DanhMucModel>> LayDanhMuc()
        {
            return await _context.DanhMucSP
                .OrderBy(dm => dm.TenDanhMuc)
                .Select(dm => new DanhMucModel
                {
                    Id = dm.Id,
                    TenDanhMuc = dm.TenDanhMuc
                })
                .ToListAsync();
        }

        public async Task<BienTheChiTietModel> LayChiTietSanPhamAsync(int maSanPham)
        {
            var sanPham = await _context.SanPham
                    .Include(sp => sp.DanhMuc)
                    .Include(sp => sp.BienThes)
                        .ThenInclude(b => b.AnhBienThe)
                    .FirstOrDefaultAsync(sp => sp.MaSanPham == maSanPham);
                    

            if (sanPham == null)
            {
                return null;
            }

            var bienTheList = sanPham.BienThes.Select(bienthe => new BienTheChiTietModel.BienTheModel
            {
                MaBienThe = bienthe.MaBienThe,
                LoaiBienThe = bienthe.LoaiBienThe,
                GiaBan = bienthe.GiaBan.ToString(),
                SKU = bienthe.MaSKU,
                SoLuongTon = bienthe.SoLuongConLai, // Thêm số lượng tồn kho
                TinhTrang = bienthe.TinhTrang,
                GiamGia = bienthe.GiamGia, // Thêm thông tin giảm giá
                URL = bienthe.AnhBienThe.Select(a => a.URL).FirstOrDefault() // Lấy ảnh đầu tiên của biến thể
            }).ToList();

            return new BienTheChiTietModel
            {
                MaSanPham = sanPham.MaSanPham,
                TenSanPham = sanPham.TenSanPham,
                MoTa = sanPham.MoTa,
                HinhAnhDaiDien = sanPham.HinhAnhDaiDien,
                DanhMuc = sanPham.DanhMuc?.TenDanhMuc ?? "Không có danh mục",
                BienThe = bienTheList // Trả về danh sách các biến thể
            };
        }

        public async Task<List<object>> TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return new List<object>();
            var ketQua = await _context.SanPham
                .Where(sp => sp.TenSanPham.Contains(tuKhoa))
                .Select(sp => new
                {
                    id = sp.MaSanPham,
                    ten = sp.TenSanPham,
                    gia = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan.ToString("N0") + "₫" ,
                    anh = sp.HinhAnhDaiDien
                })
                .Take(5)
                .ToListAsync();
            return ketQua.Cast<object>().ToList();
        }
    }
}