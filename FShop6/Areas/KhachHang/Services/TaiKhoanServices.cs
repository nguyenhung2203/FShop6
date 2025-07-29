using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ISanPhamYeuThichServices
    {
        bool ThemSanPham(int nguoiDungId, int sanPhamId);
    }
    public interface IHoSoServices
    {
        Task<HoSoViewModel> LayThongTinHoSo(int nguoiDungId);
        bool HuyDonHang(string donHangId);
        bool CapNhatThongTinHoSo(NguoiDungModel nguoiDungModel);
    }
    public interface ITaiKhoanServices : ISanPhamYeuThichServices, IHoSoServices
    {

    }
    public class TaiKhoanServices : ITaiKhoanServices
    {
        private readonly AppDbContext _context;
        public TaiKhoanServices(AppDbContext context)
        {
            _context = context;
        }
        public bool ThemSanPham(int nguoiDungId, int sanPhamId)
        {
            try
            {
                var daTonTai = KiemTraSanPhamYeuThich(nguoiDungId, sanPhamId);
                if (!daTonTai)
                {
                    var yeuThich = new SPYeuThichModel
                    {
                        MaNguoiDung = 5,
                        MaSanPham = sanPhamId,
                        NgayThem = DateTime.Now
                    };
                    _context.SPYeuThich.Add(yeuThich);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                return false;
            }

        }

        public bool CapNhatThongTinHoSo(NguoiDungModel nguoiDungModel)
        {
            try
            {
                var nguoiDung = _context.NguoiDung.FirstOrDefault(nd => nd.MaNguoiDung == nguoiDungModel.MaNguoiDung);
                if (nguoiDung != null)
                {
                    nguoiDung.HoTen = nguoiDungModel.HoTen;
                    nguoiDung.Email = nguoiDungModel.Email;
                    nguoiDung.SoDienThoai = nguoiDungModel.SoDienThoai;
                    nguoiDung.DiaChi = nguoiDungModel.DiaChi;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public async Task<HoSoViewModel> LayThongTinHoSo(int nguoiDungId)
        {
            var nguoiDung = await _context.NguoiDung
                .Where(nd => nd.MaNguoiDung == nguoiDungId)
                .Select(nd => new NguoiDungModel
                {
                    MaNguoiDung = nd.MaNguoiDung,
                    HoTen = nd.HoTen,
                    Email = nd.Email,
                    SoDienThoai = nd.SoDienThoai,
                    DiaChi = nd.DiaChi
                })
                .FirstOrDefaultAsync();

            var donHang = await _context.DonHang
                .Where(dh => dh.MaNguoiDung == nguoiDungId)
                .ToListAsync();
            var hoSo = new HoSoViewModel
            {
                nguoiDungModels = nguoiDung,
                donHangModels = donHang
            };

            return hoSo;
        }

        public bool HuyDonHang(string donHangId)
        {
            try
            {
                var donHang = _context.DonHang.Where(dh => dh.MaDonHang == donHangId && dh.TrangThai == "Chờ xử lý").FirstOrDefault();
                if (donHang != null)
                {
                    donHang.TrangThai = "Đã hủy";
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch(SqlException ex)
            {
                return false;
            }

        }

        public bool KiemTraSanPhamYeuThich(int nguoiDungId, int sanPhamId)
        {
            return _context.SPYeuThich.Any(sp => sp.MaNguoiDung == nguoiDungId && sp.MaSanPham == sanPhamId);
        }
    }
}
