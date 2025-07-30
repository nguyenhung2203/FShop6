using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.Admin.Services
{
    public interface IQuanLySanPhamServices
    {
        Task<QuanLySanPhamViewModel> LayTatCaSanPhamAsync();
        Task<QuanLySanPhamModel> LayTatCaBienTheSPAsync(int maSanPham);
    }

    public class QuanLySanPhamServices : IQuanLySanPhamServices
    {
        private readonly AppDbContext _context;
        public QuanLySanPhamServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuanLySanPhamViewModel> LayTatCaSanPhamAsync()
        {
            var dsSanPham = await _context.SanPham
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.BienThes)
                .ThenInclude(bt => bt.AnhBienThe)
                .ToListAsync();

            var sanPhamList = dsSanPham.Select(sp => new QuanLySanPhamModel
            {
                SanPham = sp,
                DanhMuc = sp.DanhMuc,
                ChiTietSanPham = sp.BienThes.Select(bt => new ChiTietSanPhamModel
                {
                    BienThes = bt,
                    dsAnh = bt.AnhBienThe.ToList()
                }).ToList()
            }).ToList();
            return new QuanLySanPhamViewModel
            {
                SanPhamList = sanPhamList
            };
        }

        public async Task<QuanLySanPhamModel> LayTatCaBienTheSPAsync(int maSanPham)
        {
            // Lấy sản phẩm
            var sanPham = await _context.SanPham
                .Include(sp => sp.DanhMuc)
                .FirstOrDefaultAsync(sp => sp.MaSanPham == maSanPham);

            if (sanPham == null) return null;

            // Lấy các biến thể
            var dsBienTheSP = await _context.BienThe
                .Include(bt => bt.AnhBienThe)
                .Where(bt => bt.MaSanPham == maSanPham)
                .ToListAsync();

            return new QuanLySanPhamModel
            {
                SanPham = sanPham,
                DanhMuc = sanPham.DanhMuc,
                ChiTietSanPham = dsBienTheSP.Select(bt => new ChiTietSanPhamModel
                {
                    BienThes = bt,
                    dsAnh = bt.AnhBienThe.ToList()
                }).ToList(),
            };
        }

    }
}
