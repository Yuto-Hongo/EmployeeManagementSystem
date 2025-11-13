using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Serilog;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// CORSポリシー登録
builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend",
    policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// サービス登録
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// AuthServiceのDI設定
builder.Services.AddScoped<AuthService>();

// DB設定
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// JWT認証設定
builder.Services.AddScoped<JwtTokenGenerator>();

var jwtSetting = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "Roles",
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSetting["Issuer"],
        ValidAudience = jwtSetting["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSetting["Key"]))
    };
});

// ログ設定
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // ログレベル(Debug以上を記録)
    .WriteTo.Console() // コンソール出力
    .WriteTo.File(
        path: "logs/app-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/app-.json", // JSON形式で保存
        rollingInterval: RollingInterval.Day,
        formatter: new Serilog.Formatting.Json.JsonFormatter())
        .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();


app.UseCors("AllowFrontend");

// 認証・承認有効化
app.UseAuthentication(); // トークン認証
app.UseAuthorization(); // [Authorize]の適用

app.MapControllers(); // [ApiController]属性使用設定

app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureDeleted(); // DB完全削除 本番環境では必ずコメントアウトまたは削除すること
    dbContext.Database.EnsureCreated(); // DB完全初期化 本番環境では必ずコメントアウトまたは削除すること
    AppDbContextSeed.Seed(dbContext); // Seed実行;
}

app.Run();