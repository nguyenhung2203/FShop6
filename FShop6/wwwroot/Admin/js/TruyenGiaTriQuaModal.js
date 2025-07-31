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

document.getElementById('editProductModal').addEventListener('show.bs.modal', function (event) {
    const button = event.relatedTarget;
    const id = button.getAttribute('data-maSanPham');
    const ten = button.getAttribute('data-tenSanPham');
    const mota = button.getAttribute('data-moTa');
    const danhmuc = button.getAttribute('data-danhMuc');
    const anh = button.getAttribute('data-anhDaiDien');

    this.querySelector('#product_id_edit').value = id;
    this.querySelector('#product_name_edit').value = ten;
    this.querySelector('#product_desc_edit').value = mota;
    this.querySelector('#category_select_edit').value = danhmuc;

    this.querySelector('#current_image_preview').src = '/KhachHang/images/' + anh;
});

document.getElementById('deleteProductModal').addEventListener('show.bs.modal', function (event) {
    const button = event.relatedTarget;
    const id = button.getAttribute('data-maSanPham');

    this.querySelector('#product_id_delete').value = id;
});

document.getElementById('addProductDetails').addEventListener('show.bs.modal', function (event) {
    const button = event.relatedTarget;
    const id = button.getAttribute('data-maSanPham');
    this.querySelector('#productDetail_id_delete').value = id; // Clear the hidden product ID field
});