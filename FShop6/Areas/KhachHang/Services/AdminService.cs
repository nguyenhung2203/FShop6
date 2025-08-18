using FShop6.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FShop6.Areas.KhachHang.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetAdminEmailAsync()
        {
            // Tìm user có role Admin
            var adminUser = await _context.NguoiDung
                .Where(u => u.TenVaiTro == "Admin")
                .FirstOrDefaultAsync();

            // Báo lỗi nếu không tìm thấy
            if (adminUser == null || string.IsNullOrEmpty(adminUser.Email))
            {
                throw new Exception("Không tìm thấy email admin trong hệ thống");
            }

            return adminUser.Email;
        }
    }
}