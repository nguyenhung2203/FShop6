using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TaiKhoanController : Controller
    {
        private readonly ISanPhamYeuThichServices _yeuThichService;

        public TaiKhoanController(ISanPhamYeuThichServices yeuThichService)
        {
            _yeuThichService = yeuThichService;
        }

        public IActionResult HoSo(string? tab)
        {
            ViewBag.SelectedTab = tab;
            return View();
        }

        public IActionResult DangNhap() => View();
        public IActionResult DangKy() => View();
        public IActionResult QuenMatKhau() => View();

        // Hiển thị danh sách sản phẩm yêu thích
        public async Task<IActionResult> SanPhamYeuThich()
        {
            int maNguoiDung = 5; // test cứng
            var model = await _yeuThichService.LayDanhSach(maNguoiDung);
            return View(model);
        }

        // Xóa sản phẩm yêu thích
        [HttpPost]
        public async Task<IActionResult> Xoa(int maSanPham)
        {
            int maNguoiDung = 5; // test cứng
            await _yeuThichService.Xoa(maNguoiDung, maSanPham);
            return RedirectToAction("SanPhamYeuThich");
        }
    }
}
