using FShop6.Data;
using FShop6.Areas.KhachHang.Services;
using FShop6.Areas.Admin.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// 👉 1. Cấu hình kết nối cơ sở dữ liệu
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 👉 2. Cấu hình dịch vụ MVC
builder.Services.AddControllersWithViews();

// 👉 3. Đăng ký các service dùng Dependency Injection
builder.Services.AddScoped<ITrangChuService, TrangChuServices>();
builder.Services.AddScoped<ICuaHangServices, CuaHangServices>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddScoped<IQuanLyTinTucService, QuanLyTinTucService>();





var app = builder.Build();

// 👉 4. Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Nếu có xác thực, còn không thì có thể bỏ dòng này
app.UseAuthorization();

// 👉 5. Định tuyến cho Areas (ưu tiên định tuyến cho Admin)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=QuanLyTinTuc}/{action=QuanLyTinTuc}/{id?}"
);

// 👉 6. Định tuyến mặc định cho ứng dụng
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=QuanLyTinTuc}/{action=QuanLyTinTuc}/{id?}",
    defaults: new { area = "Admin" }
);
app.Run();
