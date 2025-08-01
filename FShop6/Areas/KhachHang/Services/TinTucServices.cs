using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.KhachHang.Services
{
    public class TinTucService : ITinTucService
    {
        private readonly AppDbContext _context;

        public TinTucService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TinTucModel>> LayTinTucHienThiAsync()
        {
            return await _context.TinTuc
                .Where(t => t.TrangThai == "Hiển thị")
                .OrderByDescending(t => t.ThoiGianTao)
                .Select(t => new TinTucModel
                {
                    MaTinTuc = t.MaTinTuc,
                    TieuDe = t.TieuDe,
                    MoTaNgan = t.MoTaNgan,
                    NoiDung = t.NoiDung,
                    HinhAnhDaiDien = t.HinhAnhDaiDien,
                    TrangThai = t.TrangThai,
                    ThoiGianTao = t.ThoiGianTao,
                    NgayCapNhat = t.NgayCapNhat
                })
                .ToListAsync();
        }

        public async Task<TinTucModel> LayTinTucTheoIdAsync(int id)
        {
            var tin = await _context.TinTuc.FirstOrDefaultAsync(t => t.MaTinTuc == id);
            if (tin == null) return null;

            return new TinTucModel
            {
                MaTinTuc = tin.MaTinTuc,
                TieuDe = tin.TieuDe,
                MoTaNgan = tin.MoTaNgan,
                NoiDung = tin.NoiDung,
                HinhAnhDaiDien = tin.HinhAnhDaiDien,
                TrangThai = tin.TrangThai,
                ThoiGianTao = tin.ThoiGianTao,
                NgayCapNhat = tin.NgayCapNhat
            };
        }

       

    }
}