using CeramicShopMasterApi.Databases;
using CeramicShopMasterApi.Models.ResponseModel;
using CeramicsShopMasterApi.Base.BaseMessages;
using CeramicsShopMasterApi.SecurityManagers;
using CeramicsShopMasterApi.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CeramicShopMasterApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly ILogger<AccountController> _logger;
        private readonly MasterDBContext _context;
        public AccountController(ILogger<AccountController> logger, MasterDBContext context, IJwtAuthenticationManager jwtAuthentication)
        {
            _logger = logger;
            _context = context;
            _jwtAuthenticationManager = jwtAuthentication;
        }
        /// <summary>
		/// Đăng nhập
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("login")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<AccLoginResp>), description: "successful operation")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var resp = new BaseResponseMessage<AccLoginResp>();
            try
            {
                var user = new Accounts();
                user = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == request.UserName);
                if (user == null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.ACCOUNT_IS_NOT_CORRECT);
                    return new OkObjectResult(resp);
                }
                if (user.Password != request.Password)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.PASSWORD_IS_INCORRECT);
                    return new OkObjectResult(resp);
                }
                if (user.Active == false)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.ACCOUNT_HAS_LOCKED);
                    return new OkObjectResult(resp);
                }
                var _authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("uuid", user.Uuid),
                };

                var _token = _jwtAuthenticationManager.Authenticate(_authClaims, user.Username, user.Uuid);
                if (_token is null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.ACCOUNT_IS_NOT_CORRECT);
                    return new OkObjectResult(resp);
                }

                resp.Data = new AccLoginResp()
                {
                    AccessToken = _token.AccessToken,
                    RefreshToken = _token.RefreshToken,
                    TimeExpired = _token.TimeExpired,
                    TimeStart = _token.TimeStart,
                    UserName = user.Username,
                    FullName = user.FullName ?? "",
                    Email = user.Email ?? "",
                    PhoneNumber = user.Phone ?? "",
                    Uuid = user.Uuid,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }

            return new OkObjectResult(resp);
        }


    }
}
