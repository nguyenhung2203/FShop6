using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;

public interface IKhachHangService
{
    List<NguoiDungViewModel> LayDanhSach();
    NguoiDungModel TimTheoId(int id);
    bool ChinhSua(int id, string TTHoatDong);
    bool XoaNguoiDung(int id);  // Dùng int thay vì email
}
