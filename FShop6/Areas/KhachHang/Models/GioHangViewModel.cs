namespace FShop6.Areas.KhachHang.Models
{
    public class GioHangViewModel
    {
       public List<GioHangItemModel> GioHang{ get; set; } = new List<GioHangItemModel>();
        public DiaChiViewModel DiaChi { get; set; }
    }
}
