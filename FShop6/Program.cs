using FShop6.Areas.Admin.Services;
using FShop6.Areas.Admin.Services.Implementations;
using FShop6.Areas.KhachHang.Services;
using FShop6.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kết nối cơ sở dữ liệu SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

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
builder.Services.AddScoped<ICuaHangServices, CuaHangServices>();
builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();
builder.Services.AddScoped<ITrangChuAdminServices, TrangChuAdminService>();
builder.Services.AddScoped<IHoSoService, HoSoService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();

var app = builder.Build();

// Middleware pipeline
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

app.UseHttpsRedirection();
app.UseStaticFiles();





// Định tuyến cho Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

app.Run();
