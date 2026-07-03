using CeramicShopMasterApi.Databases;
using CeramicShopMasterApi.Models.ResponseModel;
using CeramicsShopMasterApi.Base.BaseMessages;
using CeramicsShopMasterApi.Base.Utils;
using CeramicsShopMasterApi.Enums;
using CeramicsShopMasterApi.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using static System.Net.Mime.MediaTypeNames;

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
        /// <summary>
        /// danh sách danh mục sản phẩm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, type: typeof(BaseResponseMessage<BaseResponseMessageItem<CategoriesResponseMessage>>), description: "successful operation")]
        [HttpGet("categories")]
        public IActionResult Categories([FromQuery] BaseRequestMessageKeyword request)
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
        /// <summary>
        /// upload hình ảnh
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [Authorize]
        [RequestSizeLimit(1024 * 1024 * 5)]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] List<IFormFile> files)
        {
            var respone = new BaseResponseMessage<BaseResponseMessageItem<string>>();
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            try
            {
                var newDatas = new List<Images>();
                var filesName = files.Select(x => x.FileName).ToList();

              //  Lấy năm và tháng hiện tại
                var year = DateTime.UtcNow.Year.ToString();
                var month = DateTime.UtcNow.Month.ToString("D2");

            //    Đường dẫn thư mục theo cấu trúc: Resources /{ year}/{ month}/ images
                var folderPath = Path.Combine(rootPath, year, month);

              //  Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var name = Guid.NewGuid().ToString();
                        var filePath = Path.Combine(folderPath, name + Path.GetExtension(file.FileName));
                        var filePathEx = Path.Combine("Resources", year, month, name + Path.GetExtension(file.FileName)).Replace("\\", "/");

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        newDatas.Add(new()
                        {
                            Uuid = Guid.NewGuid().ToString(),
                            Path = filePathEx,
                            
                        });
                    }
                }

                await _context.Images.AddRangeAsync(newDatas);
                await _context.SaveChangesAsync();

                respone.Data = new()
                {
                    Items = newDatas.Select(x => x.Path).ToList(),
                };
                return new OkObjectResult(respone);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erorr: {ex.Message} : {ex.StackTrace}");
                respone.error = new BaseResponseMessage.Error(ErrorCode.SYSTEM_ERROR);
                return new OkObjectResult(respone);
            }
        }

    }
}
