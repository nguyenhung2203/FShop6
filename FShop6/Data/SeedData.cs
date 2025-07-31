using FShop6.Areas.KhachHang.Models;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Data
{
    public class SeedData
    {
        public static void DuLieuMau(AppDbContext _context)
        {
            if (!_context.DanhMucSP.Any() && !_context.SanPham.Any() && !_context.BienThe.Any())
            {
                // 1. Dữ liệu mẫu cho DanhMucModel
                var danhMucs = new List<DanhMucModel>
                {
                    new DanhMucModel { Id = 1, TenDanhMuc = "Áo thun" },
                    new DanhMucModel { Id = 2, TenDanhMuc = "Gấu bông" },
                    new DanhMucModel { Id = 3, TenDanhMuc = "Móc khóa" }
                };
                _context.DanhMucSP.AddRange(danhMucs);
                _context.SaveChanges();

                // 2. Dữ liệu mẫu cho SanPhamModel
                var sanPhams = new List<SanPhamModel>
                {
                    new SanPhamModel
                    {
                        MaSanPham = 1,
                        MaDanhMucSP = 1,
                        TenSanPham = "Áo Thun Local Brand",
                        MoTa = "Áo thun cotton 100%, thiết kế trẻ trung.",
                        HinhAnhDaiDien = "/images/ao-thun.jpg",
                        DanhMuc = danhMucs[0]
                    },
                    new SanPhamModel
                    {
                        MaSanPham = 2,
                        MaDanhMucSP = 2,
                        TenSanPham = "Gấu Bông Brown",
                        MoTa = "Gấu bông siêu mềm, cao 50cm.",
                        HinhAnhDaiDien = "/images/gau-bong.jpg",
                        DanhMuc = danhMucs[1]
                    },
                    new SanPhamModel
                    {
                        MaSanPham = 3,
                        MaDanhMucSP = 3,
                        TenSanPham = "Móc Khóa Doremon",
                        MoTa = "Móc khóa cao su hình Doremon dễ thương.",
                        HinhAnhDaiDien = "/images/moc-khoa.jpg",
                        DanhMuc = danhMucs[2]
                    }
                };
                _context.SanPham.AddRange(sanPhams);
                _context.SaveChanges();

                // 3. Dữ liệu mẫu cho BienTheModels
                var bienThes = new List<BienTheModels>
{
    new BienTheModels
    {
        MaBienThe = 1,
        MaSanPham = 1,
        MaSKU = "AT-LB-M",
        LoaiBienThe = "Size M",
        GiaNhap = 120000,
        GiaBan = 150000,
        SoLuongConLai = 20,
        TinhTrang = "Còn hàng",
        SanPham = sanPhams[0]
    },
    new BienTheModels
    {
        MaBienThe = 2,
        MaSanPham = 2,
        MaSKU = "GB-BROWN-50",
        LoaiBienThe = "Size 50cm",
        GiaNhap = 180000,
        GiaBan = 220000,
        SoLuongConLai = 10,
        TinhTrang = "Còn hàng",
        SanPham = sanPhams[1]
    },
    new BienTheModels
    {
        MaBienThe = 3,
        MaSanPham = 3,
        MaSKU = "MK-DM-STD",
        LoaiBienThe = "Móc khóa tiêu chuẩn",
        GiaNhap = 30000,
        GiaBan = 50000,
        SoLuongConLai = 50,
        TinhTrang = "Còn hàng",
        SanPham = sanPhams[2]
    }
};

                _context.BienThe.AddRange(bienThes);
                _context.SaveChanges();
            }
        }
    }


}
