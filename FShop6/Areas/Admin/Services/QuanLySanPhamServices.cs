using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.Admin.Services
{
    public interface IQuanLySanPhamServices
    {
        Task<QuanLySanPhamViewModel> LayTatCaSanPhamAsync();
        Task ThemSanPhamAsync(IFormCollection form, IFormFile AnhDaiDien);
        Task SuaSanPhamAsync(IFormCollection form, IFormFile AnhDaiDien);
        Task XoaSanPhamAsync(int MaSanPham);
        Task ThemBienTheAsync(IFormCollection form, List<IFormFile> AnhBienThe);
    }

    public class QuanLySanPhamServices : IQuanLySanPhamServices
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QuanLySanPhamServices(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<QuanLySanPhamViewModel> LayTatCaSanPhamAsync()
        {
            var dsSanPham = await _context.SanPham
                .Include(sp => sp.DanhMuc)
                .Include(sp => sp.BienThes)
                .ThenInclude(bt => bt.AnhBienThe)
                .ToListAsync();

            var sanPhamList = dsSanPham.Select(sp => new QuanLySanPhamModel
            {
                SanPham = sp,
                DanhMuc = sp.DanhMuc,
                ChiTietSanPham = sp.BienThes.Select(bt => new ChiTietSanPhamModel
                {
                    BienThes = bt,
                    dsAnh = bt.AnhBienThe.ToList()
                }).ToList()
            }).ToList();
            return new QuanLySanPhamViewModel
            {
                SanPhamList = sanPhamList
            };
        }

        public async Task ThemSanPhamAsync(IFormCollection form, IFormFile AnhDaiDien)
        {
            try
            {
                var sanPham = new SanPhamModel
                {
                    TenSanPham = form["TenSanPham"],
                    MoTa = form["MoTa"],
                    MaDanhMucSP = int.Parse(form["DanhMucID"])
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
                    sanPham.HinhAnhDaiDien = TenFile;
                }

                _context.SanPham.Add(sanPham);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần thiết
                Console.WriteLine($"Lỗi khi thêm sản phẩm: {ex.Message}");
            }
        }

        public async Task SuaSanPhamAsync(IFormCollection form, IFormFile AnhDaiDien)
        {
            try
            {
                int maSanPham = int.Parse(form["MaSanPham"]);
                var sanPham = await _context.SanPham.FindAsync(maSanPham);
                if (sanPham == null) return;

                sanPham.TenSanPham = form["TenSanPham"];
                sanPham.MaDanhMucSP = int.Parse(form["DanhMucID"]);
                sanPham.MoTa = form["MoTa"];

                var ThoiGianLuuFile = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var TenFile = ThoiGianLuuFile + "_" + Path.GetFileName(AnhDaiDien.FileName);
                var TaiLenFolder = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images");
                var duongDan = Path.Combine(TaiLenFolder, TenFile);
                using (var stream = new FileStream(duongDan, FileMode.Create))
                {
                    await AnhDaiDien.CopyToAsync(stream);
                }
                sanPham.HinhAnhDaiDien = TenFile;

                _context.SanPham.Update(sanPham);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi sửa sản phẩm: {ex.Message}");
            }
        }

        public async Task XoaSanPhamAsync(int MaSanPham)
        {
            try
            {
                var sanPham = await _context.SanPham.FindAsync(MaSanPham);
                if (sanPham == null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(sanPham.HinhAnhDaiDien))
                {
                    var duongDanAnh = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images", sanPham.HinhAnhDaiDien);
                    if (System.IO.File.Exists(duongDanAnh))
                    {
                        System.IO.File.Delete(duongDanAnh);
                    }
                }

                _context.SanPham.Remove(sanPham);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xoá sản phẩm: {ex.Message}");
            }
        }

        public async Task ThemBienTheAsync(IFormCollection form, List<IFormFile> AnhBienThe)
        {
            try
            {
                int maSanPham = int.Parse(form["MaSanPham"]);
                var sanPham = await _context.SanPham.FindAsync(maSanPham);
                if (sanPham == null) return;
                var bienThe = new BienTheModels
                {
                    MaSKU = form["MaSKU"],
                    MaSanPham = maSanPham,
                    LoaiBienThe = form["LoaiBienThe"],
                    GiaBan = decimal.Parse(form["GiaBan"]),
                    GiaNhap = decimal.Parse(form["GiaNhap"]),
                    TinhTrang = form["TinhTrang"],
                    SoLuongConLai = int.Parse(form["SoLuongConLai"])
                };
                _context.BienThe.Add(bienThe);
                await _context.SaveChangesAsync();
                Console.WriteLine(bienThe.MaBienThe);
                if (AnhBienThe != null && AnhBienThe.Count > 0)
                {
                    foreach (var anh in AnhBienThe)
                    {
                        var ThoiGianLuuFile = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        var TenFile = ThoiGianLuuFile + "_" + Path.GetFileName(anh.FileName);
                        var TaiLenFolder = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images");
                        var duongDan = Path.Combine(TaiLenFolder, TenFile);
                        using (var stream = new FileStream(duongDan, FileMode.Create))
                        {
                            await anh.CopyToAsync(stream);
                        }
                        var anhBienThe = new AnhBienTheModel
                        {
                            MaBienThe = bienThe.MaBienThe,
                            URL = TenFile // Thiết lập mối quan hệ với biến thể
                        };
                        _context.AnhBienThe.Add(anhBienThe);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm biến thể sản phẩm: {ex.Message}");
            }
        }
    }
}
