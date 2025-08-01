using FShop6.Areas.Admin.Models;
using FShop6.Areas.Admin.Services;
using FShop6.Data; // hoặc namespace của DbContext  
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
                HoTen = u.HoTen,
                Email = u.Email,
                SoDienThoai = u.SoDienThoai,
                DiaChi = u.DiaChi,
                TTHoatDong = u.TTHoatDong,
                TenVaiTro = u.TenVaiTro
            }).ToList();
        }

        public bool CapNhatTrangThai(int maNguoiDung, string trangThai)
        {
            var user = _context.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == maNguoiDung);
            if (user == null) return false;

            user.TTHoatDong = trangThai;
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
    }
}
