using System.Threading.Tasks.Sources;
using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThongKeController : Controller
    {
        private readonly IThongKeServices _thongKeServices;
        public ThongKeController(IThongKeServices thongKeServices)
        {
            _thongKeServices = thongKeServices;
        }
        public async Task<IActionResult> ThongKe()
        {
            var thongKe = await _thongKeServices.LayThongKeAsync();
            return View(thongKe);
        }
    }
}
