using Microsoft.AspNetCore.Mvc;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrangChuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
