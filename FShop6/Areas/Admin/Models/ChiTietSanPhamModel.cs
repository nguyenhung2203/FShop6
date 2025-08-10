using FShop6.Areas.KhachHang.Models;

namespace FShop6.Areas.Admin.Models
{
    public class ChiTietSanPhamModel
    {
        public BienTheModels BienThes { get; set; } = new BienTheModels();

        public List<AnhBienTheModel> dsAnh { get; set; } = new List<AnhBienTheModel>();
    }
}
