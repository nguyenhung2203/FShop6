using Microsoft.AspNetCore.Mvc;
using FShop6.Areas.Admin.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace FShop6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyTinTucController : Controller
    {
        private readonly IQuanLyTinTucService _quanLyTinTucService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuanLyTinTucController(IQuanLyTinTucService quanLyTinTucService, IWebHostEnvironment webHostEnvironment)
        {
            _quanLyTinTucService = quanLyTinTucService;
            _webHostEnvironment = webHostEnvironment;
        }

        // 📌 Hiển thị danh sách tin tức
        public async Task<IActionResult> QuanLyTinTuc()
        {
            var danhSach = await _quanLyTinTucService.LayTatCa();
            return View("~/Areas/Admin/Views/TinTuc/QuanLyTinTuc.cshtml", danhSach);
        }

        // 📌 Hàm upload nhiều ảnh, trả về danh sách tên file
        private async Task<List<string>> UploadNhieuAnh(List<IFormFile> files)
        {
            var fileNames = new List<string>();

            if (files != null && files.Any())
            {
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        fileNames.Add(fileName);
                    }
                }
            }
            return fileNames;
        }

        // 📌 Thêm tin tức mới
        [HttpPost]
        public async Task<IActionResult> ThemTinTuc(IFormCollection form, List<IFormFile> AnhDaiDien)
        {
            if (string.IsNullOrWhiteSpace(form["TieuDe"]) || string.IsNullOrWhiteSpace(form["NoiDung"]))
            {
                TempData["Loi"] = "❌ Vui lòng nhập đầy đủ tiêu đề và nội dung.";
                return RedirectToAction(nameof(QuanLyTinTuc));
            }

            var fileNames = await UploadNhieuAnh(AnhDaiDien);
            var mergedFiles = string.Join(";", fileNames) + (fileNames.Any() ? ";" : "");

            var newForm = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(form)
            {
                { "HinhAnhDaiDien", mergedFiles }
            });

            var ketQua = await _quanLyTinTucService.Them(newForm, AnhDaiDien);

            TempData[ketQua.ThanhCong ? "ThongBao" : "Loi"] = ketQua.ThanhCong
                ? "✅ Thêm tin tức thành công!"
                : $"❌ Thêm thất bại: {ketQua.ThongBao}";

            return RedirectToAction(nameof(QuanLyTinTuc));
        }

        // 📌 Sửa tin tức
        [HttpPost]
        public async Task<IActionResult> SuaTinTuc(IFormCollection form, List<IFormFile> AnhDaiDien)
        {
            if (string.IsNullOrWhiteSpace(form["TieuDe"]) || string.IsNullOrWhiteSpace(form["NoiDung"]))
            {
                TempData["Loi"] = "❌ Vui lòng nhập đầy đủ tiêu đề và nội dung.";
                return RedirectToAction(nameof(QuanLyTinTuc));
            }

            // Lấy ảnh cũ (nếu có)
            var fileNames = new List<string>();
            if (!string.IsNullOrEmpty(form["HinhAnhCu"]))
                fileNames.AddRange(form["HinhAnhCu"].ToString()
                    .Split(';', StringSplitOptions.RemoveEmptyEntries));

            // Upload ảnh mới (nếu có)
            var newFiles = await UploadNhieuAnh(AnhDaiDien);
            fileNames.AddRange(newFiles);

            // Ghép tất cả ảnh lại (không để ;;)
            var mergedFiles = string.Join(";", fileNames) + (fileNames.Any() ? ";" : "");

            var newForm = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(form)
            {
                { "HinhAnhDaiDien", mergedFiles }
            });

            var ketQua = await _quanLyTinTucService.Sua(newForm, AnhDaiDien);

            TempData[ketQua.ThanhCong ? "ThongBao" : "Loi"] = ketQua.ThanhCong
                ? "✅ Cập nhật tin tức thành công!"
                : $"❌ Cập nhật thất bại: {ketQua.ThongBao}";

            return RedirectToAction(nameof(QuanLyTinTuc));
        }

        // 📌 Xóa tin tức
        [HttpPost]
        public async Task<IActionResult> XoaTinTuc(int MaTinTuc)
        {
            var ketQua = await _quanLyTinTucService.Xoa(MaTinTuc);
            TempData[ketQua.ThanhCong ? "ThongBao" : "Loi"] = ketQua.ThanhCong
                ? "🗑️ Đã xóa tin tức thành công."
                : $"❌ Xóa thất bại: {ketQua.ThongBao}";

            return RedirectToAction(nameof(QuanLyTinTuc));
        }
    }
}
