using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RecipeManagementApi.Data;
using RecipeManagementApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 註冊 DbContext (使用 InMemory 資料庫)
builder.Services.AddDbContext<RecipeDbContext>(options =>
    options.UseInMemoryDatabase("RecipeManagementDb"));

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

var app = builder.Build();

// 確保資料庫已建立並初始化種子資料
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe Management API v1");
        c.RoutePrefix = string.Empty; // 讓 Swagger UI 在根路徑顯示
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
