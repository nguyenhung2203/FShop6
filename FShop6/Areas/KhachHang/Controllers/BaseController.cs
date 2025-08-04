using FShop6.Areas.KhachHang.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FShop6.Areas.KhachHang.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IHeaderServices _headerServices;

        public BaseController(IHeaderServices headerServices)
        {
            _headerServices = headerServices;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int maNguoiDung = Convert.ToInt32(HttpContext.Session.GetInt32("MaNguoiDung"));
            var header = await _headerServices.LayDuLieu(maNguoiDung);

            ViewBag.TongSoSanPhamGioHang = header.TongGioHang;
            ViewBag.TongSoSanPhamYeuThich = header.TongYeuThich;
            ViewBag.MaNguoiDung = maNguoiDung;
            ViewBag.TenNguoiDung = header.HoTen;

            await next(); 
        }
    }
}
