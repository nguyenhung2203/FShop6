using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using FShop6.Areas.Admin.Models;
using FShop6.Areas.Admin.Services;
using FShop6.Areas.KhachHang.Models;
using Microsoft.AspNetCore.Hosting;
using FShop6.Data;

public class QuanLyTinTucService : IQuanLyTinTucService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public QuanLyTinTucService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<(bool ThanhCong, string ThongBao)> Sua(IFormCollection form, IFormFile AnhDaiDien)
    {
        // TODO: Viết logic sửa tin tức
        // Ví dụ: Lấy dữ liệu từ form, tìm bản ghi trong DB, cập nhật rồi lưu

        return (true, "Sửa tin tức thành công");
    }

    // Các hàm khác cũng phải implement đầy đủ
    public async Task<(bool ThanhCong, string ThongBao)> Them(IFormCollection form, IFormFile AnhDaiDien)
    {
        var tintuc = new TinTucModel
        {
            TieuDe = form["TieuDe"],
            MoTaNgan = form["MoTaNgan"],
            NoiDung = form["NoiDung"],
            TrangThai = form["TrangThai"],
            ThoiGianTao = DateTime.Now,
            NgayCapNhat = DateTime.Now
        };
        if (AnhDaiDien != null && AnhDaiDien.Length > 0)
        {
            var ThoiGianLuuFile = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var TenFile = ThoiGianLuuFile + "_" + Path.GetFileName(AnhDaiDien.FileName);
            var TaiLenFolder = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images");
            var duongDan = Path.Combine(TaiLenFolder, TenFile);
            using (var stream = new FileStream(duongDan, FileMode.Create))
            {
                await AnhDaiDien.CopyToAsync(stream);
            }
            tintuc.HinhAnhDaiDien = TenFile;
        }
        _context.TinTuc.Add(tintuc);
        await _context.SaveChangesAsync();
        return (true, "Thêm tin tức thành công");
      
    }

    public async Task<(bool ThanhCong, string ThongBao)> Xoa(int maTinTuc)
    {
        // Logic xóa tin tức
        return (true, "Xóa tin tức thành công");
    }

    public async Task<List<TinTucViewModel>> LayTatCa()
    {
        // Logic lấy danh sách tin tức
        return new List<TinTucViewModel>();
    }
}
