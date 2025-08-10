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
                .Select(nd => new NguoiDungViewModel
                {
                    MaNguoiDung = nd.MaNguoiDung,
                    HoTen = nd.HoTen ?? "",
                    Email = nd.Email ?? "",
                    SDT = nd.SoDienThoai ?? "",
                    MatKhau = nd.MatKhau ?? "",
                    DiaChi = nd.DiaChi ?? "",
                    TTHoatDong = nd.TTHoatDong ?? "",
                    SoLuongDonDat = _context.DonHang.Count(d => d.MaNguoiDung == nd.MaNguoiDung),
                    TongTienDaDat = _context.DonHang
                        .Where(d => d.MaNguoiDung == nd.MaNguoiDung)
                        .Sum(d => (decimal?)d.TongTien) ?? 0,
                    DonHang = _context.DonHang
                        .Where(d => d.MaNguoiDung == nd.MaNguoiDung)
                        .ToList()
                }).ToList();
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

            _context.NguoiDung.Remove(user);
            _context.SaveChanges(); 
            return true;
        }



    }
}
