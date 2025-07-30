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
            var thongKe = await _thongKeServices.LayThongKe();
            if (thongKe == null)
            {
                // Kiểm tra nếu dữ liệu null, trả về view thông báo lỗi hoặc xử lý khác
                return View("Error");
            }

            ViewData["Months"] = thongKe.Month;
            ViewData["Years"] = thongKe.Year;
            return View(thongKe);
        }
        [HttpPost]
        public async Task<IActionResult> ThongKe(int year, int month)
        {
            //Lấy dữ liệu thống kê từ services
            var thongKe = await _thongKeServices.LayThongKe(month, year);

            //Truyền lại các giá trị tháng và năm vào viewdata để hiển thị lại trên form
            ViewData["Months"] = thongKe.Month;
            ViewData["Years"] = thongKe.Year;

            //Lưu giá trị tháng và năm đã chọn vào ViewData để hiển thị lại trên form
            ViewData["SelectedYear"] = year;
            ViewData["SelectedMonth"] = month;

            //Trả về dữ liệu thống kê (theo năm và theo tháng) cho View
            return View(thongKe);
        }
    }
}
