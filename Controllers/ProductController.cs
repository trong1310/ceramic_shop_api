using CeramicShopMasterApi.Databases;
using CeramicShopMasterApi.Models;
using CeramicShopMasterApi.Models.RequestModel;
using CeramicShopMasterApi.Models.ResponseModel;
using CeramicsShopMasterApi.Base.BaseExtension;
using CeramicsShopMasterApi.Base.BaseMessages;
using CeramicsShopMasterApi.Base.Utils;
using CeramicsShopMasterApi.Extensions;
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
        /// <summary>
        /// danh sách sản phẩm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<ProductResponeMessage>>), description: "successful operation")]
        public async Task<IActionResult> Get([FromBody] ProductRequestModels request)
        {
            var resp = new BaseResponseMessage<BaseResponseMessagePage<ProductResponeMessage>>();
            try
            {
                var products = await _context.Products.AsNoTracking().Include(x => x.Images).Include(x => x.Categories)
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
                        Price = p.Price,
                        Images = p.Images.Where(i => i.IsEnable == true && i.Owner == p.Slug).Select(i => i.Path).ToList(),
                    }).TakePage(request.Page, request.Limit);
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
        /// <summary>
        /// thêm sản phẩm mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "successful operation")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var resp = new BaseResponseMessage();
            var acc = User.GetUuid();
            if (acc is null)
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.UNAUTHORIZED);
                return new OkObjectResult(resp);
            }
            try
            {
                if (string.IsNullOrEmpty(request.Name) || request.Name.Length > 255)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.NAME_IS_INVALID);
                    return new OkObjectResult(resp);
                }
                if (!request.CategoriesId.HasValue || !await _context.Categories.AsNoTracking().AnyAsync(x => x.IsEnable == true && x.Id == request.CategoriesId))
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.CATEGORIES_IS_INVALID);
                    return new OkObjectResult(resp);
                }
                if (!request.Quantity.HasValue || request.Quantity.Value <= 0)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.QUANTITY_IS_INVALID);
                    return new OkObjectResult(resp);
                }
                if (!request.Price.HasValue || request.Price.Value <= 0)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.PRICE_IS_INVALID);
                    return new OkObjectResult(resp);
                }
                var slug = Util.GenerateSlug(request.Name);
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Slug == slug);
                if (existingProduct != null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.Product_Already_Exists);
                    return new OkObjectResult(resp);
                }
                var product = new Products
                {
                    Name = request.Name,
                    Slug = slug,
                    CategoriesId = request.CategoriesId.Value,
                    Quantity = request.Quantity.Value,
                    Price = request.Price.Value,

                };
                await _context.Products.AddAsync(product);
                if (request.Images != null && request.Images.Count > 0)
                {
                    var images = await _context.Images.Where(i => request.Images.Contains(i.Path)).ToListAsync();
                    if (images.Count > 0)
                    {
                        foreach (var img in images)
                        {
                            img.Owner = slug;
                            img.IsEnable = true;
                        }
                    }
                    _context.Images.UpdateRange(images);
                }
                await _context.SaveChangesAsync();
                return new OkObjectResult(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR :{ex.Message} : {ex.StackTrace}");
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }
        }
        /// <summary>
        /// chỉnh sửa sản phẩm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("update")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "successful operation")]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest request)
        {
            var resp = new BaseResponseMessage();
            var acc = User.GetUuid();
            if (acc is null)
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.UNAUTHORIZED);
                return new OkObjectResult(resp);
            }
            try
            {
                if (string.IsNullOrEmpty(request.Slug))
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.Slug_IS_INVALID);
                    return new OkObjectResult(resp);
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Slug == request.Slug && p.IsEnable == true);
                if (product == null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(resp);
                }

                if (string.IsNullOrEmpty(request.Name) || request.Name.Length > 255)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.NAME_IS_INVALID);
                    return new OkObjectResult(resp);
                }

                if (!request.CategoriesId.HasValue || !await _context.Categories.AsNoTracking().AnyAsync(x => x.IsEnable == true && x.Id == request.CategoriesId))
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.CATEGORIES_IS_INVALID);
                    return new OkObjectResult(resp);
                }
                if (!request.Quantity.HasValue || request.Quantity.Value <= 0)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.QUANTITY_IS_INVALID);
                    return new OkObjectResult(resp);
                }
                if (!request.Price.HasValue || request.Price.Value <= 0)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.PRICE_IS_INVALID);
                    return new OkObjectResult(resp);
                }

                product.Name = request.Name;
                product.Slug = request.Slug;
                product.CategoriesId = request.CategoriesId.Value;
                product.Quantity = request.Quantity.Value;
                product.Price = request.Price.Value;
                var oldImages = await _context.Images.Where(i => i.Owner == product.Slug).ToListAsync();
                foreach (var img in oldImages)
                {
                    img.IsEnable = false;
                }
                if (request.Images != null && request.Images.Count > 0)
                {
                    var images = await _context.Images.Where(i => request.Images.Contains(i.Path)).ToListAsync();
                    if (images.Count > 0)
                    {
                        foreach (var img in images)
                        {
                            img.Owner = product.Slug;
                            img.IsEnable = true;
                        }
                    }
                    _context.Images.UpdateRange(images);
                }
                await _context.SaveChangesAsync();
                return new OkObjectResult(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR :{ex.Message} : {ex.StackTrace}");
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }
        }
        /// <summary>
        /// xóa sản phẩm
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>

        [HttpPost("delete/{slug}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "successful operation")]
        public async Task<IActionResult> Delete(string slug)
        {
            var resp = new BaseResponseMessage();
            var acc = User.GetUuid();
            if (acc is null)
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.UNAUTHORIZED);
                return new OkObjectResult(resp);
            }
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Slug == slug && p.IsEnable == true);
                if (product == null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                    return new OkObjectResult(resp);
                }

                product.IsEnable = false;
                var images = await _context.Images.Where(i => i.Owner == slug && i.IsEnable == true).ToListAsync();
                foreach (var img in images)
                {
                    img.IsEnable = false;
                }

                await _context.SaveChangesAsync();
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
