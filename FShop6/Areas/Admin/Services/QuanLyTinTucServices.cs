using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using FShop6.Areas.Admin.Models;
using FShop6.Areas.Admin.Services;
using FShop6.Areas.KhachHang.Models;
using Microsoft.AspNetCore.Hosting;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;


public class QuanLyTinTucService : IQuanLyTinTucService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public QuanLyTinTucService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
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
            var TenFile = Path.GetFileName(AnhDaiDien.FileName);
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

    public async Task<(bool ThanhCong, string ThongBao)> Sua(IFormCollection form, IFormFile AnhDaiDien)
    {
        if (!int.TryParse(form["MaTinTuc"], out int maTinTuc))
            return (false, "Mã tin tức không hợp lệ.");

        var tinTuc = await _context.TinTuc.FindAsync(maTinTuc);
        if (tinTuc == null)
            return (false, "Không tìm thấy tin tức.");

        tinTuc.TieuDe = form["TieuDe"];
        tinTuc.MoTaNgan = form["MoTaNgan"];
        tinTuc.NoiDung = form["NoiDung"];
        tinTuc.TrangThai = form["TrangThai"];
        tinTuc.NgayCapNhat = DateTime.Now;

        if (AnhDaiDien != null && AnhDaiDien.Length > 0)
        {
            // Xóa ảnh cũ nếu tồn tại
            if (!string.IsNullOrEmpty(tinTuc.HinhAnhDaiDien))
            {
                var pathOld = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images", tinTuc.HinhAnhDaiDien);
                if (File.Exists(pathOld))
                    File.Delete(pathOld);
            }

            // Lưu ảnh mới
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Path.GetFileName(AnhDaiDien.FileName);
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images");
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await AnhDaiDien.CopyToAsync(stream);
            }

            tinTuc.HinhAnhDaiDien = fileName;
        }

        _context.TinTuc.Update(tinTuc);
        await _context.SaveChangesAsync();

        return (true, "Sửa tin tức thành công.");
    }
    public async Task<(bool ThanhCong, string ThongBao)> Xoa(int maTinTuc)
    {
        try
        {
            var tinTuc = await _context.TinTuc.FindAsync(maTinTuc);
            if (tinTuc == null)
                return (false, "Không tìm thấy tin tức cần xóa.");

            _context.TinTuc.Remove(tinTuc);
            await _context.SaveChangesAsync();
            return (true, "Xóa tin tức thành công.");
        }
        catch (Exception ex)
        {
            return (false, $"Lỗi khi xóa tin tức: {ex.Message}");
        }
    }



    public async Task<List<TinTucViewModel>> LayTatCa()
    {
        var listEntity = await _context.TinTuc.ToListAsync();

        var listViewModel = listEntity.Select(t => new TinTucViewModel
        {
            MaTinTuc = t.MaTinTuc,
            TieuDe = t.TieuDe,
            MoTaNgan = t.MoTaNgan,
            NoiDung = t.NoiDung,
            HinhAnhDaiDien = t.HinhAnhDaiDien,
            TrangThai = t.TrangThai,
            ThoiGianTao = t.ThoiGianTao,
            NgayCapNhat = t.NgayCapNhat,
            
        }).ToList();

        return listViewModel;
    }


}
