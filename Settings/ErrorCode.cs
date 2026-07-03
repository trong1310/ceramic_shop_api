using System.ComponentModel;

namespace CeramicsShopMasterApi.Settings
{
    public enum ErrorCode
    {
        [Description("Thất bại")]
        FAILED = -1,
        [Description("Tài khoản không tồn tại")]
        ACCOUNT_IS_NOT_CORRECT,
        [Description("Mật khẩu không chính xác")]
        PASSWORD_IS_INCORRECT,
        [Description("Tài khoản đã bị khóa")]
        ACCOUNT_HAS_LOCKED,
        [Description("Tên không hợp lệ")]
        NAME_IS_INVALID,
        [Description("Danh mục không hợp lệ")]
        CATEGORIES_IS_INVALID,
        [Description("Số lượng không hợp lệ")]
        QUANTITY_IS_INVALID,
        [Description("Giá không hợp lệ")]
        PRICE_IS_INVALID,
        [Description("Không tìm thấy foler")]
        FOLDER_IMAGE_NOT_FOUND,
        [Description("Sản phẩm đã tồn tại")]
        Product_Already_Exists,

        [Description("Slug không hợp lệ")]
        Slug_IS_INVALID,
        [Description("Danh sách sản phẩm trống")]
        ORDER_ITEMS_EMPTY,
        [Description("Sản phẩm không đủ số lượng trong kho")]
        PRODUCT_OUT_OF_STOCK,
        [Description("Uuid không được trống")]
        UUID_IS_EMPTY,
        [Description("Không tìm thấy đơn hàng")]
        ORDER_NOT_FOUND,
        //--------------------------------
        [Description("Thành công")]
        SUCCESS = 200,
        [Description("Bad Request")]
        BAD_REQUEST = 400,
        [Description("Unauthorized")]
        UNAUTHORIZED = 401,
        [Description("Forbidden")]
        FORBIDDEN = 403,
        [Description("Object Not Found")]
        NOT_FOUND = 404,
        [Description("Lỗi hệ thống")]
        SYSTEM_ERROR = 999,
    }
}
