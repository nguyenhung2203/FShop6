//using FShop6.Areas.Admin.Models;
//using FShop6.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace FShop6.Areas.Admin.Services
//{
//    public interface IQuanLyDonHangServices
//    {
//        Task<DonHangModel> donHang();
//    }
//    public class QuanLyDonHangServices : IQuanLyDonHangServices
//    {
//        private readonly AppDbContext _context;
//        public QuanLyDonHangServices(AppDbContext context)
//        {
//            _context = context;
//        }
//        public async Task<DonHangModel> donHang()
//        {
//            // Dữ liệu giả tạm thời, để hiện giao diện mà không lỗi
//            var model = new DonHangModel
//            {
//                DanhSachDonHang = new List<DonHang>
//        {
//            new DonHang
//            {
//                MaDon = "DH001",
//                NgayDat = DateTime.Now,
//                TongTien = 1000000,
//                TrangThai = "Đang xử lý",
//                KhachHang = new KhachHang { TenKhachHang = "Nguyễn Văn A" }
//            },
//            new DonHang
//            {
//                MaDon = "DH002",
//                NgayDat = DateTime.Now.AddDays(-1),
//                TongTien = 2500000,
//                TrangThai = "Hoàn thành",
//                KhachHang = new KhachHang { TenKhachHang = "Trần Thị B" }
//            }
//        }
//            };

//            return await Task.FromResult(model); // giả lập async
//        }
//    }
//}

