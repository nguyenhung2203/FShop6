using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ICuaHangServices
    {
        Task<PhanTrangSanPhamViewModel> LaySanPhamDanhMuc(int maDanhMuc, int trang = 1, int kichThuocTrang = 8);
        Task<PhanTrangSanPhamViewModel> LaySanPhamTatCa(int trang = 1, int kichThuocTrang = 8);
    }

    public interface IChiTietSanPhamService
    {
        Task<BienTheChiTietModel> LayChiTietSanPhamAsync(int maSanPham);
    }

    public interface IShopService : ICuaHangServices, IChiTietSanPhamService
    {
        Task ThemVaoGioHang(int maNguoiDung, int maBienThe, int soLuong);
    }


    public class ShopService : IShopService
    {
        public readonly AppDbContext _context;

        public ShopService(AppDbContext context)
        {
            _context = context;
        }



        public async Task<PhanTrangSanPhamViewModel> LaySanPhamDanhMuc(int maDanhMuc, int trang = 1, int kichThuocTrang = 8)
        {
            var tongSoSanPham = await _context.BienThe
                .Include(bt => bt.SanPham)
                .Where(bt => bt.SanPham.DanhMuc.Id == maDanhMuc)
                .CountAsync();

            var tongSoTrang = (int)Math.Ceiling((double)tongSoSanPham / kichThuocTrang);
            if (trang > tongSoTrang && tongSoTrang > 0) trang = tongSoTrang;
            if (trang < 1) trang = 1;

            var danhSachSanPhamLocDanhMuc = await _context.BienThe
                .Include(bt => bt.SanPham)
                .ThenInclude(sp => sp.DanhMuc)
                .Where(bt => bt.SanPham.DanhMuc.Id == maDanhMuc)
                .OrderBy(bt => bt.SanPham.TenSanPham)
                .Skip((trang - 1) * kichThuocTrang)
                .Take(kichThuocTrang)
                .Select(bt => new BienTheTrangChuViewModel
                {
                    MaBienThe = bt.MaBienThe,
                    TenSanPham = bt.SanPham.TenSanPham,
                    HinhAnhDaiDien = bt.SanPham.HinhAnhDaiDien,
                    GiaBan = bt.GiaBan,
                    MoTaNgan = bt.SanPham.MoTa,
                    TenDanhMuc = bt.SanPham.DanhMuc.TenDanhMuc
                })
                .ToListAsync();

            var danhSachDanhMuc = await _context.DanhMucSP
                .OrderBy(dm => dm.TenDanhMuc)
                .Select(dm => new DanhMucModel
                {
                    Id = dm.Id,
                    TenDanhMuc = dm.TenDanhMuc
                })
                .ToListAsync();

            return new PhanTrangSanPhamViewModel
            {
                DanhSachSanPham = danhSachSanPhamLocDanhMuc,
                DanhSachDanhMuc = danhSachDanhMuc,
                TrangHienTai = trang,
                TongSoTrang = tongSoTrang,
            };
        }

        public async Task<PhanTrangSanPhamViewModel> LaySanPhamTatCa(int trang = 1, int kichThuocTrang = 8)
        {
            var tongSoSanPhamCount = await _context.BienThe
                .Include(bt => bt.SanPham)
                .CountAsync();

            var tongSoTrang = (int)Math.Ceiling((double)tongSoSanPhamCount / kichThuocTrang);

            if (trang > tongSoTrang && tongSoTrang > 0) trang = tongSoTrang;
            if (trang < 1) trang = 1;

            var tongSanPham = await _context.BienThe
                .Include(bt => bt.SanPham)
                .ThenInclude(sp => sp.DanhMuc)
                .OrderBy(bt => bt.SanPham.TenSanPham)
                .Skip((trang - 1) * kichThuocTrang)
                .Take(kichThuocTrang)
                .Select(bt => new BienTheTrangChuViewModel
                {
                    MaBienThe = bt.MaBienThe,
                    TenSanPham = bt.SanPham.TenSanPham,
                    HinhAnhDaiDien = bt.SanPham.HinhAnhDaiDien,
                    GiaBan = bt.GiaBan,
                    MoTaNgan = bt.SanPham.MoTa,
                    TenDanhMuc = bt.SanPham.DanhMuc.TenDanhMuc
                })
                .ToListAsync();

            var danhSachDanhMuc = await _context.DanhMucSP
                .OrderBy(dm => dm.TenDanhMuc)
                .Select(dm => new DanhMucModel
                {
                    Id = dm.Id,
                    TenDanhMuc = dm.TenDanhMuc
                })
                .ToListAsync();

            return new PhanTrangSanPhamViewModel
            {
                DanhSachSanPham = tongSanPham,
                DanhSachDanhMuc = danhSachDanhMuc,
                TrangHienTai = trang,
                TongSoTrang = tongSoTrang,
            };
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
                GiaBan = bienthe.GiaBan.ToString("N0"),
                SKU = bienthe.MaSKU,
                SoLuongTon = bienthe.SoLuongConLai, // Thêm số lượng tồn kho
                DanhSachAnh = bienthe.AnhBienThe.Select(a => a.URL).ToList()
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

        public async Task ThemVaoGioHang(int maNguoiDung, int maBienThe, int soLuong)
        {
            var giohang = await _context.GioHang
                .FirstOrDefaultAsync(g => g.MaNguoiDung == maNguoiDung && g.MaBienThe == maBienThe);

            if (giohang != null)
            {
                giohang.SoLuong += soLuong;
            }
            else
            {
                _context.GioHang.Add(new GioHangModel
                {
                    MaNguoiDung = maNguoiDung,
                    MaBienThe = maBienThe,
                    SoLuong = soLuong,
                    NgayThem = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
        }

    }
}