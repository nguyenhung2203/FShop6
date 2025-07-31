using System.ComponentModel.DataAnnotations;

namespace FShop6.Areas.KhachHang.Models
{
    public class DanhMucSanPhamViewModel
    {
        public string TenDanhMuc { get; set; }
        public ICollection<SanPhamTrangChuViewModel> SanPhams { get; set; } = new List<SanPhamTrangChuViewModel>();
    }
}
