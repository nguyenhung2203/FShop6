using FShop6.Areas.Admin.Models;
using FShop6.Areas.KhachHang.Models;
using System.Collections.Generic;

public interface IKhachHangService
{
    List<NguoiDungViewModel> LayDanhSachViewModel();
    NguoiDungModel TimTheoId(int id);
    bool ChinhSua(int id, string tthd, string matkhau);

    bool XoaNguoiDung(int id);
}
