using System.ComponentModel;

namespace VTSTravelMasterApi.Settings
{
	public enum ErrorCode
	{
		[Description("Thất bại")]
		FAILED = -1,



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
