using FShop6.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FShop6.Areas.KhachHang.Services
{
    public interface IHeaderServices
    {
        Task<HeaderModel> LayDuLieu(int id);
    }
    public class HeaderModel
    {
        public int TongGioHang { get; set; }
        public int TongYeuThich { get; set; }
    }

    public class HeaderServices : IHeaderServices
    {
        private readonly AppDbContext context;
        private int maNguoiDung;
        public HeaderServices(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<HeaderModel> LayDuLieu(int id)
        {
            maNguoiDung = id;
            var TongGioHang = await context.GioHang
                .CountAsync(x => x.MaNguoiDung == maNguoiDung);
            var TongYeuThich = await context.SPYeuThich
                    .CountAsync(x => x.MaNguoiDung == maNguoiDung);
            var head = new HeaderModel
            {
                TongGioHang = TongGioHang,
                TongYeuThich = TongYeuThich
            };
            return head;

        }
    }
}
