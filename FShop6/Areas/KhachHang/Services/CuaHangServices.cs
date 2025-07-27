using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ICuaHangServices
    {
        Task<PhanTrangSanPhamViewModel> LaySanPhamDanhMuc(int maDanhMuc, int trang = 1, int kichThuocTrang = 8);
        Task<PhanTrangSanPhamViewModel> LaySanPhamTatCa(int trang = 1, int kichThuocTrang = 8);
        Task<PhanTrangSanPhamViewModel> LaySanPhamXapXep(int loai, int trang = 1, int kichThuocTrang = 8);
        Task<PhanTrangSanPhamViewModel> LaySanPhamTheoGia(decimal khoangGia, int trang = 1, int kichThuocTrang = 8);
    }

    public class CuaHangServices : ICuaHangServices
    {
        private readonly AppDbContext _context;

        public CuaHangServices(AppDbContext context)
        {
            _context = context;
        }

        private (int tongSoTrang, int trang) PhanTrang(Expression<Func<BienTheModels, bool>> dieuKien, int kichThuocTrang, int trang = 1)
        {

            var tongSoSanPham = _context.BienThe
                .Include(bt => bt.SanPham)
                .Where(dieuKien)
                .Count();

            int tongSoTrang = (int)Math.Ceiling((double)tongSoSanPham / kichThuocTrang);
            if (trang > tongSoTrang && tongSoTrang > 0) trang = tongSoTrang;
            if (trang < 1) trang = 1;
            return (tongSoTrang, trang);
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


        public async Task<PhanTrangSanPhamViewModel> LaySanPhamDanhMuc(int maDanhMuc, int trang = 1, int kichThuocTrang = 8)
        {
            var (tongSoTrang, trangHienTai) = PhanTrang(
                bt => bt.SanPham.DanhMuc.Id == maDanhMuc,
                kichThuocTrang,
                trang);


            var danhSachSanPhamLocDanhMuc = await _context.SanPham
                .Include(sp => sp.DanhMuc)
                .Where(dm => dm.DanhMuc.Id == maDanhMuc)
                .OrderBy(sp => sp.TenSanPham)
                .Skip((trangHienTai - 1) * kichThuocTrang)
                .Take(kichThuocTrang)
                .Select(sp => new SanPhamTrangChuViewModel
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    HinhAnhDaiDien = sp.HinhAnhDaiDien,
                    GiaBan = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan,
                    MoTaNgan = sp.MoTa,
                    TenDanhMuc = sp.DanhMuc.TenDanhMuc
                })
                .ToListAsync();

            var danhSachDanhMuc = await LayDanhMuc();

            return new PhanTrangSanPhamViewModel
            {
                DanhSachSanPham = danhSachSanPhamLocDanhMuc,
                DanhSachDanhMuc = danhSachDanhMuc,
                TrangHienTai = trangHienTai,
                TongSoTrang = tongSoTrang,
            };
        }

        public async Task<PhanTrangSanPhamViewModel> LaySanPhamTatCa(int trang = 1, int kichThuocTrang = 8)
        {
            var (tongSoTrang, trangHienTai) = PhanTrang(
                bt => true,
               kichThuocTrang,
               trang);

            if (trang > tongSoTrang && tongSoTrang > 0) trang = tongSoTrang;
            if (trang < 1) trang = 1;

            var tongSanPham = await _context.SanPham
                .Include(sp => sp.DanhMuc)
                .OrderBy(sp => sp.TenSanPham)
                .Skip((trangHienTai - 1) * kichThuocTrang)
                .Take(kichThuocTrang)
                .Select(sp => new SanPhamTrangChuViewModel
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    HinhAnhDaiDien = sp.HinhAnhDaiDien,
                    GiaBan = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan,
                    MoTaNgan = sp.MoTa,
                    TenDanhMuc = sp.DanhMuc.TenDanhMuc
                })
                .ToListAsync();

            var danhSachDanhMuc = await LayDanhMuc();

            return new PhanTrangSanPhamViewModel
            {
                DanhSachSanPham = tongSanPham,
                DanhSachDanhMuc = danhSachDanhMuc,
                TrangHienTai = trangHienTai,
                TongSoTrang = tongSoTrang,
            };
        }
        public async Task<PhanTrangSanPhamViewModel> LaySanPhamXapXep(int loai, int trang = 1, int kichThuocTrang = 8)
        {
            var (tongSoTrang, trangHienTai) = PhanTrang(
                bt => true,
                kichThuocTrang,
                trang);
            List<SanPhamTrangChuViewModel> tongSanPham = new List<SanPhamTrangChuViewModel>();

            IQueryable<SanPhamModel> query = _context.SanPham
            .Include(sp => sp.DanhMuc);

            if (loai == 2)
            {
                query = query.OrderBy(bt => bt.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan);
            }
            else if (loai == 3)
            {
                query = query.OrderByDescending(bt => bt.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan);
            }
            else if (loai == 4)
            {
                query = query.OrderBy(sp => sp.TenSanPham);
            }
            else if (loai == 5)
            {
                query = query.OrderByDescending(sp => sp.TenSanPham);
            }
            else if (loai == 6)
            {
                query = query.OrderBy(sp => sp.NgayTao);
            }
            else if (loai == 7)
            {
                query = query.OrderByDescending(sp => sp.NgayTao);
            }

            tongSanPham = await query
            .Skip((trangHienTai - 1) * kichThuocTrang)
            .Take(kichThuocTrang)
            .Select(sp => new SanPhamTrangChuViewModel
            {
                MaSanPham = sp.MaSanPham,
                TenSanPham = sp.TenSanPham,
                HinhAnhDaiDien = sp.HinhAnhDaiDien,
                GiaBan = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan,
                MoTaNgan = sp.MoTa,
                TenDanhMuc = sp.DanhMuc.TenDanhMuc
            })
            .ToListAsync();


            var danhSachDanhMuc = await LayDanhMuc();

            return new PhanTrangSanPhamViewModel
            {
                DanhSachSanPham = tongSanPham,
                DanhSachDanhMuc = danhSachDanhMuc,
                TrangHienTai = trangHienTai,
                TongSoTrang = tongSoTrang,
            };
        }

        public async Task<PhanTrangSanPhamViewModel> LaySanPhamTheoGia(decimal khoangGia, int trang = 1, int kichThuocTrang = 8)
        {
            var (tongSoTrang, trangHienTai) = PhanTrang(
                bt => bt.GiaBan < khoangGia,
                kichThuocTrang,
                trang);
            var tongSanPham = await _context.SanPham
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.BienThes)
                .Where(sp => sp.BienThes.Any(bt => bt.GiaBan <= khoangGia))
                .OrderBy(sp => sp.TenSanPham)
                .Skip((trangHienTai - 1) * kichThuocTrang)
                .Take(kichThuocTrang)
                .Select(sp => new SanPhamTrangChuViewModel
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    HinhAnhDaiDien = sp.HinhAnhDaiDien,
                    GiaBan = sp.BienThes.OrderBy(bt => bt.GiaBan).FirstOrDefault().GiaBan,
                    MoTaNgan = sp.MoTa,
                    TenDanhMuc = sp.DanhMuc.TenDanhMuc
                })
                .ToListAsync();

            var danhSachDanhMuc = await LayDanhMuc();
            return new PhanTrangSanPhamViewModel
            {
                DanhSachSanPham = tongSanPham,
                DanhSachDanhMuc = danhSachDanhMuc,
                TrangHienTai = trangHienTai,
                TongSoTrang = tongSoTrang,
            };
        }
    }
}