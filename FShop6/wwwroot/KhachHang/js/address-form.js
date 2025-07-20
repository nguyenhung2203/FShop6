document.addEventListener('DOMContentLoaded', function () {
    const host = "https://provinces.open-api.vn/api/";
    const citySelect = document.getElementById("city-province");
    const districtSelect = document.getElementById("district");
    const wardSelect = document.getElementById("ward");

    // Hàm gọi API để lấy danh sách Tỉnh/Thành phố
    function callAPICity() {
        return axios.get(host + "?depth=1")
            .then(response => {
                printCity(response.data);
            })
            .catch(error => console.error('Lỗi khi tải danh sách tỉnh/thành phố:', error));
    }

    // Hàm gọi API để lấy danh sách Quận/Huyện từ Tỉnh/Thành phố đã chọn
    function callApiDistrict(cityCode) {
        return axios.get(host + "p/" + cityCode + "?depth=2")
            .then(response => {
                printDistrict(response.data.districts);
            })
            .catch(error => console.error('Lỗi khi tải danh sách quận/huyện:', error));
    }

    // Hàm gọi API để lấy danh sách Phường/Xã từ Quận/Huyện đã chọn
    function callApiWard(districtCode) {
        return axios.get(host + "d/" + districtCode + "?depth=2")
            .then(response => {
                printWard(response.data.wards);
            })
            .catch(error => console.error('Lỗi khi tải danh sách phường/xã:', error));
    }

    // Hàm hiển thị danh sách Tỉnh/Thành phố ra dropdown
    function printCity(data) {
        data.forEach(city => {
            citySelect.innerHTML += `<option value="${city.code}">${city.name}</option>`;
        });
    }

    // Hàm hiển thị danh sách Quận/Huyện ra dropdown
    function printDistrict(data) {
        districtSelect.innerHTML = "<option value='' disabled selected>Chọn Quận/Huyện</option>";
        data.forEach(district => {
            districtSelect.innerHTML += `<option value="${district.code}">${district.name}</option>`;
        });
    }

    // Hàm hiển thị danh sách Phường/Xã ra dropdown
    function printWard(data) {
        wardSelect.innerHTML = "<option value='' disabled selected>Chọn Phường/Xã</option>";
        data.forEach(ward => {
            wardSelect.innerHTML += `<option value="${ward.code}">${ward.name}</option>`;
        });
    }

    // Bắt sự kiện khi người dùng chọn một Tỉnh/Thành phố
    if (citySelect) {
        citySelect.addEventListener("change", () => {
            const cityCode = citySelect.value;
            callApiDistrict(cityCode);
            wardSelect.innerHTML = "<option value='' disabled selected>Chọn Phường/Xã</option>"; // Reset phường/xã
        });
    }

    // Bắt sự kiện khi người dùng chọn một Quận/Huyện
    if (districtSelect) {
        districtSelect.addEventListener("change", () => {
            const districtCode = districtSelect.value;
            callApiWard(districtCode);
        });
    }

    // Gọi hàm để tải danh sách Tỉnh/Thành phố ngay khi trang được tải
    callAPICity();
});