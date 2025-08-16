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
    document.getElementById('featured_select_edit').value = (noibat === 'True') ? 'true' : 'false';
    document.getElementById('status_select_edit').value = (trangthai === 'True') ? 'true' : 'false';
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
    const giamGia = button.dataset.giamgia;
    document.getElementById('ma_bien_the_sp_edit').value = maBienThe;
    document.getElementById('sku_edit_detail').value = maSku;
    document.getElementById('type_edit_detail').value = loaiSanPham;
    document.getElementById('price_sell_edit_detail').value = giaBan;
    document.getElementById('price_import_edit_detail').value = giaNhap;
    document.getElementById('discount_edit_detail').value = giamGia;
    document.getElementById('quantity_edit_detail').value = soLuong;
    document.getElementById('status_input_edit').value = tinhTrang;
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




document.addEventListener('DOMContentLoaded', function () {
    // Xử lý form thêm sản phẩm mới
    var formadd = document.querySelector('#addProductModal form');

    var tenSanPhamAdd = document.getElementById('product_name_add');
    var moTaAdd = document.getElementById('product_desc_add');

    // Hai thẻ div báo lỗi
    var loiTenAdd = tenSanPhamAdd.nextElementSibling; // div.thong_bao_loi ngay sau input
    var loiMoTaAdd = moTaAdd.nextElementSibling; // div.thong_bao_loi ngay sau textarea
    formadd.addEventListener('submit', function (suKien) {
        var hopLe = true;
        // Xóa thông báo lỗi cũ
        loiTenAdd.textContent = '';
        loiMoTaAdd.textContent = '';
        var giaTriTenAdd = tenSanPhamAdd.value.trim();
        var giaTriMoTaAdd = moTaAdd.value.trim();
        if (giaTriTenAdd.length > 100) {
            loiTenAdd.textContent = 'Tên sản phẩm không được vượt quá 100 ký tự.';
            hopLe = false;
        }
        if (giaTriMoTaAdd.length > 255) {
            loiMoTaAdd.textContent = 'Mô tả sản phẩm không được vượt quá 255 ký tự.';
            hopLe = false;
        }
        if (!hopLe) {
            suKien.preventDefault(); // ✅ Ngăn form submit
        }
    });


    // Xử lý form sửa sản phẩm
    var formedit = document.querySelector('#editProductModal form');

    var tenSanPhamEdit = document.getElementById('product_name_edit');
    var moTaEdit = document.getElementById('product_desc_edit');

    // Hai thẻ div báo lỗi
    var loiTenEdit = tenSanPhamEdit.nextElementSibling; // div.thong_bao_loi ngay sau input
    var loiMoTaEdit = moTaEdit.nextElementSibling; // div.thong_bao_loi ngay sau textarea

    formedit.addEventListener('submit', function (suKien) {
        var hopLe = true;

        // Xóa thông báo lỗi cũ
        loiTenEdit.textContent = '';
        loiMoTaEdit.textContent = '';

        var giaTriTenEdit = tenSanPhamEdit.value.trim();
        var giaTriMoTaEdit = moTaEdit.value.trim();

        if (giaTriTenEdit.length > 100) {
            loiTenEdit.textContent = 'Tên sản phẩm không được vượt quá 100 ký tự.';
            hopLe = false;
        }

        if (giaTriMoTaEdit.length > 255) {
            loiMoTaEdit.textContent = 'Mô tả sản phẩm không được vượt quá 255 ký tự.';
            hopLe = false;
        }

        if (!hopLe) {
            suKien.preventDefault(); // ✅ Ngăn form submit
        }
    });

    // Xử lý form thêm biến thể sản phẩm
    var formAddVariant = document.querySelector('#addProductDetails form');
    var skuAdd = document.getElementById('sku_add_detail');
    var loaiSanPhamAdd = document.getElementById('type_add_detail');
    var giaNhapAdd = document.getElementById('price_import_add_detail');
    var giaBanAdd = document.getElementById('price_sell_add_detail');

    // thẻ div báo lỗi
    var loiSkuAdd = skuAdd.nextElementSibling; // div.thong_bao_loi ngay sau input
    var loiLoaiSanPhamAdd = loaiSanPhamAdd.nextElementSibling; // div.thong_bao_loi ngay sau input
    var loiGiaNhapAdd = giaNhapAdd.nextElementSibling; // div.thong_bao_loi ngay sau input
    var loiGiaBanAdd = giaBanAdd.nextElementSibling; // div.thong_bao_loi ngay sau input

    formAddVariant.addEventListener('submit', function (suKien) {
        var hopLe = true;
        // Xóa thông báo lỗi cũ
        loiSkuAdd.textContent = '';
        loiLoaiSanPhamAdd.textContent = '';
        loiGiaNhapAdd.textContent = '';
        loiGiaBanAdd.textContent = '';
        var giaTriSkuAdd = skuAdd.value.trim();
        var giaTriLoaiSanPhamAdd = loaiSanPhamAdd.value.trim();
        var giaTriGiaNhapAdd = parseInt(giaNhapAdd.value.trim().replace(/,/g, ''), 10);
        var giaTriGiaBanAdd = parseInt(giaBanAdd.value.trim().replace(/,/g, ''), 10);
        if (giaTriSkuAdd.length > 30) {
            loiSkuAdd.textContent = 'SKU không được vượt quá 30 ký tự.';
            hopLe = false;
        }
        if (giaTriLoaiSanPhamAdd.length > 30) {
            loiLoaiSanPhamAdd.textContent = 'Loại sản phẩm không được vượt quá 30 ký tự.';
            hopLe = false;
        }
        if (giaTriGiaNhapAdd > 1000000000) {
            loiGiaNhapAdd.textContent = 'Giá nhập không vượt quá 1,000,000,000.';
            hopLe = false;
        }
        if (giaTriGiaBanAdd > 1000000000) {
            loiGiaBanAdd.textContent = 'Giá bán không vượt quá 1,000,000,000.';
            hopLe = false;
        }
        if (!isNaN(giaTriGiaNhapAdd) && !isNaN(giaTriGiaBanAdd) && giaTriGiaBanAdd < giaTriGiaNhapAdd) {
            loiGiaBanAdd.textContent = 'Giá bán không được nhỏ hơn giá nhập.';
            hopLe = false;
        }
        if (!hopLe) {
            suKien.preventDefault(); // ✅ Ngăn form submit
        }
    });
    // Xử lý form sửa biến thể sản phẩm
    var formEditVariant = document.querySelector('#editProductDetails form');
    var loaiSanPhamEdit = document.getElementById('type_edit_detail');
    var giaNhapEdit = document.getElementById('price_import_edit_detail');
    var giaBanEdit = document.getElementById('price_sell_edit_detail');

    // Thẻ div báo lỗi
    var loiLoaiSanPhamEdit = loaiSanPhamEdit.nextElementSibling; // div.thong_bao_loi ngay sau input
    var loiGiaNhapEdit = giaNhapEdit.nextElementSibling; // div.thong_bao_loi ngay sau input
    var loiGiaBanEdit = giaBanEdit.nextElementSibling; // div.thong_bao_loi ngay sau input

    formEditVariant.addEventListener('submit', function (suKien) {
        var hopLe = true;
        // Xóa thông báo lỗi cũ
        loiLoaiSanPhamEdit.textContent = '';
        loiGiaNhapEdit.textContent = '';
        loiGiaBanEdit.textContent = '';
        var giaTriLoaiSanPhamEdit = loaiSanPhamEdit.value.trim();
        var giaTriGiaNhapEdit = parseInt(giaNhapEdit.value.trim().replace(/,/g, ''), 10);
        var giaTriGiaBanEdit = parseInt(giaBanEdit.value.trim().replace(/,/g, ''), 10);
        if (giaTriLoaiSanPhamEdit.length > 30) {
            loiLoaiSanPhamEdit.textContent = 'Loại sản phẩm không được vượt quá 30 ký tự.';
            hopLe = false;
        }
        if (giaTriGiaNhapEdit > 1000000000) {
            loiGiaNhapEdit.textContent = 'Giá nhập không vượt quá 1,000,000,000.';
            hopLe = false;
        }
        if (giaTriGiaBanEdit > 1000000000) {
            loiGiaBanEdit.textContent = 'Giá bán không vượt quá 1,000,000,000.';
            hopLe = false;
        }
        if (!isNaN(giaTriGiaNhapEdit) && !isNaN(giaTriGiaBanEdit) && giaTriGiaBanEdit < giaTriGiaNhapEdit) {
            loiGiaBanEdit.textContent = 'Giá bán không được nhỏ hơn giá nhập.';
            hopLe = false;
        }
        if (!hopLe) {
            suKien.preventDefault(); // ✅ Ngăn form submit
        }
    });
});

