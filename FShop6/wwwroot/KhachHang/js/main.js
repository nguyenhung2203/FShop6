/*=============== SHOW MENU ===============*/
const navMenu = document.getElementById("nav-menu"),
    navToggle = document.getElementById("nav-toggle"),
    navClose = document.getElementById("nav-close");

if (navToggle) {
    navToggle.addEventListener("click", () => {
        if (navMenu) navMenu.classList.add("show-menu");
    });
}
if (navClose) {
    navClose.addEventListener("click", () => {
        if (navMenu) navMenu.classList.remove("show-menu");
    });
}

/*=============== STICKY NAVIGATION MENU LOGIC (FIXED) ===============*/
document.addEventListener('DOMContentLoaded', () => {
    const nav = document.querySelector('.nav');
    const header = document.querySelector('.header');

    if (nav && header) {
        // Lấy vị trí ban đầu của thanh menu
        const navOffsetTop = nav.offsetTop;
        // Lấy chiều cao của thanh menu
        const navHeight = nav.offsetHeight;

        window.addEventListener('scroll', () => {
            // Nếu vị trí cuộn của trang lớn hơn vị trí ban đầu của menu
            if (window.scrollY > navOffsetTop) {
                // Thêm class để "ghim" menu lại
                header.classList.add('header-fixed');
                // Thêm một khoảng đệm cho nội dung chính để không bị che
                document.body.style.paddingTop = navHeight + 'px';
            } else {
                // Gỡ bỏ class khi cuộn lên đầu
                header.classList.remove('header-fixed');
                document.body.style.paddingTop = '0';
            }
        });
    }

    // --- LOGIC MỚI: ĐẢM BẢO CHỈ CHỌN MỘT CHECKBOX ---
    categoryCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', (e) => {
            // Nếu checkbox hiện tại được tích vào
            if (e.target.checked) {
                // Bỏ tích tất cả các checkbox khác
                categoryCheckboxes.forEach(otherCheckbox => {
                    if (otherCheckbox !== e.target) {
                        otherCheckbox.checked = false;
                    }
                });
            }
        });
    });

    // ... các đoạn code khác của bạn như Active Link, Login Status ...
});
/*=============== LOGIC TỰ ĐỘNG THAY ĐỔI MÀU MENU CHO ASP.NET MVC ===============*/
document.addEventListener('DOMContentLoaded', () => {
    const navLinks = document.querySelectorAll('.nav__link');
    const currentUrl = window.location.href.toLowerCase();
    const currentPath = window.location.pathname.toLowerCase();

    // Debug - xem URL hiện tại
    console.log('Current URL:', currentUrl);
    console.log('Current Path:', currentPath);

    let isLinkActive = false;

    navLinks.forEach((link, index) => {
        const linkHref = link.href.toLowerCase();
        const linkPath = new URL(link.href).pathname.toLowerCase();

        console.log(`Link ${index + 1}:`, linkHref, 'Path:', linkPath);

        // Xóa active class trước
        link.classList.remove('active-link');

        // Kiểm tra nhiều điều kiện
        let isMatch = false;

        // 1. So sánh URL đầy đủ
        if (currentUrl === linkHref) {
            isMatch = true;
        }

        // 2. So sánh path
        if (currentPath === linkPath) {
            isMatch = true;
        }

        // 3. Kiểm tra theo controller/action cho ASP.NET MVC
        if (linkPath.includes('/trangchu') && (currentPath.includes('/trangchu') || currentPath === '/' || currentPath.includes('/index'))) {
            isMatch = true;
        }
        else if (linkPath.includes('/cuahang') && currentPath.includes('/cuahang')) {
            isMatch = true;
        }
        else if (linkPath.includes('/tintuc') && currentPath.includes('/tintuc')) {
            isMatch = true;
        }
        else if (linkPath.includes('/gioithieu') && currentPath.includes('/gioithieu')) {
            isMatch = true;
        }

        // 4. Kiểm tra theo action name trong URL
        const urlParams = new URLSearchParams(window.location.search);
        const actionFromUrl = currentPath.split('/').pop();
        const actionFromLink = linkPath.split('/').pop();

        if (actionFromUrl && actionFromLink && actionFromUrl === actionFromLink) {
            isMatch = true;
        }

        if (isMatch) {
            link.classList.add('active-link');
            isLinkActive = true;
            console.log('Active link set:', link.textContent);
        }
    });

    // Nếu không có link nào active, set trang chủ làm active
    if (!isLinkActive) {
        const homeLink = document.querySelector('.nav__link[href*="TrangChu"], .nav__link[href*="Index"]');
        if (homeLink) {
            homeLink.classList.add('active-link');
            console.log('Home link set as active');
        }
    }

    // Thêm event listener để debug khi click
    navLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            console.log('Clicked link:', this.href);
            console.log('Will navigate to:', this.href);
            // Không preventDefault() - để link hoạt động bình thường
        });
    });
});
///*=============== LOGIC HEADER TOP LOGIN/LOGOUT ===============*/
//const URL_HO_SO = "/KhachHang/TaiKhoan/HoSo";
//const URL_DANG_NHAP = "/KhachHang/TaiKhoan/DangNhap";
//const URL_DANG_KY = "/KhachHang/TaiKhoan/DangKy";

