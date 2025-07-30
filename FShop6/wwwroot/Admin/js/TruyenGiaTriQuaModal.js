document.addEventListener('DOMContentLoaded', function () {
    var viewModel = document.getElementById('viewProductDetails');
    viewModel.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget; // Nút đã click
        var productId = button.getAttribute('data-maSanPham');

        var tatCaRows = viewModel.querySelectorAll('#dsSanPhamBienThe tr');
        tatCaRows.forEach(row => {
            var rowMaSP = row.getAttribute('data-masanpham');
            row.style.display = (rowMaSP === productId) ? '' : 'none';
        });
    });
});