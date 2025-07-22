using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TrangChuController : Controller
    {
        private readonly ITrangChuService _trangChuService;

        public TrangChuController(ITrangChuService trangChuService)
        {
            _trangChuService = trangChuService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _trangChuService.LayDuLieuTrangChuDataAsync();
            return View(model);
        }
    }
}