//document.addEventListener('DOMContentLoaded', () => {
//    const accountText = document.getElementById('account-text');
//    const dropdownContent = document.getElementById('account-dropdown-content');

//    if (!accountText || !dropdownContent) return;

//    const currentUserId = @HttpContext.Session.GetInt32("MaNguoiDung") ?? 0;

//    if (currentUser > 0) {
//        // Đã đăng nhập
//        accountText.textContent = `Xin chào, ${currentUser.name}`;
//        dropdownContent.innerHTML = `
//            <a href="${URL_HO_SO}">Tài khoản của tôi</a>
//            <a href="#" id="show-logout-modal" class="dropdown-link logout">Đăng xuất</a>
//        `;

//        const logoutBtn = document.getElementById('show-logout-modal');
//        if (logoutBtn) {
//            logoutBtn.addEventListener('click', (e) => {
//                e.preventDefault();
//                const modalOverlay = document.getElementById('logout-confirm-modal');
//                if (modalOverlay) {
//                    modalOverlay.classList.remove('hidden');
//                }
//            });
//        }

//    } else {
//        // Chưa đăng nhập
//        accountText.textContent = 'Tài khoản';
//        dropdownContent.innerHTML = `
//            <a href="${URL_DANG_NHAP}" class="dropdown-link">Đăng nhập</a>
//            <a href="${URL_DANG_KY}" class="dropdown-link">Đăng ký</a>
//        `;
//    }
//});


//document.addEventListener('DOMContentLoaded', function () {
//    const modalOverlay = document.getElementById('logout-confirm-modal');
//    const confirmBtn = document.getElementById('logout-confirm-btn');
//    const cancelBtn = document.getElementById('logout-cancel-btn');
//    const openModalBtn = document.getElementById('show-logout-modal'); // Nút mở modal
//    console.log({ modalOverlay, confirmBtn, cancelBtn, openModalBtn });
//    if (modalOverlay && confirmBtn && cancelBtn && openModalBtn) {
//        // 👉 MỞ modal khi nhấn nút "Đăng xuất"
//        openModalBtn.addEventListener('click', () => {
//            modalOverlay.classList.remove('hidden');
//        });

//        // 👉 Đăng xuất
//        confirmBtn.addEventListener('click', () => {
//            localStorage.removeItem('currentUser');
//            modalOverlay.classList.add('hidden');
//            window.location.reload();
//        });

//        // 👉 Hủy modal
//        cancelBtn.addEventListener('click', () => {
//            modalOverlay.classList.add('hidden');
//        });

