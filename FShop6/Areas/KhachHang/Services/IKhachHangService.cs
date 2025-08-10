namespace FShop6.Areas.KhachHang.Services
{
    public interface IKhachHangService
    {
        bool KiemTraEmailTonTai(string email);
        void CapNhatMatKhau(string email, string matKhauMoi);
    }

}
