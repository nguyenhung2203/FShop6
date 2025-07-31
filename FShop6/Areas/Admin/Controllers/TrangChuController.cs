using FShop6.Areas.Admin.Models;
using FShop6.Areas.Admin.Services;
using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrangChuController : Controller
    {
        private readonly ITrangChuAdminServices _service;

        public TrangChuController(ITrangChuAdminServices service, IHoSoService hoSoService)
        {
            _service = service;
            _hoSoService = hoSoService;
        }
        public IActionResult Index()
        {
            var model = _service.LayThongTinTrangChu();
            return View(model);
        }
        private readonly IHoSoService _hoSoService;

        public IActionResult HoSo()
        {
            var model = _hoSoService.LayThongTinAdmin();
            return View(model);
        }


    }
}
