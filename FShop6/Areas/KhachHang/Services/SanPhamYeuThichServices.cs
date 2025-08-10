using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ISanPhamYeuThichServices
    {
        Task<List<SanPhamYeuThichViewModel>> LayDanhSach(int maNguoiDung);
        Task Them(int maNguoiDung, int maSanPham);
        Task Xoa(int maNguoiDung, int maSanPham);
    }

    public class SanPhamYeuThichServices : ISanPhamYeuThichServices
    {
        private readonly AppDbContext _context;

        public SanPhamYeuThichServices(AppDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách sản phẩm yêu thích của người dùng
        public async Task<List<SanPhamYeuThichViewModel>> LayDanhSach(int maNguoiDung)
        {
            return await _context.SPYeuThich
                .Where(spyt => spyt.MaNguoiDung == maNguoiDung)
                .Join(_context.SanPham,
                      spyt => spyt.MaSanPham,
                      sp => sp.MaSanPham,
                      (spyt, sp) => new SanPhamYeuThichViewModel
                      {
                          MaSanPham = sp.MaSanPham,
                          TenSanPham = sp.TenSanPham,
                          MoTaNgan = sp.MoTa,
                          HinhAnh = sp.HinhAnhDaiDien,
                          GiaBan = _context.BienThe
                                    .Where(bt => bt.MaSanPham == sp.MaSanPham)
                                    .OrderBy(bt => bt.GiaBan)
                                    .Select(bt => bt.GiaBan)
                                    .FirstOrDefault(),
                          ConHang = _context.BienThe
                                    .Any(bt => bt.MaSanPham == sp.MaSanPham && bt.SoLuongConLai > 0)
                      })
                .ToListAsync();
        }

        // Thêm sản phẩm vào danh sách yêu thích
        public async Task Them(int maNguoiDung, int maSanPham)
        {
            var tonTai = await _context.SPYeuThich
                .AnyAsync(spyt => spyt.MaNguoiDung == maNguoiDung && spyt.MaSanPham == maSanPham);

            if (!tonTai)
            {
                _context.SPYeuThich.Add(new SanPhamYeuThichModel
                {
                    MaNguoiDung = maNguoiDung,
                    MaSanPham = maSanPham,
                    NgayThem = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }
        }

        // Xoá sản phẩm khỏi danh sách yêu thích
        public async Task Xoa(int maNguoiDung, int maSanPham)
        {
            var spyt = await _context.SPYeuThich
                .FirstOrDefaultAsync(sp => sp.MaNguoiDung == maNguoiDung && sp.MaSanPham == maSanPham);

            if (spyt != null)
            {
                _context.SPYeuThich.Remove(spyt);
                await _context.SaveChangesAsync();
            }
        }
    }
}
