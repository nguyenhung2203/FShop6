using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text;

namespace FShop6.Areas.KhachHang.Services
{
    public class DichVuAIThongMinh
    {
        private readonly IConfiguration _cauHinh;
        private readonly HttpClient _httpClient;
        private readonly string _geminiApiKey;
        private readonly string _geminiApiUrl;

        public DichVuAIThongMinh(IConfiguration cauHinh, HttpClient httpClient)
        {
            _cauHinh = cauHinh;
            _httpClient = httpClient;

            // Lấy API key từ appsettings.json
            _geminiApiKey = _cauHinh["GeminiAI:ApiKey"];
            _geminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent";

            if (string.IsNullOrEmpty(_geminiApiKey))
            {
                throw new ArgumentException("Vui lòng cấu hình GeminiAI:ApiKey trong appsettings.json");
            }
        }

        // Thay thế hàm LayCauTraLoi trong DichVuAIThongMinh.cs
        public async Task<string> LayCauTraLoi(string cauHoiNguoiDung)
        {
            try
            {
                Console.WriteLine($"=== BẮT ĐẦU XỬ LÝ CÂU HỎI ===");
                Console.WriteLine($"Câu hỏi: {cauHoiNguoiDung}");

                // Bước 1: Tạo prompt hướng dẫn cho AI
                string promptHuongDan = TaoPromptHuongDanChoAI();

                // Bước 2: Gửi câu hỏi cho Gemini để nhận SQL
                string cauLenhSql = await LayCauLenhSqlTuGemini(promptHuongDan, cauHoiNguoiDung);

                Console.WriteLine($"SQL được AI tạo: {cauLenhSql}");

                // Kiểm tra SQL có hợp lệ không
                if (string.IsNullOrEmpty(cauLenhSql) ||
                    cauLenhSql.ToLower().Contains("không thể tạo") ||
                    cauLenhSql.ToLower().Contains("sorry") ||
                    !cauLenhSql.ToUpper().Contains("SELECT"))
                {
                    Console.WriteLine("❌ SQL không hợp lệ!");
                    return "Xin lỗi, tôi chưa hiểu rõ câu hỏi của bạn. Bạn có thể hỏi về sản phẩm, giá cả, số lượng tồn kho, hoặc danh mục sản phẩm được không?";
                }

                Console.WriteLine("✅ SQL hợp lệ, bắt đầu thực thi...");

                // Bước 3: Thực thi SQL để lấy dữ liệu
                DataTable ketQuaTuDb = await ThucThiCauLenhSqlAsync(cauLenhSql);

                Console.WriteLine($"📊 Số dòng kết quả: {ketQuaTuDb.Rows.Count}");
                Console.WriteLine($"📊 Số cột: {ketQuaTuDb.Columns.Count}");

                if (ketQuaTuDb.Columns.Count > 0)
                {
                    Console.WriteLine("📋 Tên các cột:");
                    foreach (DataColumn col in ketQuaTuDb.Columns)
                    {
                        Console.WriteLine($"   - {col.ColumnName} ({col.DataType.Name})");
                    }
                }

                if (ketQuaTuDb.Rows.Count > 0)
                {
                    Console.WriteLine("💾 Dữ liệu mẫu (3 dòng đầu):");
                    for (int i = 0; i < Math.Min(3, ketQuaTuDb.Rows.Count); i++)
                    {
                        var row = ketQuaTuDb.Rows[i];
                        var rowData = new List<string>();
                        foreach (var item in row.ItemArray)
                        {
                            rowData.Add(item?.ToString() ?? "NULL");
                        }
                        Console.WriteLine($"   Row {i + 1}: {string.Join(" | ", rowData)}");
                    }
                }
                else
                {
                    Console.WriteLine("⚠️ Không có dữ liệu nào được trả về!");

                    // Test thử câu SQL đơn giản
                    Console.WriteLine("🔍 Đang test câu SQL cơ bản...");
                    var testSql = "SELECT TOP 3 * FROM SanPham";
                    var testResult = await ThucThiCauLenhSqlAsync(testSql);
                    Console.WriteLine($"📊 Test SanPham: {testResult.Rows.Count} dòng");

                    if (testResult.Rows.Count > 0)
                    {
                        Console.WriteLine("✅ Database có dữ liệu SanPham");
                        for (int i = 0; i < Math.Min(2, testResult.Rows.Count); i++)
                        {
                            var row = testResult.Rows[i];
                            Console.WriteLine($"   {string.Join(" | ", row.ItemArray.Select(x => x?.ToString() ?? "NULL"))}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("❌ Database không có dữ liệu trong bảng SanPham");
                    }
                }

                // Bước 4: Cho Gemini diễn giải kết quả thành câu trả lời thân thiện
                Console.WriteLine("🤖 Đang cho AI diễn giải kết quả...");
                string cauTraLoiCuoiCung = await DienGiaiKetQuaThanhCauTraLoiVoiGemini(cauHoiNguoiDung, ketQuaTuDb);

                Console.WriteLine($"💬 Câu trả lời cuối: {cauTraLoiCuoiCung}");
                Console.WriteLine($"=== KẾT THÚC XỬ LÝ ===\n");

                return cauTraLoiCuoiCung;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi trong LayCauTraLoi: {ex.Message}");
                Console.WriteLine($"📍 Stack Trace: {ex.StackTrace}");
                return "Xin lỗi, có lỗi xảy ra. Vui lòng thử lại sau!";
            }
        }

        // Cải thiện prompt cho AI để tạo SQL chính xác hơn
        private string TaoPromptHuongDanChoAI()
        {
            return @"
            Bạn là chuyên gia SQL Server cho cửa hàng. Tạo câu lệnh SQL chính xác từ câu hỏi tiếng Việt.

            CẤU TRÚC DATABASE:
            1. SanPham: MaSanPham (int), TenSanPham (nvarchar), MaDanhMucSP (int)
            2. BienThe: MaBienThe (int), MaSanPham (int), GiaBan (decimal), SoLuongConLai (int)  
            3. DanhMucSP: MaDanhMucSP (int), TenDanhMuc (nvarchar)

            QUY TẮC QUAN TRỌNG:
            - CHỈ trả về câu lệnh SQL thuần túy, không thêm giải thích
            - Nếu không hiểu: trả về 'Không thể tạo SQL'
            - Luôn dùng INNER JOIN khi cần liên kết
            - Tìm kiếm: dùng LIKE N'%từ_khóa%' (không phân biệt hoa thường)
            - Giới hạn kết quả: thêm TOP 10
            ";
        }

        private async Task<string> LayCauLenhSqlTuGemini(string promptHuongDan, string cauHoi)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = $"{promptHuongDan}\n\nCâu hỏi: {cauHoi}" }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.1,
                        maxOutputTokens = 200
                    }
                };

