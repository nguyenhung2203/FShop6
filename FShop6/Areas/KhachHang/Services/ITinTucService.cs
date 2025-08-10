using FShop6.Areas.KhachHang.Models;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ITinTucService
    {

        Task<TinTucModel> LayTinTucTheoIdAsync(int id);
        Task<List<TinTucModel>> LayTinTucHienThiAsync(); // Lọc theo trạng thái nếu muốn
    }

   
 }