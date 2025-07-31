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

        public List<NguoiDungViewModel> LayDanhSach()
        {
            var nguoiDungs = _context.NguoiDung
                .Select(nd => new NguoiDungViewModel
                {
                    MaNguoiDung = nd.MaNguoiDung,
                    HoTen = nd.HoTen ?? "",
                    Email = nd.Email ?? "",
                    SDT = nd.SoDienThoai ?? "",
                    TTHoatDong = nd.TTHoatDong ?? "",
                    DiaChi = nd.DiaChi ?? "",
                    SoLuongDonDat = _context.DonHang.Count(d => d.MaNguoiDung == nd.MaNguoiDung),
                    TongTienDaDat = _context.DonHang
                                            .Where(d => d.MaNguoiDung == nd.MaNguoiDung)
                                            .Sum(d => (decimal?)d.TongTien) ?? 0
                }).ToList();

            return nguoiDungs;
        }
                    
        public NguoiDungModel TimTheoId(int id)
        {
            return _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == id);
        }

        public bool ChinhSua(int id, string TTHoatDong)
        {
            var nguoiDung = _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == id);
            if (nguoiDung != null)
            {
                nguoiDung.TTHoatDong = TTHoatDong;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        // 🟢 Hàm xóa nguoi dung
        public bool XoaNguoiDung(int id)
        {
            var user = _context.NguoiDung.FirstOrDefault(x => x.MaNguoiDung == id);
            if (user == null) return false;

            _context.NguoiDung.Remove(user);
            _context.SaveChanges(); 
            return true;
        }



    }
}
