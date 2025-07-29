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
builder.Services.AddScoped<ITrangChuService, TrangChuServices>();
// 🟢 Đăng ký dịch vụ CuaHangService
builder.Services.AddScoped<ICuaHangServices, CuaHangServices>();
// 🟢 Đăng ký dịch vụ SanPhamYeuThichService
builder.Services.AddScoped<ISanPhamYeuThichServices, SanPhamYeuThichServices>();

var app = builder.Build();

// Cấu hình pipeline
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
    pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}"
);

// 🔵 Định tuyến mặc định: Chuyển hướng về KhachHang/TrangChu/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}",
    defaults: new { area = "KhachHang" } // 🟢 Mặc định dùng Area KhachHang
);

app.Run();
