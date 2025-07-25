document.addEventListener('DOMContentLoaded', () => {
    // ---- LẤY DỮ LIỆU VÀ CÁC PHẦN TỬ DOM ----
    // Ưu tiên giỏ hàng thật, nếu không có thì dùng dữ liệu giả để demo
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    if (cart.length === 0) {
        cart = [
            { id: 1, name: 'Áo thun Polo FPT', price: 250000, quantity: 2, img: 'https://via.placeholder.com/80x80' },
            { id: 4, name: 'Balo Laptop FPT', price: 450000, quantity: 1, img: 'https://via.placeholder.com/80x80' }
        ];
    }

    const checkoutForm = document.getElementById('checkout-form');
    const orderTable = document.getElementById('order-summary-table');
    const bankInfoModal = document.getElementById('bank-info-modal');
    const button = document.querySelector('button[form="checkout-form"]');
    const url = button.dataset.successUrl;

    console.log('Giỏ hàng:', url);

    // --- HIỂN THỊ TÓM TẮT ĐƠN HÀNG ---
    let subtotal = 0;
    let tableHTML = `
        <thead>
            <tr>
                <th colspan="2">Sản phẩm</th>
                <th>Tạm tính</th>
            </tr>
        </thead>
        <tbody>
    `;

    cart.forEach(item => {
        const itemTotal = item.price * item.quantity;
        subtotal += itemTotal;
        tableHTML += `
            <tr>
                <td><img src="${item.img}" alt="${item.name}" class="order__img" /></td>
                <td>
                    <h3 class="table__title">${item.name}</h3>
                    <p class="table__quantity">x ${item.quantity}</p>
                </td>
                <td><span class="table__price">${itemTotal.toLocaleString()}đ</span></td>
            </tr>
        `;
    });

    tableHTML += `
            <tr>
                <td><span class="order__subtitle">Tạm tính</span></td>
                <td colspan="2"><span class="table__price">${subtotal.toLocaleString()}đ</span></td>
            </tr>
            <tr>
                <td><span class="order__subtitle">Phí vận chuyển</span></td>
                <td colspan="2"><span>Miễn phí</span></td>
            </tr>
            <tr>
                <td><span class="order__subtitle">Tổng cộng</span></td>
                <td colspan="2"><span class="order__grand-total">${subtotal.toLocaleString()}đ</span></td>
            </tr>
        </tbody>
    `;

    if (orderTable) {
        orderTable.innerHTML = tableHTML;
    }

    // ---- LOGIC CHO MODAL VÀ THANH TOÁN ----

    // Hàm để mở modal
    function openBankInfoModal() {
        if (bankInfoModal) bankInfoModal.classList.remove('hidden');
    }

    // Hàm để đóng modal
    function closeBankInfoModal() {
        if (bankInfoModal) bankInfoModal.classList.add('hidden');
        localStorage.removeItem('cart');
        window.location.href = url;
    }

    // Gắn sự kiện click để đóng modal
    if (bankInfoModal) {
        const closeButtons = bankInfoModal.querySelectorAll('#confirm-transfer-btn, .modal-bank-info__overlay');
        closeButtons.forEach(btn => btn.addEventListener('click', closeBankInfoModal));
    }
    var bank = document.getElementById('bank');
    var modal = document.getElementById('bank-info-modal');
    // XỬ LÝ KHI NHẤN NÚT "HOÀN TẤT ĐƠN HÀNG"
    if (checkoutForm) {
        checkoutForm.addEventListener('submit', (e) => {
            e.preventDefault();
            const selectedPayment = document.querySelector('input[name="payment"]:checked').value;

            if (checkoutForm.checkValidity()) {
                if (selectedPayment === 'bank') {
                    modal.style.display = 'flex';
                    openBankInfoModal();
                } else {
                    localStorage.removeItem('cart');
                    window.location.href = url;
                }
            } else {
                checkoutForm.reportValidity();
            }
        });
    }
});
