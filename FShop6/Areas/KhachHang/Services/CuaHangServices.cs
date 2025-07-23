using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ICuaHangServices
    {
        Task<PhanTrangSanPhamViewModel> laySanPham(int maDanhMuc);
        Task<PhanTrangSanPhamViewModel> laySanPhamTatCa();
    }
    public class CuaHangServices : ICuaHangServices
    {
        private readonly AppDbContext _context;
        public CuaHangServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PhanTrangSanPhamViewModel> laySanPham(int maDanhMuc)
        {
            var tongSoSanPham = await _context.SanPham.CountAsync();
            var trangHienTai = 1;
            var kichThuocTrang = 8;
            var danhSachSanPhamLocDanhMuc= await _context.BienThe
            .Include(bt => bt.SanPham).ThenInclude(sp => sp.DanhMuc)
           .OrderBy(sp => sp.MaSanPham)
           .Where(bt => bt.SanPham.DanhMuc.Id == maDanhMuc)
           .Skip((trangHienTai - 1) * kichThuocTrang)
           .Take(kichThuocTrang)
           .Select(p => new BienTheTrangChuViewModel
           {
                MaBienThe = p.MaBienThe,
                TenSanPham = p.SanPham.TenSanPham,
                HinhAnhDaiDien = p.SanPham.HinhAnhDaiDien,
                GiaBan = p.GiaBan,
                MoTaNgan = p.SanPham.MoTa
           }).ToListAsync();

            // Lấy danh sách danh mục sản phẩm
            var danhSachDanhMuc = await _context.DanhMucSP.ToListAsync(); // ✅ load danh mục

            var model = new PhanTrangSanPhamViewModel
                {
                    DanhSachSanPhamLocDanhMuc = danhSachSanPhamLocDanhMuc,
                    DanhSachDanhMuc = danhSachDanhMuc,
                    DanhSachSanPhamLocGia = new List<BienTheTrangChuViewModel>(), // Chưa có dữ liệu lọc giá
                    DanhSachSanPhamXapXep = new List<BienTheTrangChuViewModel>(), // Chưa có dữ liệu sắp xếp
                    TrangHienTai = trangHienTai,
                    TongSoTrang = (int)Math.Ceiling((double)tongSoSanPham / kichThuocTrang)
                };

            return model;
        }

        public async Task<PhanTrangSanPhamViewModel> laySanPhamTatCa()
        {
            var danhSachDanhMuc = await _context.DanhMucSP.ToListAsync(); // ✅ load danh mục
            // Lấy tất cả sản phẩm
            var tongSanPham = await _context.BienThe
                .Include(bt => bt.SanPham).ThenInclude(sp => sp.DanhMuc)
                .Take(12) // Giới hạn 12 sản phẩm
                .Select(bt => new BienTheTrangChuViewModel
                {
                    MaBienThe = bt.MaBienThe,
                    TenSanPham = bt.SanPham.TenSanPham,
                    HinhAnhDaiDien = bt.SanPham.HinhAnhDaiDien,
                    GiaBan = bt.GiaBan,
                    MoTaNgan = bt.SanPham.MoTa,
                    TenDanhMuc = bt.SanPham.DanhMuc.TenDanhMuc
                }).ToListAsync();

            return new PhanTrangSanPhamViewModel
            {
                DanhSachDanhMuc = danhSachDanhMuc,
                TongSanPham = tongSanPham
            };
        }

    }
}
