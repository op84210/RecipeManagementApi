using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RecipeManagementApi.Data;
using RecipeManagementApi.Services;
using System.Reflection;

// 配方管理 Web API 應用程式進入點
// 提供完整的配方、材料、產品管理功能

var builder = WebApplication.CreateBuilder(args);

// Railway PORT 支援 - 雲端部署時使用動態 PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddControllers();

// 資料庫配置 - 智慧選擇 PostgreSQL 或 InMemory
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
}

if (!string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("🗄️ 使用 PostgreSQL 資料庫儲存");
    
    // Railway PostgreSQL 格式轉換 (postgresql://user:pass@host:port/database 或 postgres://user:pass@host:port/database)
    if (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://"))
    {
        try
        {
            var uri = new Uri(connectionString);
            var userInfo = uri.UserInfo.Split(':');
            var username = userInfo[0];
            var password = userInfo.Length > 1 ? userInfo[1] : "";
            var database = uri.AbsolutePath.Trim('/');
            
            connectionString = $"Host={uri.Host};Port={uri.Port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ PostgreSQL 連接字串解析失敗: {ex.Message}");
            throw;
        }
    }
    
    // 註冊 PostgreSQL DbContext
    builder.Services.AddDbContext<RecipeDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    Console.WriteLine("🧠 使用記憶體儲存（開發模式）");
    
    // 註冊 InMemory DbContext (開發/測試用)
    builder.Services.AddDbContext<RecipeDbContext>(options =>
        options.UseInMemoryDatabase("RecipeManagementDb"));
}

// 註冊服務
builder.Services.AddScoped<IRecipeService, RecipeService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Recipe Management API",
        Version = "v1",
        Description = "配方管理系統 API - 管理產品、配方、材料和配方項目",
        Contact = new OpenApiContact
        {
            Name = "Recipe Management System",
            Email = "contact@recipemanagement.com"
        }
    });

    // 加入 XML 註釋檔案以支援 Swagger 文件
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// 配置 CORS（跨域請求）
// 允許前端應用程式呼叫此 API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// 資料庫自動遷移（僅在有資料庫時）
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();
    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL")) || 
        !string.IsNullOrEmpty(builder.Configuration.GetConnectionString("DefaultConnection")))
    {
        try
        {
            Console.WriteLine("🔄 執行資料庫遷移...");
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("✅ 資料庫遷移完成");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 資料庫遷移失敗: {ex.Message}");
            Console.WriteLine("⚠️ 將使用記憶體儲存模式");
        }
    }
    else
    {
        Console.WriteLine("ℹ️ 初始化記憶體資料庫");
        context.Database.EnsureCreated();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 開發環境啟用 Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe Management API v1");
        c.RoutePrefix = string.Empty; // 讓 Swagger UI 成為根頁面
        c.DocumentTitle = "配方管理 API 文件";
    });
}
else
{
    // 生產環境也啟用 Swagger（方便測試）
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe Management API v1");
        c.RoutePrefix = string.Empty;
        c.DocumentTitle = "配方管理 API 文件";
    });
}

// 健康檢查端點（Railway 部署需要）
app.MapGet("/health", () => new { 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName 
});

// 環境變數診斷端點（僅限開發/除錯用）
app.MapGet("/debug/env", () => 
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    var port = Environment.GetEnvironmentVariable("PORT") ?? "not set";
    var railwayEnvironment = Environment.GetEnvironmentVariable("RAILWAY_ENVIRONMENT") ?? "not set";
    
    return new 
    { 
        timestamp = DateTime.UtcNow,
        environment = app.Environment.EnvironmentName,
        databaseUrl = new 
        {
            exists = databaseUrl != null,
            length = databaseUrl?.Length ?? 0,
            preview = databaseUrl?.Length > 0 ? databaseUrl.Substring(0, Math.Min(30, databaseUrl.Length)) + "..." : "empty"
        },
        port,
        railwayEnvironment,
        message = "檢查 Railway 控制台中的 DATABASE_URL 環境變數設定"
    };
});

// 根路徑重導向到 Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

// 取消 HTTPS 重導向（Railway 會在 Proxy 層處理 HTTPS）
// app.UseHttpsRedirection();

// 啟用 CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
