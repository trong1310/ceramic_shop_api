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
