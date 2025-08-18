using System.Threading.Tasks;

namespace FShop6.Areas.KhachHang.Services
{
    public interface IAdminService
    {
        Task<string> GetAdminEmailAsync();
    }
}