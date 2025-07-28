using FShop6.Areas.KhachHang.Services;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

//connection db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

// 🟢 Đăng ký dịch vụ TrangChuService
builder.Services.AddScoped<IShopService, ShopService>();
// Đảm bảo đăng ký dịch vụ đúng cách
builder.Services.AddScoped<ITrangChuService, TrangChuServices>();

// 🟢 Đăng ký dịch vụ CuaHangService
// 🟢 Đăng ký dịch vụ TaiKhoanService
builder.Services.AddScoped<ITaiKhoanServices, TaiKhoanServices>();

var app = builder.Build();

// Cấu hình pipelinex
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 🟡 Định tuyến cho khu vực (Areas) — Quan trọng
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=CuaHang}/{action=ChiTietSanPham}/{id?}"
);

// 🔵 Định tuyến mặc định: Chuyển hướng về KhachHang/TrangChu/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ThongKe}/{action=ThongKe}/{id?}",
    defaults: new { area = "Admin" } // 🟢 Mặc định dùng Area KhachHang
);

app.Run();
