using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FShop6.Areas.Admin.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly AppDbContext _context;

        public KhachHangService(AppDbContext context)
        {
            _context = context;
        }

        public List<NguoiDungViewModel> LayDanhSachViewModel()
        {
            return _context.NguoiDung
                .Where(nd => nd.TenVaiTro == "Khách hàng")
                .Select(nd => new NguoiDungViewModel
                {
                    MaNguoiDung = nd.MaNguoiDung,
                    HoTen = nd.HoTen ?? "",
                    Email = nd.Email ?? "",
                    SDT = nd.SoDienThoai ?? "",
                    MatKhau = "******",
                    MatKhauThuc = nd.MatKhau,
                    DiaChi = nd.DiaChi ?? "",
                    TTHoatDong = nd.TTHoatDong ?? "",
                })
                .ToList();
        }
        public NguoiDungModel TimTheoId(int id)
        {
            return _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == id);
        }

        public bool ChinhSua(int id, string trangThai, string matKhau)
        {
            var nd = _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == id);
            if (nd == null) return false;

            nd.TTHoatDong = trangThai;
            nd.MatKhau = matKhau; // nếu cần mã hóa, thêm xử lý tại đây

            _context.SaveChanges();
            return true;
        }


        // 🟢 Hàm xóa nguoi dung
        public bool XoaNguoiDung(int id)
        {
            var user = _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == id);
            if (user == null) return false;

            // Xóa dữ liệu trong GioHang liên quan
            var gioHangs = _context.GioHang.Where(g => g.MaNguoiDung == id).ToList();
            if (gioHangs.Any())
            {
                _context.GioHang.RemoveRange(gioHangs);
            }

            // Xóa dữ liệu trong SPYeuThich liên quan
            var spYeuThichs = _context.SPYeuThich.Where(s => s.MaNguoiDung == id).ToList();
            if (spYeuThichs.Any())
            {
                _context.SPYeuThich.RemoveRange(spYeuThichs);
            }

            // Nếu còn bảng nào khác FK tới NguoiDung thì cũng phải xóa tương tự

            // Cuối cùng xóa người dùng
            _context.NguoiDung.Remove(user);

            _context.SaveChanges();
            return true;
        }






    }
}
