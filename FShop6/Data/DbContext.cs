using FShop6.Areas.KhachHang.Models;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<DanhMucModel> DanhMucSP { get; set; }
        public DbSet<SanPhamModel> SanPham { get; set; }
        public DbSet<TinTucModel> TinTuc { get; set; }
        public DbSet<BienTheModels> BienThe { get; set; }
        public DbSet<NguoiDungModel> KhachHang { get; set; }
        public DbSet<DonHangModel> DonHang { get; set; }
        public DbSet<ChiTietDonHangModel> ChiTietDonHang { get; set; }
        public DbSet<GioHangModel> GioHang { get; set; }
        public DbSet<AnhBienTheModel> AnhBienThe { get; set; }

    }
}