//        // 👉 Đóng khi nhấn ra ngoài vùng modal
//        modalOverlay.addEventListener('click', (event) => {
//            if (event.target === modalOverlay) {
//                modalOverlay.classList.add('hidden');
//            }
//        });
//    } else {
//        console.warn("⚠️ Một trong các phần tử không tồn tại!");
//    }
//});

/*=============== IMAGE GALLERY ===============*/
function imgGallery() {
    const mainImg = document.querySelector(".details__img"),
        smallImg = document.querySelectorAll(".details__small-img");

    if (mainImg && smallImg.length > 0) {
        smallImg.forEach((img) => {
            img.addEventListener("click", function () {
                mainImg.src = this.src;
            });
        });
    }
}

// Gọi hàm này để kích hoạt gallery ở trang chi tiết sản phẩm
imgGallery();
/*=============== SWIPER CATEGORIES ===============*/
let swiperCategories = new Swiper(".categories__container", {
    spaceBetween: 24,
    loop: true,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },

    breakpoints: {
        350: {
            slidesPerView: 2,
            spaceBetween: 24,
        },
        768: {
            slidesPerView: 3,
            spaceBetween: 24,
        },
        992: {
            slidesPerView: 4,
            spaceBetween: 24,
        },
        1200: {
            slidesPerView: 5,
            spaceBetween: 24,
        },
        1400: {
            slidesPerView: 6,
            spaceBetween: 24,
        },
    },
});

/*=============== SWIPER PRODUCTS ===============*/
let swiperProducts = new Swiper(".new__container", {
    spaceBetween: 24,
    loop: true,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },

    breakpoints: {
        768: {
            slidesPerView: 2,
            spaceBetween: 24,
        },
        992: {
            slidesPerView: 4,
            spaceBetween: 24,
        },
        1400: {
            slidesPerView: 4,
            spaceBetween: 24,
        },
    },
});

/*=============== PRODUCTS TABS ===============*/
const tabs = document.querySelectorAll("[data-target]"),
    tabsContents = document.querySelectorAll("[content]");

tabs.forEach((tab) => {
    tab.addEventListener("click", () => {
        const target = document.querySelector(tab.dataset.target);

        tabsContents.forEach((tabsContent) => {
            tabsContent.classList.remove("active-tab");
        });

        target.classList.add("active-tab");

        tabs.forEach((tab) => {
            tab.classList.remove("active-tab");
        });

        tab.classList.add("active-tab");
    });
});

// Đợi cho toàn bộ nội dung trang HTML được tải xong rồi mới chạy code
document.addEventListener("DOMContentLoaded", function () {
    /**
     * Hàm chung để xử lý việc chuyển đổi class 'active' cho một nhóm các phần tử.
     * Khi một phần tử được nhấp, nó sẽ có class active, và các phần tử khác trong nhóm sẽ mất class đó.
     *
     * @param {string} selector - CSS selector cho các phần tử (ví dụ: '.size__link').
     * @param {string} activeClass - Tên của class active cần thêm/xóa (ví dụ: 'size-active').
     */
    function setupActiveClassToggle(selector, activeClass) {
        const elements = document.querySelectorAll(selector);

        if (elements.length > 0) {
            elements.forEach((element) => {
                element.addEventListener("click", function (event) {
                    // Ngăn hành động mặc định của thẻ <a>
                    event.preventDefault();

                    // Xóa class active khỏi tất cả các phần tử trong nhóm
                    elements.forEach((el) => {
                        el.classList.remove(activeClass);
                    });

                    // Thêm class active vào phần tử vừa được nhấn
                    this.classList.add(activeClass);
                });
            });
        }
    }

    // Áp dụng hàm cho việc chọn size
    setupActiveClassToggle(".size__link", "size-active");

    // Áp dụng hàm cho việc chọn màu
    setupActiveClassToggle(".color__link", "color-active");
});



