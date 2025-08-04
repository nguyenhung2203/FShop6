using FShop6.Areas.Admin.Models;

namespace FShop6.Areas.Admin.Services
{
    public interface ITaiKhoanService
    {
        List<TaiKhoanViewModel> GetAll();
        bool CapNhatTrangThai(int maNguoiDung, string trangThai);
        bool XoaTaiKhoan(int maNguoiDung);
    }
}
