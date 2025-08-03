// Truyền mã sản phẩm từ nút xem chi tiết tới modal xem chi tiết
window.showProductVariants = function (button) {
    const maSanPham = button.dataset.masanpham;

    // Gán mã sản phẩm vào input hidden nếu cần dùng tiếp
    document.getElementById('view_bien_the_sp').value = maSanPham;

    // Hiện/ẩn các dòng biến thể
    const rows = document.querySelectorAll('#dsSanPhamBienThe tr');
    rows.forEach(row => {
        const rowMaSP = row.dataset.masanpham;
        row.style.display = (rowMaSP === maSanPham) ? '' : 'none';
    });

    // Mở modal bằng JS nếu không dùng data-bs-toggle
    const modal = new bootstrap.Modal(document.getElementById('viewProductDetails'));
    modal.show();
}

window.passProductIdToAddVariant = function (button) {
    const maSanPham = document.getElementById('view_bien_the_sp').value;

    if (!maSanPham) {
        alert("Không tìm thấy mã sản phẩm. Vui lòng thử lại.");
        return;
    }

    // Gán vào thuộc tính data cho nút nếu cần
    button.dataset.masanpham = maSanPham;

    // Gán vào form thêm biến thể (modal khác)
    const input = document.getElementById('maSanPham_add_variant');
    if (input) {
        input.value = maSanPham;
    }
};


window.openEditModal = function (button) {
    const maSanPham = button.dataset.masanpham;
    const tenSanPham = button.dataset.tensanpham;
    const moTa = button.dataset.mota;
    const danhMucId = button.dataset.danhmucid;
    const hinhAnh = button.dataset.anhdaidien;
    const noibat = button.dataset.noibat;
    const trangthai = button.dataset.trangthai;

    document.getElementById('product_name_edit').value = tenSanPham;
    document.getElementById('product_desc_edit').value = moTa;
    document.getElementById('category_select_edit').value = danhMucId;
    document.getElementById('ma_san_pham_hidden').value = maSanPham;
    document.getElementById('featured_select_edit').value = noibat ? 'true' : 'false';
    document.getElementById('status_select_edit').value = trangthai ? 'true' : 'false';
    const imgPreview = document.getElementById('old_image_preview');
    if (imgPreview && hinhAnh) {
        imgPreview.src = '/KhachHang/images/' + hinhAnh;
    } else {
        imgPreview.src = '';
    }
}

//const modal = document.getElementById('editProductModal');
//modal.addEventListener('hidden.bs.modal', function () {
//    location.reload();
//});

window.openDeleteModal = function (button) {
    const maSanPham = button.dataset.masanpham;
    document.getElementById('product_id_delete').value = maSanPham;
}

window.openEditDetailsModal = function (button) {
    const maBienThe = button.dataset.mabienthe;
    const maSku = button.dataset.masku;
    const loaiSanPham = button.dataset.loaibienthe;
    const giaNhap = button.dataset.gianhap;
    const giaBan = button.dataset.giaban;
    const soLuong = button.dataset.soluongconlai;
    const tinhTrang = button.dataset.tinhtrang;
    const maAnhBTCu = button.dataset.maanhbtcu;
    const urlAnhBTCu = button.dataset.urlanhbtcu;
    document.getElementById('ma_bien_the_sp_edit').value = maBienThe;
    document.getElementById('sku_edit_detail').value = maSku;
    document.getElementById('type_edit_detail').value = loaiSanPham;
    document.getElementById('price_sell_edit_detail').value = giaBan;
    document.getElementById('price_import_edit_detail').value = giaNhap;
    document.getElementById('quantity_edit_detail').value = soLuong;
    document.getElementById('status_input_edit').value = tinhTrang;
    document.getElementById('ma_anh_bien_the_cu').value = maAnhBTCu;
    const imgPreview = document.getElementById('anhBTCu');
    if (imgPreview && urlAnhBTCu) {
        imgPreview.src = '/KhachHang/images/' + urlAnhBTCu;
    } else {
        imgPreview.src = '';
    }
}

window.openDeleteDetailsModal = function (button) {
    const maBienThe = button.dataset.mabienthe;

    document.getElementById('ma_bien_the_sp_delete').value = maBienThe;
}
