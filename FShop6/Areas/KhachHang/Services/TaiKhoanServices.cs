using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.Data.SqlClient;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ISanPhamYeuThichServices
    {
        bool ThemSanPham(int nguoiDungId, int sanPhamId);
    }
    public interface ITaiKhoanServices : ISanPhamYeuThichServices
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
                        MaNguoiDung = 4,
                        MaSanPham = 6,
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

        public bool KiemTraSanPhamYeuThich(int nguoiDungId, int sanPhamId)
        {
            return _context.SPYeuThich.Any(sp => sp.MaNguoiDung == nguoiDungId && sp.MaSanPham == sanPhamId);
        }
    }
}