                string jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                string url = $"{_geminiApiUrl}?key={_geminiApiKey}";
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var geminiResponse = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

                    if (geminiResponse.TryGetProperty("candidates", out var candidates) &&
                        candidates.GetArrayLength() > 0)
                    {
                        var firstCandidate = candidates[0];
                        if (firstCandidate.TryGetProperty("content", out var content_prop) &&
                            content_prop.TryGetProperty("parts", out var parts) &&
                            parts.GetArrayLength() > 0)
                        {
                            var sqlText = parts[0].GetProperty("text").GetString();
                            return sqlText?.Trim() ?? "Không thể tạo SQL";
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Lỗi gọi Gemini API: {response.StatusCode}");
                }

                return "Không thể tạo SQL";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi LayCauLenhSqlTuGemini: {ex.Message}");
                return "Không thể tạo SQL";
            }
        }

        // Thêm vào đầu hàm ThucThiCauLenhSqlAsync trong DichVuAIThongMinh.cs
        private async Task<DataTable> ThucThiCauLenhSqlAsync(string cauLenh)
        {
            DataTable bangDuLieu = new DataTable();
            string chuoiKetNoi = _cauHinh.GetConnectionString("DefaultConnection");

            // DEBUG: In ra connection string để kiểm tra
            Console.WriteLine($"🔗 Connection String: {chuoiKetNoi}");

            try
            {
                using (SqlConnection ketNoi = new SqlConnection(chuoiKetNoi))
                {
                    Console.WriteLine("🔄 Đang mở kết nối database...");
                    await ketNoi.OpenAsync();
                    Console.WriteLine("✅ Kết nối database thành công!");

                    // Kiểm tra SQL injection cơ bản
                    if (KiemTraSqlInjection(cauLenh))
                    {
                        Console.WriteLine("⚠️ Phát hiện SQL có thể không an toàn");
                        return bangDuLieu;
                    }

                    Console.WriteLine($"📝 Executing SQL: {cauLenh}");

                    using (SqlCommand lenh = new SqlCommand(cauLenh, ketNoi))
                    {
                        lenh.CommandTimeout = 30; // 30 giây timeout
                        using (SqlDataAdapter boChuyenDoi = new SqlDataAdapter(lenh))
                        {
                            boChuyenDoi.Fill(bangDuLieu);
                            Console.WriteLine($"✅ SQL executed successfully. Rows: {bangDuLieu.Rows.Count}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi thực thi SQL: {ex.Message}");
                Console.WriteLine($"📍 Full Exception: {ex}");
            }

            return bangDuLieu;
        }

        private bool KiemTraSqlInjection(string cauLenh)
        {
            // Các từ khóa nguy hiểm
            string[] tuKhoaNguyHiem = {
                "DROP", "DELETE", "UPDATE", "INSERT", "ALTER",
                "CREATE", "EXEC", "EXECUTE", "sp_", "xp_", "--", "/*", "*/"
            };

            string cauLenhHoa = cauLenh.ToUpper();
            foreach (string tuKhoa in tuKhoaNguyHiem)
            {
                if (cauLenhHoa.Contains(tuKhoa))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<string> DienGiaiKetQuaThanhCauTraLoiVoiGemini(string cauHoi, DataTable duLieu)
        {
            try
            {
                // Chuyển DataTable thành JSON để gửi cho Gemini
                string duLieuJson = ChuyenDataTableThanhJson(duLieu);

                string promptDienGiai = $@"
                                        Bạn là trợ lý bán hàng của FShop6. Hãy diễn giải kết quả database thành câu trả lời thân thiện, tự nhiên bằng tiếng Việt.

                                        Câu hỏi khách hàng: {cauHoi}
                                        Dữ liệu từ database: {duLieuJson}

                                        YÊU CẦU:
                                        - Trả lời ngắn gọn, thân thiện như nhân viên bán hàng
                                        - Nếu có nhiều sản phẩm, chỉ hiển thị 3-5 sản phẩm đầu tiên
                                        - Định dạng giá tiền: 100,000đ
                                        - Nếu không có dữ liệu: 'Xin lỗi, hiện tại chúng tôi chưa có sản phẩm này.'
                                        - Luôn kết thúc bằng câu hỏi gợi ý: 'Bạn có cần tư vấn thêm không?'
                ";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = promptDienGiai }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.7,
                        maxOutputTokens = 300
                    }
                };

                string jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                string url = $"{_geminiApiUrl}?key={_geminiApiKey}";
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var geminiResponse = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

                    if (geminiResponse.TryGetProperty("candidates", out var candidates) &&
                        candidates.GetArrayLength() > 0)
                    {
                        var firstCandidate = candidates[0];
                        if (firstCandidate.TryGetProperty("content", out var content_prop) &&
                            content_prop.TryGetProperty("parts", out var parts) &&
                            parts.GetArrayLength() > 0)
                        {
                            return parts[0].GetProperty("text").GetString() ?? TaoTraLoiMacDinh(duLieu);
                        }
                    }
                }

                // Fallback nếu Gemini không phản hồi
                return TaoTraLoiMacDinh(duLieu);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi DienGiaiKetQuaThanhCauTraLoiVoiGemini: {ex.Message}");
                return TaoTraLoiMacDinh(duLieu);
            }
        }

        private string ChuyenDataTableThanhJson(DataTable duLieu)
        {
            if (duLieu.Rows.Count == 0)
            {
                return "Không có dữ liệu";
            }

            var danhSach = new List<Dictionary<string, object>>();

            foreach (DataRow hang in duLieu.Rows)
            {
                var dictionary = new Dictionary<string, object>();
                foreach (DataColumn cot in duLieu.Columns)
                {
                    dictionary[cot.ColumnName] = hang[cot] ?? "";
                }
                danhSach.Add(dictionary);
            }

            return JsonSerializer.Serialize(danhSach, new JsonSerializerOptions
            {
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        private string TaoTraLoiMacDinh(DataTable duLieu)
        {
            if (duLieu.Rows.Count == 0)
            {
                return "Xin lỗi, hiện tại chúng tôi chưa có sản phẩm này. Bạn có thể tham khảo các sản phẩm khác không?";
            }

            // Tạo câu trả lời đơn giản từ dữ liệu
            var hang = duLieu.Rows[0];
            var tenSanPham = hang.Table.Columns.Contains("TenSanPham") ? hang["TenSanPham"].ToString() : "";

            if (hang.Table.Columns.Contains("GiaBan"))
            {
                var giaBan = Convert.ToDecimal(hang["GiaBan"]);
                return $"Sản phẩm '{tenSanPham}' có giá {giaBan:N0}đ. Bạn có cần tư vấn thêm không?";
            }
            else if (hang.Table.Columns.Contains("SoLuongConLai"))
            {
                var soLuong = Convert.ToInt32(hang["SoLuongConLai"]);
                return $"Hiện tại '{tenSanPham}' còn {soLuong} sản phẩm. Bạn có muốn đặt hàng không?";
            }

            return "Tôi đã tìm thấy thông tin sản phẩm. Bạn có cần tư vấn thêm không?";
        }
    }
}