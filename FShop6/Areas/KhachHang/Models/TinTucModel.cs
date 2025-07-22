using System.ComponentModel.DataAnnotations;

namespace FShop6.Areas.KhachHang.Models
{
    public class TinTucModel
    {
        [Key]
        public int MaTinTuc { get; set; }

        public string TieuDe { get; set; }
        public string MoTaNgan { get; set; }
        public string NoiDung { get; set; }
        public string HinhAnhDaiDien { get; set; }
        public string TrangThai { get; set; }

        public DateTime ThoiGianTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
    }
}
