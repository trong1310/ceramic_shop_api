using CeramicShopMasterApi.Databases;
using CeramicShopMasterApi.Models.ResponseModel;
using CeramicsShopMasterApi.Base.BaseMessages;
using CeramicsShopMasterApi.Enums;
using CeramicsShopMasterApi.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace CeramicShopMasterApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;
        private readonly MasterDBContext _context;
        public CommonController(ILogger<CommonController> logger, MasterDBContext context)
        {
            _logger = logger;
            _context = context;
        }
           [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessageItem<CategoriesResponseMessage>>), description: "successful operation")]
        [HttpGet("categories")]
        public IActionResult Categories([FromBody] BaseRequestMessageKeyword request)
        {
            var resp = new BaseResponseMessage<BaseResponseMessageItem<CategoriesResponseMessage>>();
            try
            {
                var categories = _context.Categories.AsNoTracking().Where(x => x.IsEnable == true)
                    .Where(c => string.IsNullOrEmpty(request.Keyword) || c.Name.Contains(request.Keyword))
                    .Select(c => new CategoriesResponseMessage
                    {
                        Id = c.Id,  
                        Name = c.Name,

                    })
                    .ToList();
                resp.Data = new()
                {
                    Items = categories
                };
                return new OkObjectResult(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR :{ex.Message} : {ex.StackTrace}");
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }
        }
    }
}
