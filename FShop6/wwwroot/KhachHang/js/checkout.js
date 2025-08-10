document.addEventListener('DOMContentLoaded', () => {
    const checkoutForm = document.getElementById('checkout-form');
    const orderTable = document.getElementById('order-summary-table');
    const bankInfoModal = document.getElementById('bank-info-modal');
    const button = document.querySelector('button[form="checkout-form"]');
    const url = button.dataset.successUrl;


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
