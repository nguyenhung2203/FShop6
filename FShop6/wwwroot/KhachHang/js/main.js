/*=============== SHOW MENU ===============*/
const navMenu = document.getElementById("nav-menu"),
    navToggle = document.getElementById("nav-toggle"),
    navClose = document.getElementById("nav-close");

/*===== Menu Show =====*/
if (navToggle) {
    navToggle.addEventListener("click", () => {
        if (navMenu) navMenu.classList.add("show-menu");
    });
}

/*===== Hide Show =====*/
if (navClose) {
    navClose.addEventListener("click", () => {
        if (navMenu) navMenu.classList.remove("show-menu");
    });
}
/*=============== LOGIC HEADER TOP LOGIN/LOGOUT ===============*/
document.addEventListener('DOMContentLoaded', () => {
    setTimeout(() => {
        const accountText = document.getElementById('account-text');
        const dropdownContent = document.getElementById('account-dropdown-content');
        if (!accountText || !dropdownContent) {
            console.error('Account text or dropdown content not found');
            return;
        }
        // Gán dữ liệu giả cho currentUser nếu chưa có (chỉ để test)
        if (!localStorage.getItem('currentUser')) {
            const fakeUser = {
                name: "Hùng",
                email: "hung@example.com",
                role: "user"
            };
            localStorage.setItem('currentUser', JSON.stringify(fakeUser));
        }

        const currentUser = JSON.parse(localStorage.getItem('currentUser'));

        if (currentUser) {
            // ĐÃ ĐĂNG NHẬP
            accountText.textContent = `Xin chào, ${currentUser.name}`;
            dropdownContent.innerHTML = `
                <a asp-controller="TaiKhoan" asp-action="HoSo">Tài khoản của tôi</a>
                <a href="#" id="show-logout-modal" class="dropdown-link logout">Đăng xuất</a>
            `;
            // Gán sự kiện logout cho nút trong dropdown
            const logoutBtn = document.getElementById('show-logout-modal');
            if (logoutBtn) {
                logoutBtn.addEventListener('click', (e) => {
                    e.preventDefault();
                    const modalOverlay = document.getElementById('logout-confirm-modal');
                    if (modalOverlay) {
                        modalOverlay.classList.remove('hidden');
                    }
                });
            }
        } else {
            // CHƯA ĐĂNG NHẬP
            accountText.textContent = 'Tài khoản';
            dropdownContent.innerHTML = `
                <a href="login.html" class="dropdown-link">Đăng nhập</a>
                <a href="register.html" class="dropdown-link">Đăng ký</a>
            `;
        }
    }, 100); // Chờ 100ms để header được include nếu dùng include.js
});

document.addEventListener('DOMContentLoaded', function () {
    const modalOverlay = document.getElementById('logout-confirm-modal');
    const confirmBtn = document.getElementById('logout-confirm-btn');
    const cancelBtn = document.getElementById('logout-cancel-btn');
    const openModalBtn = document.getElementById('show-logout-modal'); // Nút mở modal
    console.log({ modalOverlay, confirmBtn, cancelBtn, openModalBtn });
    if (modalOverlay && confirmBtn && cancelBtn && openModalBtn) {
        // 👉 MỞ modal khi nhấn nút "Đăng xuất"
        openModalBtn.addEventListener('click', () => {
            modalOverlay.classList.remove('hidden');
        });

        // 👉 Đăng xuất
        confirmBtn.addEventListener('click', () => {
            localStorage.removeItem('currentUser');
            modalOverlay.classList.add('hidden');
            window.location.reload();
        });

        // 👉 Hủy modal
        cancelBtn.addEventListener('click', () => {
            modalOverlay.classList.add('hidden');
        });

        // 👉 Đóng khi nhấn ra ngoài vùng modal
        modalOverlay.addEventListener('click', (event) => {
            if (event.target === modalOverlay) {
                modalOverlay.classList.add('hidden');
            }
        });
    } else {
        console.warn("⚠️ Một trong các phần tử không tồn tại!");
    }
});

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



