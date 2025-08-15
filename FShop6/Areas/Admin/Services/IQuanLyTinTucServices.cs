using FShop6.Areas.Admin.Models;

public interface IQuanLyTinTucService
{
    Task<(bool ThanhCong, string ThongBao)> Them(IFormCollection form, List<IFormFile> AnhDaiDien);
    Task<(bool ThanhCong, string ThongBao)> Sua(IFormCollection form, List<IFormFile> AnhDaiDien);
    Task<(bool ThanhCong, string ThongBao)> Xoa(int maTinTuc);
    Task<List<TinTucViewModel>> LayTatCa();
}
