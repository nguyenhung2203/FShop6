using FShop6.Areas.KhachHang.Models;
using FShop6.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FShop6.Areas.KhachHang.Services
{
    public interface ISanPhamYeuThichServices
    {
        bool ThemSanPham(int nguoiDungId, int sanPhamId);
        Task<List<SPYeuThichModel>> LayDanhSach(int maNguoiDung);
        bool XoaSanPham(int nguoiDungId, int sanPhamId);
    }
    public interface IHoSoServices
    {
        Task<HoSoViewModel> LayThongTinHoSo(int nguoiDungId);
        bool HuyDonHang(int donHangId, int maNguoiDung, string noiDungHuy);
        bool CapNhatThongTinHoSo(NguoiDungModel nguoiDungModel);
        bool DoiMatKhau(int nguoiDungId, string matKhauCu, string matKhauMoi);
    }
    public interface ITaiKhoanServices : ISanPhamYeuThichServices, IHoSoServices
    {

    }
    public class TaiKhoanServices : ITaiKhoanServices
    {
        private readonly AppDbContext _context;
        public TaiKhoanServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<SPYeuThichModel>> LayDanhSach(int maNguoiDung)
        {
            return await _context.SPYeuThich
                .Where(spyt => spyt.MaNguoiDung == maNguoiDung)
                .Include(yt => yt.SanPham)
                .ThenInclude(sp => sp.BienThes)
                .Select(spyt => new SPYeuThichModel
                {
                    Id = spyt.Id,
                    MaNguoiDung = spyt.MaNguoiDung,
                    MaSanPham = spyt.MaSanPham,
                    NgayThem = spyt.NgayThem,
                    SanPham = new SanPhamModel
                    {
                        MaSanPham = spyt.SanPham.MaSanPham,
                        TenSanPham = spyt.SanPham.TenSanPham,
                        HinhAnhDaiDien = spyt.SanPham.HinhAnhDaiDien,
                        BienThes = spyt.SanPham.BienThes
                    }
                }).ToListAsync();
        }

        public bool XoaSanPham(int nguoiDungId, int sanPhamId)
        {
            try
            {
                var yeuThich = _context.SPYeuThich.FirstOrDefault(sp => sp.MaNguoiDung == nguoiDungId && sp.MaSanPham == sanPhamId);
                if (yeuThich != null)
                {
                    _context.SPYeuThich.Remove(yeuThich);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }
        public bool ThemSanPham(int nguoiDungId, int sanPhamId)
        {
            try
            {
                var daTonTai = KiemTraSanPhamYeuThich(nguoiDungId, sanPhamId);
                if (!daTonTai)
                {
                    var yeuThich = new SPYeuThichModel
                    {
                        MaNguoiDung = nguoiDungId,
                        MaSanPham = sanPhamId,
                        NgayThem = DateTime.Now
                    };
                    _context.SPYeuThich.Add(yeuThich);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                return false;
            }

        }

        public bool CapNhatThongTinHoSo(NguoiDungModel nguoiDungModel)
        {
            try
            {
                var nguoiDung = _context.NguoiDung.FirstOrDefault(nd => nd.MaNguoiDung == nguoiDungModel.MaNguoiDung);
                if (nguoiDung != null)
                {
                    nguoiDung.HoTen = nguoiDungModel.HoTen;
                    nguoiDung.Email = nguoiDungModel.Email;
                    nguoiDung.SoDienThoai = nguoiDungModel.SoDienThoai;
                    nguoiDung.DiaChi = nguoiDungModel.DiaChi;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public bool DoiMatKhau(int nguoiDungId, string matKhauCu, string matKhauMoi)
        {
            try
            {
                var nguoiDung = _context.NguoiDung.FirstOrDefault(nd => nd.MaNguoiDung == nguoiDungId);
                if (nguoiDung != null && nguoiDung.MatKhau == matKhauCu)
                {
                    nguoiDung.MatKhau = matKhauMoi;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public async Task<HoSoViewModel> LayThongTinHoSo(int nguoiDungId)
        {
            var nguoiDung = await _context.NguoiDung
                .Where(nd => nd.MaNguoiDung == nguoiDungId)
                .Select(nd => new NguoiDungModel
                {
                    MaNguoiDung = nd.MaNguoiDung,
                    HoTen = nd.HoTen,
                    Email = nd.Email,
                    SoDienThoai = nd.SoDienThoai,
                    DiaChi = nd.DiaChi
                })
                .FirstOrDefaultAsync();

            var donHang = await _context.DonHang
                .Where(dh => dh.MaNguoiDung == nguoiDungId)
                .ToListAsync();
            var hoSo = new HoSoViewModel
            {
                nguoiDungModels = nguoiDung,
                donHangModels = donHang
            };
            return hoSo;
        }

        public bool HuyDonHang(int donHangId, int maNguoiDung, string noiDungHuy)
        {
            Console.WriteLine($"Hủy đơn hàng: ID {donHangId}, Người dùng {maNguoiDung}, Nội dung hủy: {noiDungHuy}");
            try
            {
                var donHang = _context.DonHang.FirstOrDefault(dh => dh.ID == donHangId && dh.TrangThai == "Chờ xác nhận" && dh.MaNguoiDung == maNguoiDung);

                if (donHang != null)
                {
                    donHang.TrangThai = "Đã hủy";
                    donHang.GhiChu = noiDungHuy;
                    donHang.NgayCapNhat = DateTime.Now;
                    _context.DonHang.Update(donHang);

                    var chiTietDonHangs = _context.ChiTietDonHang
                        .Where(ct => ct.IDDonHang == donHangId)
                        .ToList();

                    foreach (var ct in chiTietDonHangs)
                    {
                        var bienThe = _context.BienThe.FirstOrDefault(bt => bt.MaBienThe == ct.MaBienThe);
                        if (bienThe != null)
                        {
                            bienThe.SoLuongConLai += ct.SoLuong;
                            _context.BienThe.Update(bienThe);
                        }
                    }
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
        }

        public bool KiemTraSanPhamYeuThich(int nguoiDungId, int sanPhamId)
        {
            return _context.SPYeuThich.Any(sp => sp.MaNguoiDung == nguoiDungId && sp.MaSanPham == sanPhamId);
        }
    }
}
