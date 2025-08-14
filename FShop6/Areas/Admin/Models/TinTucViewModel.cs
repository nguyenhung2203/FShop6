using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using FShop6.Areas.KhachHang.Models;

namespace FShop6.Areas.Admin.Models
{
    public class TinTucViewModel
    {
        public int MaTinTuc { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string TieuDe { get; set; } = "";

        public string MoTaNgan { get; set; } = "";
        public string NoiDung { get; set; } = "";

        public string HinhAnhDaiDien { get; set; } = "";
        public IFormFile? FileAnh { get; set; }

        public string TrangThai { get; set; } = "Hiển thị";

        public DateTime ThoiGianTao { get; set; } = DateTime.Now;
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;
    }
}
