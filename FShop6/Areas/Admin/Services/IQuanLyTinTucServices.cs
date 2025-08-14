using FShop6.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FShop6.Areas.Admin.Services
{
    public interface IQuanLyTinTucService
    {
        Task<(bool ThanhCong, string ThongBao)> Them(IFormCollection form, IFormFile AnhDaiDien);
        Task<(bool ThanhCong, string ThongBao)> Sua(IFormCollection form, IFormFile AnhDaiDien);
        Task<(bool ThanhCong, string ThongBao)> Xoa(int maTinTuc);
        Task<List<TinTucViewModel>> LayTatCa();
    }
}
