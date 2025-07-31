using FShop6.Areas.KhachHang.Models;
using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace FShop6.Areas.KhachHang.Controllers
{
    [Area("KhachHang")]
    public class TinTucController : BaseController
    {
        private readonly ITinTucService _tinTucService;

        public TinTucController(ITinTucService tinTucService)
        public TinTucController(IHeaderServices headerServices, ITinTucService tinTucService)
        : base(headerServices)
        {
            _tinTucService = tinTucService
        }
        public IActionResult TinTuc()
        {
            _tinTucService = tinTucService;
        }


    
        public async Task<IActionResult> TinTuc()
        {
            var danhSachTinTuc = await _tinTucService.LayTinTucHienThiAsync();

            if (danhSachTinTuc == null || !danhSachTinTuc.Any())
            {
                ViewBag.ThongBao = "Hiện chưa có tin tức nào.";
            }

            return View(danhSachTinTuc);
        }
      public async Task<IActionResult> ChiTietTinTuc(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID không hợp lệ.");
            }

            var tinTuc = await _tinTucService.LayTinTucTheoIdAsync(id);
            if (tinTuc == null)
            {
                return NotFound("Không tìm thấy tin tức.");
            }

            return View(tinTuc);
        }


    }
}