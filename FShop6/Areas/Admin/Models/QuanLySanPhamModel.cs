using FShop6.Areas.KhachHang.Models;

namespace FShop6.Areas.Admin.Models
{
    public class QuanLySanPhamModel
    {
        public SanPhamModel SanPham { get; set; } = new SanPhamModel();
        public DanhMucModel DanhMuc { get; set; } = new DanhMucModel();
        public List<ChiTietSanPhamModel> ChiTietSanPham { get; set; } = new List<ChiTietSanPhamModel>();
    }
}
