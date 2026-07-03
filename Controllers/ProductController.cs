using CeramicShopMasterApi.Databases;
using CeramicShopMasterApi.Models;
using CeramicShopMasterApi.Models.RequestModel;
using CeramicShopMasterApi.Models.ResponseModel;
using CeramicsShopMasterApi.Base.BaseExtension;
using CeramicsShopMasterApi.Base.BaseMessages;
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
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly MasterDBContext _context;
        public ProductController(ILogger<ProductController> logger, MasterDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost()]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<ProductResponeMessage>>), description: "successful operation")]
        public async Task<IActionResult> Get([FromBody] ProductRequestModels request)
        {
            var resp = new BaseResponseMessage<BaseResponseMessagePage<ProductResponeMessage>>();
            try
            {
                var products = await _context.Products.AsNoTracking().Include(x=>x.Categories)
                    .Where(x => x.IsEnable == true)
                    .Where(p => string.IsNullOrEmpty(request.Keyword) || p.Name.Contains(request.Keyword))
                    .Select(p => new ProductResponeMessage
                    {
                        Slug = p.Slug,
                        Name = p.Name,
                        Categories = new DataObjectModels
                        {
                            Id = p.Categories.Id,
                            Name = p.Categories.Name,
                        },
                        Quantity = p.Quantity,
                        Price = p.Price
                    }).TakePage(request.Page,request.Limit);
                resp.Data = new()
                {
                    Items = products,
                    Pagination = new()
                    {
                        TotalCount = products.TotalCount,
                        TotalPage = products.TotalPages,
                    }
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
