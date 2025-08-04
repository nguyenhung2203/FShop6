using FShop6.Areas.Admin.Services;
using FShop6.Areas.Admin.Services.Implementations;
using FShop6.Areas.KhachHang.Services;
using FShop6.Data;
using FShop6.Hubs;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);

// Kết nối cơ sở dữ liệu SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Thêm cấu hình Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session hết hạn sau 30 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.Secure = CookieSecurePolicy.SameAsRequest; // Chấp nhận http
});
// Đăng ký các service
builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();
builder.Services.AddScoped<ITrangChuAdminServices, TrangChuAdminService>();
builder.Services.AddScoped<IHoSoService, HoSoService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
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
// 🟢 Đăng ký dịch vụ TinTucService
builder.Services.AddScoped<ITinTucService, TinTucService>();

// Đăng ký DichVuAIThongMinh như Scoped service
builder.Services.AddScoped<DichVuAIThongMinh>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseRouting();

app.UseCookiePolicy();

// Bắt buộc thêm dòng này để Session hoạt động
app.UseSession();

app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");
app.UseHttpsRedirection();
app.UseStaticFiles();





// Định tuyến cho Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}",
    defaults: new { area = "KhachHang" });

app.Run();
