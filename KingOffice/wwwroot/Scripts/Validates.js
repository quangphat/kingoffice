const customer_name_must_not_be_empty = "Tên khách hàng không được bỏ trống";
const select_partner = "Vui lòng chọn đối tác";
const select_product = "Vui lòng chọn sản phẩm vay";
const success_message = "Thành công";
const invalid_phone = "Số điện thoại di động phải đúng 10 số!";
const invalid_employee_phone = "Số điện thoại nhân viên phải đúng 10 số!";
const invalid_id_number = "Số CMND chỉ 9 hoặc 12 số!";
function isNullOrNoItem(arr) {
    if (arr === null || arr === undefined || arr.length === 0)
        return true;
    return false;
}
function isNullOrUndefined(value) {
    if (value === null || value === undefined)
        return true;
    return false;
}
function isNullOrWhiteSpace(text) {
    if (text === null || text === undefined || text === '' || text.toString().trim() === '')
        return true;
    return false;
}
