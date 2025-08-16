using FShop6.Areas.KhachHang.Services;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TrangChuController : BaseController
    {
        private readonly ITrangChuService _trangChuService;
        public TrangChuController(IHeaderServices headerServices, ITrangChuService trangChuService)
            : base(headerServices)
        {
            _trangChuService = trangChuService;
        }

        public async Task<IActionResult> Index()
        {
            int maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung") ?? 0;
            var model = await _trangChuService.LayDuLieuTrangChuDataAsync(maNguoiDung);
            return View(model);
        }
    }
}
