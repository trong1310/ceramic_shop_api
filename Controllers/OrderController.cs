using CeramicShopMasterApi.Databases;
using CeramicShopMasterApi.Models.RequestModel;
using CeramicShopMasterApi.Models.ResponseModel;
using CeramicsShopMasterApi.Base.BaseExtension;
using CeramicsShopMasterApi.Base.BaseMessages;
using CeramicsShopMasterApi.Enums;
using CeramicsShopMasterApi.Extensions;
using CeramicsShopMasterApi.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CeramicShopMasterApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly MasterDBContext _context;

        public OrderController(ILogger<OrderController> logger, MasterDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        /// <summary>
        /// tạo đơn hàng mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "successful operation")]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var resp = new BaseResponseMessage();
            var acc = User.GetUuid();
            if (acc is null)
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.UNAUTHORIZED);
                return new OkObjectResult(resp);
            }

            if (request.Items == null || request.Items.Count == 0)
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.ORDER_ITEMS_EMPTY);
                return new OkObjectResult(resp);
            }

            await _context.Database.BeginTransactionAsync();
            try
            {
                var totalAmount = 0m;
                var orderDetailsList = new List<OrderDetail>();
                var orderUuid = Guid.NewGuid().ToString();

                for (int i = 0; i < request.Items.Count; i++)
                {
                    var item = request.Items[i];
                    if (string.IsNullOrEmpty(item.SlugProduct))
                    {
                        resp.error = new BaseResponseMessage.Error(ErrorCode.Slug_IS_INVALID);
                        return new OkObjectResult(resp);
                    }
                    if (item.Quantity <= 0)
                    {
                        resp.error = new BaseResponseMessage.Error(ErrorCode.QUANTITY_IS_INVALID);
                        return new OkObjectResult(resp);
                    }

                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Slug == item.SlugProduct && p.IsEnable == true);
                    if (product == null)
                    {
                        resp.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND);
                        return new OkObjectResult(resp);
                    }

                    if (product.Quantity < item.Quantity)
                    {
                        resp.error = new BaseResponseMessage.Error(ErrorCode.PRODUCT_OUT_OF_STOCK);
                        return new OkObjectResult(resp);
                    }

                    product.Quantity -= item.Quantity;
                    _context.Products.Update(product);

                    var detail = new OrderDetail
                    {
                        OrderUuid = orderUuid,
                        SlugProduct = product.Slug,
                        Amount = product.Price,
                        Quantity = item.Quantity,
                        Uuid = Guid.NewGuid().ToString(),
                    };

                    orderDetailsList.Add(detail);
                    totalAmount += product.Price * item.Quantity;
                }

                var order = new Orders
                {
                    Uuid = orderUuid,
                    TotalAmount = totalAmount,
                    State = request.State ?? (byte)edOrderState.Pending,
                    IsEnable = true,
                    CreatedAt = DateTime.Now,
                    PhoneNumber = request.PhoneNumber,
                    FullName = request.FullName,
                    CreatedBy = acc
                };

                await _context.Orders.AddAsync(order);
                await _context.OrderDetail.AddRangeAsync(orderDetailsList);

                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
                return new OkObjectResult(resp);
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                _logger.LogError($"ERROR: {ex.Message} : {ex.StackTrace}");
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }
        }
        /// <summary>
        /// danh sách đơn hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost()]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessagePage<OrderResponseMessage>>), description: "successful operation")]
        public async Task<IActionResult> Get([FromBody] OrderRequestModels request)
        {
            var resp = new BaseResponseMessage<BaseResponseMessagePage<OrderResponseMessage>>();
            try
            {
                var orders = await _context.Orders.AsNoTracking().Include(x => x.CreatedByNavigation)
                    .Where(x => x.IsEnable == true)
                    .Where(x => !request.State.HasValue || x.State == request.State.Value)
                    .Where(p => string.IsNullOrEmpty(request.Keyword)
                    || EF.Functions.Like(p.FullName, $"%{request.Keyword}%")
                    || EF.Functions.Like(p.PhoneNumber, $"%{request.Keyword}%"))
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(p => new OrderResponseMessage
                    {
                        Uuid = p.Uuid,
                        TotalAmount = p.TotalAmount,
                        State = p.State,
                        IsEnable = p.IsEnable,
                        CreatedAt = p.CreatedAt,
                        PhoneNumber = p.PhoneNumber,
                        FullName = p.FullName,
                        CreatedBy = p.CreatedByNavigation.FullName ?? ""
                    }).TakePage(request.Page, request.Limit);

                resp.Data = new()
                {
                    Items = orders,
                    Pagination = new()
                    {
                        TotalCount = orders.TotalCount,
                        TotalPage = orders.TotalPages,
                    }
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
        /// chi tiết đơn hàng
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>

        [HttpGet("detail/{uuid}")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<OrderResponseMessage>), description: "successful operation")]
        public async Task<IActionResult> GetDetail(string uuid)
        {
            var resp = new BaseResponseMessage<OrderResponseMessage>();
            try
            {
                var order = await _context.Orders.AsNoTracking().Include(x => x.OrderDetail)
                    .Where(x => x.Uuid == uuid && x.IsEnable == true)
                    .Select(p => new OrderResponseMessage
                    {
                        Uuid = p.Uuid,
                        TotalAmount = p.TotalAmount,
                        State = p.State,
                        IsEnable = p.IsEnable,
                        CreatedAt = p.CreatedAt,
                        PhoneNumber = p.PhoneNumber,
                        FullName = p.FullName,
                        CreatedBy = p.CreatedBy,
                        OrderDetails = p.OrderDetail.Where(d => d.IsEnable == true).Select(d => new OrderDetailResponseMessage
                        {
                            Uuid = d.Uuid,
                            SlugProduct = d.SlugProduct,
                            ProductName = d.SlugProductNavigation.Name,
                            Amount = d.Amount,
                            Quantity = d.Quantity,
                            CreatedAt = d.CreatedAt
                        }).ToList()
                    }).FirstOrDefaultAsync();

                if (order == null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.ORDER_NOT_FOUND);
                    return new OkObjectResult(resp);
                }

                resp.Data = order;
                return new OkObjectResult(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: {ex.Message} : {ex.StackTrace}");
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }
        }

        [HttpPost("update")]
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage), description: "successful operation")]
        public async Task<IActionResult> Update([FromBody] UpdateOrderRequest request)
        {
            var resp = new BaseResponseMessage();
            var acc = User.GetUuid();
            if (acc is null)
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.UNAUTHORIZED);
                return new OkObjectResult(resp);
            }

            if (string.IsNullOrEmpty(request.Uuid))
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.BAD_REQUEST)
                {
                    Message = "Uuid đơn hàng không được trống."
                };
                return new OkObjectResult(resp);
            }

            if (request.Items != null && request.Items.Count == 0)
            {
                resp.error = new BaseResponseMessage.Error(ErrorCode.ORDER_ITEMS_EMPTY);
                return new OkObjectResult(resp);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.Include(o => o.OrderDetail)
                    .FirstOrDefaultAsync(x => x.Uuid == request.Uuid && x.IsEnable == true);

                if (order == null)
                {
                    resp.error = new BaseResponseMessage.Error(ErrorCode.ORDER_NOT_FOUND);
                    return new OkObjectResult(resp);
                }

                if (request.PhoneNumber != null)
                {
                    order.PhoneNumber = request.PhoneNumber;
                }
                if (request.FullName != null)
                {
                    order.FullName = request.FullName;
                }
                if (request.State.HasValue)
                {
                    order.State = request.State.Value;
                }

                if (request.Items != null)
                {
                    var oldActiveDetails = order.OrderDetail.Where(d => d.IsEnable == true).ToList();
                    foreach (var oldDetail in oldActiveDetails)
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Slug == oldDetail.SlugProduct);
                        if (product != null)
                        {
                            product.Quantity += oldDetail.Quantity;
                            _context.Products.Update(product);
                        }
                        oldDetail.IsEnable = false;
                        _context.OrderDetail.Update(oldDetail);
                    }

                    var totalAmount = 0m;
                    var orderDetailsList = new List<OrderDetail>();

                    var lastDetailId = await _context.OrderDetail.MaxAsync(x => (int?)x.Id) ?? 0;
                    var currentDetailId = lastDetailId + 1;

                    for (int i = 0; i < request.Items.Count; i++)
                    {
                        var item = request.Items[i];
                        if (string.IsNullOrEmpty(item.SlugProduct))
                        {
                            resp.error = new BaseResponseMessage.Error(ErrorCode.Slug_IS_INVALID);
                            return new OkObjectResult(resp);
                        }
                        if (item.Quantity <= 0)
                        {
                            resp.error = new BaseResponseMessage.Error(ErrorCode.QUANTITY_IS_INVALID);
                            return new OkObjectResult(resp);
                        }

                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Slug == item.SlugProduct && p.IsEnable == true);
                        if (product == null)
                        {
                            resp.error = new BaseResponseMessage.Error(ErrorCode.NOT_FOUND)
                            {
                                Message = $"Sản phẩm có slug '{item.SlugProduct}' không tồn tại hoặc đã bị ẩn."
                            };
                            return new OkObjectResult(resp);
                        }

                        if (product.Quantity < item.Quantity)
                        {
                            resp.error = new BaseResponseMessage.Error(ErrorCode.PRODUCT_OUT_OF_STOCK)
                            {
                                Message = $"Sản phẩm '{product.Name}' không đủ số lượng trong kho. Hiện tại chỉ còn {product.Quantity} (sau khi đã khôi phục kho từ đơn cũ)."
                            };
                            return new OkObjectResult(resp);
                        }

                        product.Quantity -= item.Quantity;
                        _context.Products.Update(product);

                        var detail = new OrderDetail
                        {
                            Id = currentDetailId++,
                            OrderUuid = order.Uuid,
                            SlugProduct = product.Slug,
                            Amount = product.Price,
                            Quantity = item.Quantity,
                            Uuid = Guid.NewGuid().ToString(),
                            IsEnable = true,
                            CreatedAt = DateTime.Now
                        };

                        orderDetailsList.Add(detail);
                        totalAmount += product.Price * item.Quantity;
                    }

                    order.TotalAmount = totalAmount;
                    await _context.OrderDetail.AddRangeAsync(orderDetailsList);
                }

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new OkObjectResult(resp);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"ERROR: {ex.Message} : {ex.StackTrace}");
                resp.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(resp);
            }
        }
    }
}
