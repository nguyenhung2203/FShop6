using FShop6.Data;
using FShop6.Areas.KhachHang.Services; // Thêm using cho DichVuAIThongMinh
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FShop6.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly DichVuAIThongMinh _dichVuAI;

        public ChatHub(AppDbContext context, DichVuAIThongMinh dichVuAI)
        {
            _context = context;
            _dichVuAI = dichVuAI;
        }

        // Hàm này được gọi từ JavaScript với tên chính xác
        public async Task GuiTinNhan(string tinNhan)
        {
            try
            {
                // 1. Gửi tin nhắn của người dùng lên giao diện ngay lập tức
                await Clients.Caller.SendAsync("NhanTinNhan", "Bạn", tinNhan);

                // 2. Hiển thị trạng thái "đang xử lý"
                await Clients.Caller.SendAsync("NhanTinNhan", "F6 Bot", "Đang xử lý...");

                // 3. HỎI Ý KIẾN "CHUYÊN GIA AI GEMINI"
                var cauTraLoiAI = await _dichVuAI.LayCauTraLoi(tinNhan);

                // 4. Xóa tin nhắn "đang xử lý" và gửi câu trả lời thực
                await Clients.Caller.SendAsync("ThayTheTinNhanCuoi", "F6 Bot", cauTraLoiAI);

                // 5. LƯU TIN NHẮN VÀO DATABASE (tùy chọn)
                // var tinNhanNguoiDung = new TinNhan 
                // { 
                //     TenNguoiGui = "Khách hàng", 
                //     NoiDung = tinNhan, 
                //     ThoiGian = DateTime.Now 
                // };
                // _context.TinNhans.Add(tinNhanNguoiDung);

                // var tinNhanBot = new TinNhan 
                // { 
                //     TenNguoiGui = "F6 Bot", 
                //     NoiDung = cauTraLoiAI, 
                //     ThoiGian = DateTime.Now 
                // };
                // _context.TinNhans.Add(tinNhanBot);
                // await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong ChatHub: {ex.Message}");
                await Clients.Caller.SendAsync("NhanTinNhan", "F6 Bot",
                    "Xin lỗi, có lỗi xảy ra. Vui lòng thử lại sau!");
            }
        }

        // Hàm xử lý khi client kết nối
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("NhanTinNhan", "F6 Bot",
                "Chào mừng bạn đến với F6 Shop! Tôi có thể giúp bạn tìm hiểu về sản phẩm, giá cả và tồn kho. Bạn cần hỗ trợ gì?");
            await base.OnConnectedAsync();
        }

        // Hàm xử lý khi client ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}