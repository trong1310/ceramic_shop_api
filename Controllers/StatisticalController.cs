using CeramicShopMasterApi.Databases;
using CeramicShopMasterApi.Models;
using CeramicShopMasterApi.Models.RequestModel;
using CeramicShopMasterApi.Models.ResponseModel;
using CeramicsShopMasterApi.Base.BaseMessages;
using CeramicsShopMasterApi.Enums;
using CeramicsShopMasterApi.Extensions;
using CeramicsShopMasterApi.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CeramicShopMasterApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class StatisticalController : ControllerBase
    {
        private readonly ILogger<StatisticalController> _logger;
        private readonly MasterDBContext _context;

        public StatisticalController(ILogger<StatisticalController> logger, MasterDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Thống kê sản lượng theo loại sản phẩm (danh mục) và từng sản phẩm trong khoảng thời gian
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("volume")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<VolumeStatisticsResponse>), description: "successful operation")]
        public async Task<IActionResult> GetVolumeStatistics([FromBody] BaseRequestMessageKeywordTime request)
        {
            var resp = new BaseResponseMessage<VolumeStatisticsResponse>();
            var acc = User.GetUuid();
            if (acc is null)
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.UNAUTHORIZED);
                return new OkObjectResult(resp);
            }

            try
            {
                // Thống kê theo danh mục (loại sản phẩm)
                var categoryStats = await _context.OrderDetail
                    .Where(d => d.IsEnable == true && d.OrderUu.IsEnable == true && d.OrderUu.State == (byte)edOrderState.Paid
                                && (!request.From.HasValue || d.OrderUu.CreatedAt >= request.From.Value)
                                && (!request.To.HasValue || d.OrderUu.CreatedAt <= request.To.Value))
                    .GroupBy(d => new { d.SlugProductNavigation.Categories.Id, d.SlugProductNavigation.Categories.Name })
                    .Select(g => new CategoryVolumeStat
                    {
                        CategoryId = g.Key.Id,
                        CategoryName = g.Key.Name,
                        TotalQuantitySold = g.Sum(d => d.Quantity)
                    }).OrderByDescending(x => x.TotalQuantitySold).ToListAsync();

                // Thống kê chi tiết theo từng sản phẩm
                var productStats = await _context.OrderDetail
                    .Where(d => d.IsEnable == true && d.OrderUu.IsEnable == true && d.OrderUu.State == (byte)edOrderState.Paid
                                && (!request.From.HasValue || d.OrderUu.CreatedAt >= request.From.Value)
                                && (!request.To.HasValue || d.OrderUu.CreatedAt <= request.To.Value))
                    .GroupBy(d => new { d.SlugProduct, d.SlugProductNavigation.Name, CategoryName = d.SlugProductNavigation.Categories.Name })
                    .Select(g => new ProductVolumeStat
                    {
                        ProductSlug = g.Key.SlugProduct,
                        ProductName = g.Key.Name,
                        CategoryName = g.Key.CategoryName,
                        TotalQuantitySold = g.Sum(d => d.Quantity),
                        Categories = new DataObjectModels
                        {
                            Id = g.FirstOrDefault().SlugProductNavigation.Categories.Id,
                            Name = g.FirstOrDefault().SlugProductNavigation.Categories.Name
                        }
                    })
                    .OrderByDescending(x => x.TotalQuantitySold)
                    .ToListAsync();

                resp.Data = new VolumeStatisticsResponse
                {
                    Categories = categoryStats,
                    Products = productStats
                };

                return new OkObjectResult(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: {ex.Message} : {ex.StackTrace}");
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }
        }

        /// <summary>
        /// Tra cứu doanh số theo tên sản phẩm và khoảng thời gian
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("revenue")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<ProductRevenueListResponse>), description: "successful operation")]
        public async Task<IActionResult> GetProductRevenue([FromBody] BaseRequestMessageKeywordTime request)
        {
            var resp = new BaseResponseMessage<ProductRevenueListResponse>();
            var acc = User.GetUuid();
            if (acc is null)
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.UNAUTHORIZED);
                return new OkObjectResult(resp);
            }

            try
            {
                var query = _context.OrderDetail.AsNoTracking()
                        .Where(d => d.IsEnable == true && d.OrderUu.IsEnable == true && d.OrderUu.State == 2
                            && (!request.From.HasValue || d.OrderUu.CreatedAt >= request.From.Value)
                            && (!request.To.HasValue || d.OrderUu.CreatedAt <= request.To.Value)
                            && d.SlugProductNavigation.IsEnable == true
                            && (string.IsNullOrEmpty(request.Keyword) || EF.Functions.Like(d.SlugProductNavigation.Name, $"{request.Keyword}%")));

                var productRevenues = await query
                    .GroupBy(d => new
                    {
                        d.SlugProduct,
                        d.SlugProductNavigation.Name,
                    })
                    .Select(g => new ProductRevenueResponse
                    {
                        ProductName = g.Key.Name,
                        ProductSlug = g.Key.SlugProduct,
                        TotalQuantitySold = g.Sum(x => x.Quantity),
                        TotalRevenue = g.Sum(x => x.Quantity * x.Amount)
                    })
                    .OrderByDescending(x => x.TotalRevenue)
                    .ToListAsync();

                var grandTotal = productRevenues.Sum(x => x.TotalRevenue);
                resp.Data = new ProductRevenueListResponse
                {
                    Products = productRevenues,
                    GrandTotalRevenue = grandTotal
                };

                return new OkObjectResult(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: {ex.Message} : {ex.StackTrace}");
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }
        }
    }
}
