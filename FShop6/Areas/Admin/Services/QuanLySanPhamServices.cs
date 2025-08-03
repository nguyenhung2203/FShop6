using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FShop6.Areas.Admin.Services
{
    public interface IQuanLySanPhamServices
    {
        Task<QuanLySanPhamViewModel> LayTatCaSanPhamAsync();
        Task ThemSanPhamAsync(IFormCollection form, IFormFile AnhDaiDien);
        Task SuaSanPhamAsync(IFormCollection form, IFormFile AnhDaiDien);
        Task XoaSanPhamAsync(int MaSanPham);
        Task ThemBienTheAsync(IFormCollection form, IFormFile AnhBienThe);
        Task SuaBienTheAsync(IFormCollection form, IFormFile AnhBienThe);
        Task XoaBienTheAsync(int MaBienThe);
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
                    MaDanhMucSP = int.Parse(form["DanhMucID"]),
                    NoiBat = form["NoiBat"] == "true",
                    TrangThai = form["TrangThai"] == "true"
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
                int maSanPham = int.Parse(form["maSanPham"]);
                var sanPham = await _context.SanPham.FindAsync(maSanPham);
                if (sanPham == null) return;

                sanPham.TenSanPham = form["tenSanPham"];
                sanPham.MaDanhMucSP = int.Parse(form["danhMucId"]);
                sanPham.MoTa = form["moTa"];
                sanPham.NoiBat = form["noiBat"] == "true";
                sanPham.TrangThai = form["trangThai"] == "true";

                if (AnhDaiDien != null && AnhDaiDien.Length > 0)
                {
                    if (!string.IsNullOrEmpty(sanPham.HinhAnhDaiDien))
                    {
                        var anhCu = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images", sanPham.HinhAnhDaiDien);
                        if (System.IO.File.Exists(anhCu))
                        {
                            System.IO.File.Delete(anhCu);
                        }
                    }
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
                var sanPham = await _context.SanPham
                    .Include(sp => sp.BienThes)
                    .ThenInclude(bt => bt.AnhBienThe)
                    .FirstOrDefaultAsync(sp => sp.MaSanPham == MaSanPham);

                if (sanPham == null)
                {
                    return;
                }

                var maBienThe = sanPham.BienThes.Select(bt => (int?)bt.MaBienThe);
                bool coTrongDonHang = await _context.ChiTietDonHang
                    .AnyAsync(ctdh => maBienThe.Contains(ctdh.MaBienThe) && ctdh.DonHang.TrangThai != "Đã giao hàng");

                if (coTrongDonHang)
                {
                    Console.WriteLine("Không thể xoá sản phẩm vì có biến thể đã được đặt hàng.");
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

                foreach (var bienThe in sanPham.BienThes)
                {
                    // Xoá ảnh biến thể (file + DB)
                    foreach (var anh in bienThe.AnhBienThe)
                    {
                        if (!string.IsNullOrEmpty(anh.URL))
                        {
                            var duongDanAnhBienThe = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images", anh.URL);
                            if (System.IO.File.Exists(duongDanAnhBienThe))
                            {
                                System.IO.File.Delete(duongDanAnhBienThe);
                            }
                        }
                        _context.AnhBienThe.Remove(anh);
                    }
                    // Xoá chi tiết đơn hàng liên quan đến biến thể này
                    var chiTietDonHangs = _context.ChiTietDonHang
                        .Where(ctdh => ctdh.MaBienThe == bienThe.MaBienThe);
                    _context.ChiTietDonHang.RemoveRange(chiTietDonHangs);

                    // Xoá biến thể
                    _context.BienThe.Remove(bienThe);
                }
                // Xoá sản phẩm yêu thích liên quan đến sản phẩm này
                var sanPhamYeuThich = _context.SPYeuThich
                    .Where(spyt => spyt.MaSanPham == MaSanPham);
                _context.SPYeuThich.RemoveRange(sanPhamYeuThich);
                // Cuối cùng xoá sản phẩm
                _context.SanPham.Remove(sanPham);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xoá sản phẩm: {ex.Message}");
            }
        }

        public async Task ThemBienTheAsync(IFormCollection form, IFormFile AnhBienThe)
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
                if (AnhBienThe != null && AnhBienThe.Length > 0)
                {
                    var ThoiGianLuuFile = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var TenFile = ThoiGianLuuFile + "_" + Path.GetFileName(AnhBienThe.FileName);
                    var TaiLenFolder = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images");
                    var duongDan = Path.Combine(TaiLenFolder, TenFile);
                    using (var stream = new FileStream(duongDan, FileMode.Create))
                    {
                        await AnhBienThe.CopyToAsync(stream);
                    }
                    var anhBienThe = new AnhBienTheModel
                    {
                        MaBienThe = bienThe.MaBienThe,
                        URL = TenFile // Thiết lập mối quan hệ với biến thể
                    };
                    _context.AnhBienThe.Add(anhBienThe);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm biến thể sản phẩm: {ex.Message}");
            }
        }

        public async Task SuaBienTheAsync(IFormCollection form, IFormFile AnhBienThe)
        {
            try
            {
                int maBienThe = int.Parse(form["MaBienThe"]);
                var bienThe = await _context.BienThe
                    .Include(bt => bt.AnhBienThe)
                    .FirstOrDefaultAsync(bt => bt.MaBienThe == maBienThe);
                if (bienThe == null) return;

                // Cập nhật thông tin biến thể
                bienThe.MaSKU = form["MaSKU"];
                bienThe.LoaiBienThe = form["LoaiBienThe"];
                bienThe.GiaBan = decimal.Parse(form["GiaBan"]);
                bienThe.GiaNhap = decimal.Parse(form["GiaNhap"]);
                bienThe.TinhTrang = form["TinhTrang"];
                bienThe.SoLuongConLai = int.Parse(form["SoLuongConLai"]);
                var maAnhCu = int.Parse(form["AnhBienTheCu"]);

                if (AnhBienThe != null && AnhBienThe.Length > 0)
                {
                    // Lấy ảnh cũ từ DB

                    var anhBienTheCu = await _context.AnhBienThe
                        .FirstOrDefaultAsync(anh => anh.MaHinhAnh == maAnhCu);

                    // Xoá file ảnh cũ nếu có
                    if (anhBienTheCu != null && !string.IsNullOrEmpty(anhBienTheCu.URL))
                    {
                        var duongDanAnhCu = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images", anhBienTheCu.URL);
                        if (System.IO.File.Exists(duongDanAnhCu))
                        {
                            System.IO.File.Delete(duongDanAnhCu);
                        }
                    }

                    // Lưu file mới
                    var ThoiGianLuuFile = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var TenFile = ThoiGianLuuFile + "_" + Path.GetFileName(AnhBienThe.FileName);
                    var TaiLenFolder = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images");
                    var duongDan = Path.Combine(TaiLenFolder, TenFile);
                    using (var stream = new FileStream(duongDan, FileMode.Create))
                    {
                        await AnhBienThe.CopyToAsync(stream);
                    }

                    // Cập nhật hoặc thêm ảnh mới trong DB
                    if (anhBienTheCu != null)
                    {
                        anhBienTheCu.URL = TenFile;
                        _context.AnhBienThe.Update(anhBienTheCu);
                    }
                    else
                    {
                        var anhBienTheMoi = new AnhBienTheModel
                        {
                            MaBienThe = bienThe.MaBienThe,
                            URL = TenFile
                        };
                        _context.AnhBienThe.Add(anhBienTheMoi);
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi sửa biến thể sản phẩm: {ex}");
            }
        }
        public async Task XoaBienTheAsync(int MaBienThe)
        {
            try
            {
                var bienThe = await _context.BienThe
                    .Include(bt => bt.AnhBienThe)
                    .FirstOrDefaultAsync(bt => bt.MaBienThe == MaBienThe);
                if (bienThe == null) return;

                // Kiểm tra xem biến thể có trong đơn hàng chưa  
                var coTrongDonHang = await _context.ChiTietDonHang
                    .AnyAsync(ctdh => ctdh.MaBienThe == MaBienThe && ctdh.DonHang.TrangThai != "Đã giao hàng");
                if (coTrongDonHang)
                {
                    Console.WriteLine("Không thể xoá biến thể vì đã được đặt hàng.");
                    return;
                }

                // Xoá ảnh biến thể  
                foreach (var anh in bienThe.AnhBienThe)
                {
                    var duongDanAnh = Path.Combine(_webHostEnvironment.WebRootPath, "KhachHang", "images", anh.URL);
                    if (System.IO.File.Exists(duongDanAnh))
                    {
                        System.IO.File.Delete(duongDanAnh);
                    }
                    _context.AnhBienThe.Remove(anh);
                }

                // Xoá chi tiết đơn hàng liên quan đến biến thể này
                var chiTietDonHangs = _context.ChiTietDonHang
                    .Where(ctdh => ctdh.MaBienThe == MaBienThe);

                // Xoá giỏ hàng liên quan đến biến thể này
                var gioHang = _context.GioHang
                    .Where(gh => gh.MaBienThe == MaBienThe);
                _context.GioHang.RemoveRange(gioHang);
                _context.ChiTietDonHang.RemoveRange(chiTietDonHangs);

                // Xoá biến thể  
                _context.BienThe.Remove(bienThe);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xoá biến thể sản phẩm: {ex.Message}");
            }
        }
    }
}
