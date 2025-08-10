using FShop6.Data;

namespace FShop6.Areas.KhachHang.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly AppDbContext _context;

        public KhachHangService(AppDbContext context)
        {
            _context = context;
        }

        public bool KiemTraEmailTonTai(string email)
        {
            return _context.NguoiDung.Any(nd => nd.Email == email && nd.TenVaiTro == "Khách hàng");
        }

        public void CapNhatMatKhau(string email, string matKhauMoi)
        {
            var user = _context.NguoiDung.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.MatKhau = matKhauMoi; // Có thể hash nếu cần
                user.NgayCapNhat = DateTime.Now;
                _context.SaveChanges();
            }
        }
    }

}
