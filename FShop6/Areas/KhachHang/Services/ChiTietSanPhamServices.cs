using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.KhachHang.Services
{
    public class ChiTietSanPhamServices
    {
        public interface IChiTietSanPhamService
        {
            Task<BienTheChiTietModel> LayChiTietSanPhamAsync(int maSanPham);
        }
        public class ChiTietSanPhamService : IChiTietSanPhamService
        {
            private readonly AppDbContext _context;
            public ChiTietSanPhamService(AppDbContext context)
            {
                _context = context;
            }
            public async Task<BienTheChiTietModel> LayChiTietSanPhamAsync(int maSanPham)
            {
                var bienThe = await _context.AnhBienThe
                    .Include(bt => bt.BienThe)
                    .ThenInclude(bt => bt.SanPham)
                    .ThenInclude(sp => sp.DanhMuc)
                    .Where(bt => bt.BienThe.SanPham.MaSanPham == maSanPham)
                    .ToListAsync();

                if (bienThe == null)
                {
                    return null;
                }

                return new BienTheChiTietModel
                {
                    MaBienThe = bienThe.First().MaBienThe,
                    MaSanPham = bienThe.First().BienThe.MaSanPham,
                    GiaBan = bienThe.First().BienThe.GiaBan,
                    MaSKU = bienThe.First().BienThe.MaSKU,
                    LoaiBienThe = bienThe.First().BienThe.LoaiBienThe,
                    SoLuong = bienThe.First().BienThe.SoLuongConLai,
                    AnhBienThe = bienThe, // Assigning the collection of AnhBienTheModel  
                    SanPham = bienThe.First().BienThe.SanPham
                };
            }
        }
    }
}
