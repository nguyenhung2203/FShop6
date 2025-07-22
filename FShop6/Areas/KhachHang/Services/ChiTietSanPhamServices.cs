using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
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
                var sanPham = await _context.SanPham
                    .Include(sp => sp.DanhMuc)
                    .Include(sp => sp.BienThe)
                        .ThenInclude(b => b.AnhBienThe)
                    .FirstOrDefaultAsync(sp => sp.MaSanPham == maSanPham);

                if (sanPham == null)
                {
                    return null;
                }    

                var bienTheList = sanPham.BienThe.Select(bienthe => new BienTheChiTietModel.BienTheModel
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
        }
    }
}