(function () {
    const duongDanCoSo = '/KhachHang/images/';

    // --- SLIDER ---
    let danhSachAnhLon = [];
    let chiSoHienTai = 0;

    function hienThiSlide(chiSo) {
        if (!danhSachAnhLon.length) return;
        danhSachAnhLon.forEach((img, i) => img.classList.toggle('active', i === chiSo));
        chiSoHienTai = chiSo;
    }

    function hienThiSlideTheoTenFile(tenFile) {
        if (!tenFile) return;

        const viTri = danhSachAnhLon.findIndex(img => img.getAttribute('src').endsWith(tenFile));
        if (viTri >= 0) {
            hienThiSlide(viTri);
        }
    }

    function ganNutChuyenSlide() {
        document.getElementById('prev-btn')?.addEventListener('click', () => {
            hienThiSlide((chiSoHienTai - 1 + danhSachAnhLon.length) % danhSachAnhLon.length);
        });
        document.getElementById('next-btn')?.addEventListener('click', () => {
            hienThiSlide((chiSoHienTai + 1) % danhSachAnhLon.length);
        });
    }

    // --- CẬP NHẬT THEO BIẾN THỂ ---
    // Gộp logic cập nhật giá/sku/tồn + chuyển ảnh
    window.updateProductDetails = function (element) {
        var maBienThe = element.getAttribute("data-id");
        var giaBan = parseInt(element.getAttribute("data-giaban"));
        var soLuong = element.getAttribute("data-soluong");
        var maSku = element.getAttribute("data-sku");
        var giamGia = parseInt(element.getAttribute("data-giamgia")) || 0;
        var hinhAnh = element.getAttribute("data-hinhanh");

        // Tính giá & hiển thị
        document.getElementById('nhap-so-luong').max = soLuong;
        document.getElementById('product-sku').textContent = maSku;
        document.getElementById('product-status').textContent = 'Còn ' + soLuong + ' sản phẩm';

        if (giamGia > 0) {
            var giaKhiGiam = (giaBan * (100 - giamGia)) / 100;
            document.getElementById('product-discount').textContent = giaBan.toLocaleString('vi-VN') + "đ";
            document.getElementById('product-price').textContent = giaKhiGiam.toLocaleString('vi-VN') + "đ";
        } else {
            document.getElementById('product-discount').textContent = '';
            document.getElementById('product-price').textContent = giaBan.toLocaleString('vi-VN') + "đ";
        }
        document.getElementById('hiddenMaBienThe').value = maBienThe;
        
        // Chuyển slide theo ảnh của biến thể
        hienThiSlideTheoTenFile(hinhAnh);
    };

    // --- KHỞI TẠO ---
    window.addEventListener('load', function () {
        danhSachAnhLon = Array.from(document.querySelectorAll('.details__big-img'));
        if (!danhSachAnhLon.length) return;

        // Hiển thị ảnh đầu tiên
        hienThiSlide(0);
        ganNutChuyenSlide();

        // 👉 Làm mờ và disable tất cả biến thể có số lượng = 0
        const listBienThe = document.querySelectorAll('.size__list a');
        let bienTheCoSan = null;

        listBienThe.forEach(link => {
            const soLuong = parseInt(link.getAttribute("data-soluong")) || 0;
            if (soLuong <= 0) {
                link.classList.add("out-of-stock");
                link.removeAttribute("onclick");
                link.style.opacity = "0.5";
                link.style.pointerEvents = "none";
                link.style.cursor = "not-allowed";
            } else if (!bienTheCoSan) {
                bienTheCoSan = link;
            }
        });

        
        // Nếu có ít nhất 1 biến thể còn hàng → click vào nó
        if (bienTheCoSan) {
            bienTheCoSan.click();
            document.getElementById('add-to-cart-btn')?.removeAttribute('disabled');
        } else {
            // Nếu tất cả đều hết hàng → disable nút Thêm giỏ hàng
            document.getElementById('add-to-cart-btn')?.setAttribute('disabled', 'disabled');
            document.getElementById('het-hang-overlay').textContent = "Hết hàng";
            document.getElementById('het-hang-overlay')?.classList.add('het-hang-hidden');
        }
    });

})();
