using FShop6.Data;
using Microsoft.Identity.Client;

namespace FShop6.Areas.KhachHang.Services
{
    public interface IGioHang
    {
        bool ThemGioHang(int maBienThe, int soLuong);
    }
    public interface IThanhToan
    {

    }

    public interface IGioHangServices : IGioHang, IThanhToan
    {
        
    }
    public class GioHangServices : IGioHangServices
    {
        private readonly AppDbContext _context;
        public GioHangServices(AppDbContext context)
        {
            _context = context;
        }

        public bool ThemGioHang(int id, int soLuong)
        {
            return true;
        }
    }
}
