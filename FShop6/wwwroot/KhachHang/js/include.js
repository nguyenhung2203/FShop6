document.addEventListener("DOMContentLoaded", function () {
    // --- TẢI HEADER ---
    const headerPlaceholder = document.getElementById("header-placeholder");
    if (headerPlaceholder) {
        fetch("header.html")
            .then((response) => response.text())
            .then((data) => {
                headerPlaceholder.innerHTML = data;
                // Sau khi header được tải, thực thi logic cho active link
                setActiveNavLink();
            })
            .catch((error) =>
                console.error("Lỗi khi tải header.html:", error)
            );
    }

    // --- TẢI FOOTER ---
    const footerPlaceholder = document.getElementById("footer-placeholder");
    if (footerPlaceholder) {
        fetch("footer.html")
            .then((response) => response.text())
            .then((data) => {
                footerPlaceholder.innerHTML = data;

                // Tải các file JS sau khi footer đã được chèn vào DOM
                // Điều này đảm bảo main.js có thể tìm thấy các phần tử trong header/footer
                const scripts = footerPlaceholder.querySelectorAll("script");
                scripts.forEach(oldScript => {
                    const newScript = document.createElement("script");
                    // Sao chép các thuộc tính (ví dụ: src)
                    Array.from(oldScript.attributes).forEach(attr => {
                        newScript.setAttribute(attr.name, attr.value);
                    });
                    oldScript.parentNode.replaceChild(newScript, oldScript);
                });

            })
            .catch((error) =>
                console.error("Lỗi khi tải footer.html:", error)
            );
    }
});

// Hàm để đặt class 'active-link' cho menu item tương ứng với trang hiện tại
function setActiveNavLink() {
    const currentPage = window.location.pathname.split("/").pop(); // Lấy tên file, ví dụ: "shop.html"
    const navLinks = document.querySelectorAll(".nav__link");

    navLinks.forEach((link) => {
        const linkPage = link.getAttribute("href");
        if (linkPage === currentPage) {
            link.classList.add("active-link");
        }
    });
}