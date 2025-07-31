using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class GioHangController : Controller
    {
        private readonly AppDbContext _context;

        // ✅ Sửa lỗi trùng tên tham số và field
        public GioHangController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Hiển thị danh sách giỏ hàng
        public async Task<IActionResult> GioHang()
        {
            int maNguoiDung = 3; // Sau này có thể lấy từ session

            var gioHang = await _context.GioHang
                .Include(g => g.BienThe)
                    .ThenInclude(bt => bt.SanPham)
                .Where(g => g.MaNguoiDung == maNguoiDung)
                .ToListAsync();

            return View(gioHang); // Truyền sang View
        }

        // ✅ Trang thanh toán (có thể mở rộng)
        public IActionResult ThanhToan()
        {
            return View();
        }
    }
}
