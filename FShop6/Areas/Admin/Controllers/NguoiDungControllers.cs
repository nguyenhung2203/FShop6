using FShop6.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FShop6.Areas.KhachHang.Models;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NguoiDungController : Controller
    {
        private readonly AppDbContext _context;

        public NguoiDungController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var danhSach = _context.NguoiDung.ToList();
            return View(danhSach);
        }
    }
}
