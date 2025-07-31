using FShop6.Areas.Admin.Services;
using FShop6.Areas.KhachHang.Services;
using FShop6.Data;
using FShop6.Hubs;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);

//connection db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// 🟢 Đăng ký dịch vụ TrangChuService
builder.Services.AddScoped<IShopService, ShopService>();
// Đảm bảo đăng ký dịch vụ đúng cách
builder.Services.AddScoped<ITrangChuService, TrangChuServices>();
// 🟢 Đăng ký dịch vụ HeaderServices
builder.Services.AddScoped<IHeaderServices, HeaderServices>();
// 🟢 Đăng ký dịch vụ CuaHangService
builder.Services.AddScoped<ICuaHangServices, ShopService>();
// 🟢 Đăng ký dịch vụ TaiKhoanService
builder.Services.AddScoped<ITaiKhoanServices, TaiKhoanServices>();
// 🟢 Đăng ký dịch vụ GioHangService
builder.Services.AddScoped<IGioHangServices, GioHangServices>();
// 🟢 Đăng ký dịch vụ DichVuAIThongMinh
builder.Services.AddHttpClient<DichVuAIThongMinh>();

// 🟢 Đăng ký dịch vụ ThongKeService
builder.Services.AddScoped<IThongKeServices, ThongKeServices>();

// 🟢 Đăng ký dịch vụ QuanLyDonHangService
builder.Services.AddScoped<IQuanLyDonHangServices, QuanLyDonHangServices>();

// 🟢 Đăng ký dịch vụ QuanLySanPhamService
builder.Services.AddScoped<IQuanLySanPhamServices, QuanLySanPhamServices>();

// Đăng ký DichVuAIThongMinh như Scoped service
builder.Services.AddScoped<DichVuAIThongMinh>();
var app = builder.Build();

// Cấu hình pipelinex
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapHub<ChatHub>("/chatHub");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 🟡 Định tuyến cho khu vực (Areas) — Quan trọng
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}"
);

// 🔵 Định tuyến mặc định: Chuyển hướng về KhachHang/TrangChu/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}",
    defaults: new { area = "KhachHang" } // 🟢 Mặc định dùng Area KhachHang
);

app.Run();
