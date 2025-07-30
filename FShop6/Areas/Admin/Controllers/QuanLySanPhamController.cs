using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")] // Đánh dấu đây là controller trong khu vực Admin
    public class QuanLySanPhamController : Controller
    {
        private readonly IQuanLySanPhamServices _quanLySanPhamServices;
        public QuanLySanPhamController(IQuanLySanPhamServices quanLySanPhamServices)
        {
            _quanLySanPhamServices = quanLySanPhamServices;
        }

        public async Task<IActionResult> QuanLySanPham()
        {
            var dsSanPham = await _quanLySanPhamServices.LayTatCaSanPhamAsync();
            return View(dsSanPham);
        }

        [HttpPost]
        public async Task<IActionResult> XemChiTietSanPham(int maSanPham)
        {
            Console.WriteLine(">> Đã vào action XemChiTietSanPham với maSanPham = " + maSanPham);
            var dsBienTheSP = await _quanLySanPhamServices.LayTatCaBienTheSPAsync(maSanPham);
            if (dsBienTheSP == null)
            {
                // Nếu không tìm thấy sản phẩm, có thể trả về một thông báo lỗi hoặc trang khác
                return NotFound();
            }
            // Trả về danh sách biến thể sản phẩm cho view
            return View("XemChiTietSanPham", dsBienTheSP);
        }
    }
}
