using FShop6.Areas.KhachHang.Models;

namespace FShop6.Areas.Admin.Models
{
    public class QuanLySanPhamViewModel
    {
        public List<DanhMucModel> DSDanhMuc { get; set; } = new List<DanhMucModel>();
        public List<QuanLySanPhamModel> SanPhamList { get; set; } = new List<QuanLySanPhamModel>();
    }
}
