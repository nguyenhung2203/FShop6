using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using FShop6.Areas.Admin.Models;
using FShop6.Areas.Admin.Services;
using FShop6.Areas.KhachHang.Models;
using Microsoft.AspNetCore.Hosting;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Linq;

public class QuanLyTinTucService : IQuanLyTinTucService
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public QuanLyTinTucService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    private string UploadFolderPath => Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images");

    private async Task<List<string>> LuuNhieuAnh(List<IFormFile> files)
    {
        var danhSachTenFile = new List<string>();

        if (files != null && files.Count > 0)
        {
            if (!Directory.Exists(UploadFolderPath))
                Directory.CreateDirectory(UploadFolderPath);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var savePath = Path.Combine(UploadFolderPath, fileName);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    danhSachTenFile.Add(fileName);
                }
            }
        }

        return danhSachTenFile;
    }

    private void XoaAnhCu(string chuoiAnh)
    {
        if (!string.IsNullOrEmpty(chuoiAnh))
        {
            var anhCu = chuoiAnh.Split(";", StringSplitOptions.RemoveEmptyEntries);
            foreach (var fileName in anhCu)
            {
                var pathOld = Path.Combine(UploadFolderPath, fileName);
                if (File.Exists(pathOld))
                    File.Delete(pathOld);
            }
        }
    }

    public async Task<(bool ThanhCong, string ThongBao)> Them(IFormCollection form, List<IFormFile> AnhDaiDien)
    {
        if (string.IsNullOrWhiteSpace(form["TieuDe"]) || string.IsNullOrWhiteSpace(form["NoiDung"]))
        {
            return (false, "Vui lòng nhập đầy đủ tiêu đề và nội dung.");
        }

        var tintuc = new TinTucModel
        {
            TieuDe = form["TieuDe"].ToString(),
            MoTaNgan = form["MoTaNgan"].ToString(),
            NoiDung = form["NoiDung"].ToString(),
            TrangThai = form["TrangThai"].ToString(),
            ThoiGianTao = DateTime.Now,
            NgayCapNhat = DateTime.Now
        };

        // Lưu nhiều ảnh, nếu không có ảnh thì để trống
        if (AnhDaiDien != null && AnhDaiDien.Count > 0)
        {
            var danhSachTenFile = await LuuNhieuAnh(AnhDaiDien);
            tintuc.HinhAnhDaiDien = string.Join(";", danhSachTenFile);
        }
        else
        {
            tintuc.HinhAnhDaiDien = string.Empty; // không có ảnh
        }

        _context.TinTuc.Add(tintuc);
        await _context.SaveChangesAsync();

        return (true, "Thêm tin tức thành công");
    }

    public async Task<(bool ThanhCong, string ThongBao)> Sua(IFormCollection form, List<IFormFile> AnhDaiDien)
    {
        if (!int.TryParse(form["MaTinTuc"], out int maTinTuc))
            return (false, "Mã tin tức không hợp lệ.");

        var tinTuc = await _context.TinTuc.FindAsync(maTinTuc);
        if (tinTuc == null)
            return (false, "Không tìm thấy tin tức.");

        tinTuc.TieuDe = form["TieuDe"].ToString();
        tinTuc.MoTaNgan = form["MoTaNgan"].ToString();
        tinTuc.NoiDung = form["NoiDung"].ToString();
        tinTuc.TrangThai = form["TrangThai"].ToString();
        tinTuc.NgayCapNhat = DateTime.Now;

        // Lấy danh sách ảnh hiện tại từ DB
        var danhSachAnhCu = string.IsNullOrEmpty(tinTuc.HinhAnhDaiDien)
            ? new List<string>()
            : tinTuc.HinhAnhDaiDien.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();

        // Nếu form gửi danh sách ảnh muốn giữ lại (ví dụ qua hidden input)
        if (!string.IsNullOrEmpty(form["HinhAnhCu"]))
        {
            danhSachAnhCu = form["HinhAnhCu"].ToString()
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        // Lưu ảnh mới (nếu có)
        if (AnhDaiDien != null && AnhDaiDien.Count > 0)
        {
            var danhSachAnhMoi = await LuuNhieuAnh(AnhDaiDien);
            danhSachAnhCu.AddRange(danhSachAnhMoi);
        }

        // Gộp ảnh cũ và mới
        tinTuc.HinhAnhDaiDien = string.Join(";", danhSachAnhCu);

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

            // Xóa file ảnh vật lý
            XoaAnhCu(tinTuc.HinhAnhDaiDien);

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

        return listEntity.Select(t => new TinTucViewModel
        {
            MaTinTuc = t.MaTinTuc,
            TieuDe = t.TieuDe,
            MoTaNgan = t.MoTaNgan,
            NoiDung = t.NoiDung,
            HinhAnhDaiDien = t.HinhAnhDaiDien,
            TrangThai = t.TrangThai,
            ThoiGianTao = t.ThoiGianTao,
            NgayCapNhat = t.NgayCapNhat
        }).ToList();
    }
}
