document.addEventListener("DOMContentLoaded", function () {
    const rawData = document.getElementById("yearChartData");
    if (!rawData) return;

    const yearData = JSON.parse(rawData.textContent);
    console.log(yearData); // Xem dữ liệu JSON đã được lấy lên

    const labels = yearData.map(item => `Tháng: ` + item.thang);
    const data = yearData.map(item => item.tongDoanhThu);

    const ctx = document.getElementById("yearChart").getContext("2d");

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Tổng doanh thu theo tháng',
                data: data,
                backgroundColor: 'rgba(0, 166, 63, 0.7)',
                borderColor: 'rgba(0, 166, 63, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return value.toLocaleString('vi-VN') + ' đ';
                        }
                    }
                }
            }
        }
    });
});
