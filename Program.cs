using VTSTravelMasterApi.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Thêm cấu hình Global
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSetting = appSettingsSection.Get<AppSettings>();
// 
var _tokenSettingSection = builder.Configuration.GetSection("Token");
builder.Services.Configure<JwtTokenSettings>(_tokenSettingSection);
var _tokenSetting = _tokenSettingSection.Get<JwtTokenSettings>();
//
GlobalSetting.Include(appSetting, _tokenSetting);

//// Cấu hình Mysql
//builder.Services.ConfigureMySql(appSetting.ConnectionStrings);

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("CorsPolicy");
app.UseCors(x =>
{
	x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});
app.Run();
