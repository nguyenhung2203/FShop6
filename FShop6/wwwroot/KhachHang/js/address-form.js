document.addEventListener('DOMContentLoaded', function () {
    const host = "https://provinces.open-api.vn/api/";
    const citySelect = document.getElementById("city-province");
    const districtSelect = document.getElementById("district");
    const wardSelect = document.getElementById("ward");

    // Lưu trữ dữ liệu để mapping code -> name
    let citiesData = [];
    let districtsData = [];
    let wardsData = [];

    // Hàm gọi API để lấy danh sách Tỉnh/Thành phố
    function callAPICity() {
        return axios.get(host + "?depth=1")
            .then(response => {
                citiesData = response.data;
                printCity(response.data);
            })
            .catch(error => console.error('Lỗi khi tải danh sách tỉnh/thành phố:', error));
    }

    // Hàm gọi API để lấy danh sách Quận/Huyện từ Tỉnh/Thành phố đã chọn
    function callApiDistrict(cityCode) {
        return axios.get(host + "p/" + cityCode + "?depth=2")
            .then(response => {
                districtsData = response.data.districts;
                printDistrict(response.data.districts);
            })
            .catch(error => console.error('Lỗi khi tải danh sách quận/huyện:', error));
    }

    // Hàm gọi API để lấy danh sách Phường/Xã từ Quận/Huyện đã chọn
    function callApiWard(districtCode) {
        return axios.get(host + "d/" + districtCode + "?depth=2")
            .then(response => {
                wardsData = response.data.wards;
                printWard(response.data.wards);
            })
            .catch(error => console.error('Lỗi khi tải danh sách phường/xã:', error));
    }

    // Hàm hiển thị danh sách Tỉnh/Thành phố ra dropdown - DÙNG TÊN LÀM VALUE
    function printCity(data) {
        // Kiểm tra xem đã có options chưa, nếu chưa thì mới thêm
        if (citySelect.options.length <= 1) {
            data.forEach(city => {
                const option = document.createElement('option');
                option.value = city.name;
                option.setAttribute('data-code', city.code);
                option.textContent = city.name;
                citySelect.appendChild(option);
            });
        }
    }

    // Hàm hiển thị danh sách Quận/Huyện ra dropdown - DÙNG TÊN LÀM VALUE
    function printDistrict(data) {
        districtSelect.innerHTML = "<option value='' disabled selected>Chọn Quận/Huyện</option>";
        data.forEach(district => {
            // QUAN TRỌNG: Dùng district.name làm value thay vì district.code
            districtSelect.innerHTML += `<option value="${district.name}" data-code="${district.code}">${district.name}</option>`;
        });
    }

    // Hàm hiển thị danh sách Phường/Xã ra dropdown - DÙNG TÊN LÀM VALUE
    function printWard(data) {
        wardSelect.innerHTML = "<option value='' disabled selected>Chọn Phường/Xã</option>";
        data.forEach(ward => {
            // QUAN TRỌNG: Dùng ward.name làm value thay vì ward.code
            wardSelect.innerHTML += `<option value="${ward.name}" data-code="${ward.code}">${ward.name}</option>`;
        });
    }

    // Bắt sự kiện khi người dùng chọn một Tỉnh/Thành phố
    if (citySelect) {
        citySelect.addEventListener("change", () => {
            // Lấy code từ data-code để call API
            const selectedOption = citySelect.options[citySelect.selectedIndex];
            const cityCode = selectedOption.getAttribute('data-code');

            console.log('Selected city:', citySelect.value, 'Code:', cityCode);

            callApiDistrict(cityCode);
            wardSelect.innerHTML = "<option value='' disabled selected>Chọn Phường/Xã</option>";
        });
    }

    // Bắt sự kiện khi người dùng chọn một Quận/Huyện
    if (districtSelect) {
        districtSelect.addEventListener("change", () => {
            // Lấy code từ data-code để call API
            const selectedOption = districtSelect.options[districtSelect.selectedIndex];
            const districtCode = selectedOption.getAttribute('data-code');

            console.log('Selected district:', districtSelect.value, 'Code:', districtCode);

            callApiWard(districtCode);
        });
    }

    // Bắt sự kiện khi người dùng chọn Phường/Xã
    if (wardSelect) {
        wardSelect.addEventListener("change", () => {
            console.log('Selected ward:', wardSelect.value);
        });
    }

    // Gọi hàm để tải danh sách Tỉnh/Thành phố ngay khi trang được tải
    callAPICity();
});