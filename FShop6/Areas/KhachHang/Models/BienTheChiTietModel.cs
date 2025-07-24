using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.KhachHang.Models
{
    public class BienTheChiTietModel
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public string HinhAnhDaiDien { get; set; }
        public string DanhMuc { get; set; }

        // Thông tin về các biến thể
        public List<BienTheModel> BienThe { get; set; } // Danh sách biến thể

        public class BienTheModel
        {
            public int MaBienThe { get; set; }
            public string LoaiBienThe { get; set; }
            public string GiaBan { get; set; }
            public string SKU { get; set; }
            public int SoLuongTon { get; set; } // Số lượng tồn kho của biến thể

            // Các ảnh liên quan đến biến thể
            public List<string> DanhSachAnh { get; set; } // Danh sách ảnh của biến thể
        }
    }
}