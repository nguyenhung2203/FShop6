using FShop6.Areas.KhachHang.Models;

namespace FShop6.Areas.Admin.Models
{
    public class QuanLySanPhamViewModel
    {
        public List<DanhMucModel> DSDanhMuc { get; set; } = new List<DanhMucModel>();
        public List<QuanLySanPhamModel> SanPhamList { get; set; } = new List<QuanLySanPhamModel>();
        public int TongSoSanPham { get; set; }
        public int SoSanPhamMoiTrang { get; set; }
        public int TrangHienTai { get; set; }
        public int TongSoTrang { get; set; }
    }
}
