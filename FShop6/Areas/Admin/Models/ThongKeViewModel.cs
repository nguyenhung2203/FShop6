namespace FShop6.Areas.Admin.Models
{
    public class ThongKeViewModel
    {
        public List<ThongKeTheoNamModel> ThongKeTheoNam { get; set; }
        public List<ThongKeTheoThangModel> ThongKeTheoThang { get; set; }
        public List<int> Year { get; set; }
        public List<int> Month { get; set; }
    }
}
