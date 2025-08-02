using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.Admin.Services
{
    public interface IQuanLySanPhamServices
    {
        Task<QuanLySanPhamViewModel> LayDanhMuc();
        Task<QuanLySanPhamViewModel> LayTatCaSanPhamAsync();
        Task<bool> ThemSanPhamAsync(IFormCollection form, IFormFile AnhDaiDien);
        bool ThemDanhMuc(string TenDanhMuc);
        bool SuaDanhMucAsync(int MaDanhMuc, string TenDanhMuc);
        Task<bool> XoaDanhMucAsync(int MaDanhMuc);

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

        public async Task<bool> ThemSanPhamAsync(IFormCollection form, IFormFile AnhDaiDien)
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
                return true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần thiết
                Console.WriteLine($"Lỗi khi thêm sản phẩm: {ex.Message}");
                return false;
            }
        }
        public async Task<QuanLySanPhamViewModel> LayDanhMuc()
        {
            var Ds = await _context.DanhMucSP
                .Select(dm => new DanhMucModel
                {
                    Id = dm.Id,
                    TenDanhMuc = dm.TenDanhMuc,
                }).ToListAsync();
            var DsHienThi = new QuanLySanPhamViewModel
            {
                DSDanhMuc = Ds
            };
            return DsHienThi;
        }
        public bool ThemDanhMuc(string TenDanhMuc)
        {
            var dm = _context.DanhMucSP.FirstOrDefault(dm => dm.TenDanhMuc == TenDanhMuc);
            if (dm == null)
            {
                var dmsp = new DanhMucModel
                {
                    TenDanhMuc = TenDanhMuc
                };
                _context.DanhMucSP.Add(dmsp);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool SuaDanhMucAsync(int MaDanhMuc, string TenDanhMuc)
        {
            var dm = _context.DanhMucSP.Find(MaDanhMuc);
            if (dm != null)
            {
                dm.TenDanhMuc = TenDanhMuc;
                _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> XoaDanhMucAsync(int MaDanhMuc)
        {
            var dm = await _context.DanhMucSP.FindAsync(MaDanhMuc);
            if (dm != null)
            {
                _context.DanhMucSP.Remove(dm);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }



    }

}
