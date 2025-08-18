using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using System.Linq;

namespace FShop6.Areas.Admin.Services.Implementations
{
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly AppDbContext _context;

        public TaiKhoanService(AppDbContext context)
        {
            _context = context;
        }

        public List<TaiKhoanViewModel> GetAll()
        {
            return _context.NguoiDung
                .Select(u => new TaiKhoanViewModel
                {
                    MaNguoiDung = u.MaNguoiDung,
                    TaiKhoan = u.TaiKhoan,
                    MatKhau = u.MatKhau,
                    Email = u.Email,
                    SoDienThoai = u.SoDienThoai,
                    DiaChi = u.DiaChi,
                    TTHoatDong = u.TTHoatDong,
                    TenVaiTro = u.TenVaiTro
                }).ToList();
        }

        public bool CapNhatTrangThai(int maNguoiDung, string trangThai, string? matKhauMoi = null)
        {
            var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == maNguoiDung);
            if (user == null) return false;

            user.TTHoatDong = trangThai;
            if (!string.IsNullOrWhiteSpace(matKhauMoi))
            {
                user.MatKhau = matKhauMoi;
            }

            user.NgayCapNhat = DateTime.Now;
            _context.SaveChanges();
            return true;
        }

        public bool XoaTaiKhoan(int maNguoiDung)
        {
            var user = _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == maNguoiDung);
            if (user == null) return false;

            _context.NguoiDung.Remove(user);
            _context.SaveChanges();
            return true;
        }

        // 👉 Thêm mới tài khoản
        public bool ThemTaiKhoan(TaiKhoanViewModel model)
        {
            try
            {
                var user = new NguoiDungModel
                {
                    HoTen = model.HoTen,
                    TaiKhoan = model.TaiKhoan,
                    MatKhau = model.MatKhau,
                    Email = model.Email,
                    SoDienThoai = model.SoDienThoai,
                    DiaChi = model.DiaChi,  
                    TTHoatDong = model.TTHoatDong ?? "Hoạt động",
                    TenVaiTro = model.TenVaiTro,
                    ThoiGianTao = DateTime.Now,
                    NgayCapNhat = DateTime.Now
                };

                _context.NguoiDung.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
