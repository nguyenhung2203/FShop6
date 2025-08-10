using FShop6.Areas.Admin.Models;
using FShop6.Data;
using System.Linq;

namespace FShop6.Areas.Admin.Services
{
    public class HoSoService : IHoSoService
    {
        private readonly AppDbContext _context;

        public HoSoService(AppDbContext context)
        {
            _context = context;
        }

        public HoSoAdminViewModel LayThongTinAdmin()
        {
            // Giả sử đang test user có MaNguoiDung = 1
            var admin = _context.NguoiDung.FirstOrDefault(nd => nd.TenVaiTro == "Admin");

            if (admin == null) return null;

            return new HoSoAdminViewModel
            {
                HoTen = admin.HoTen,
                Email = admin.Email,
                SoDienThoai = admin.SoDienThoai,
                DiaChi = admin.DiaChi,
                ThoiGianTao = admin.ThoiGianTao,
                NgayCapNhat = admin.NgayCapNhat,
                TenVaiTro = admin.TenVaiTro
            };
        }
    }
}
