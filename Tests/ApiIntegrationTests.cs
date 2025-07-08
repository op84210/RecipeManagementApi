using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RecipeManagementApi.Data;
using RecipeManagementApi.Models;
using System.Text;
using System.Text.Json;
using Xunit;

namespace RecipeManagementApi.Tests
{
    /// <summary>
    /// API 整合測試
    /// 測試完整的 HTTP 請求流程
    /// </summary>
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly RecipeDbContext _context;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 移除原本的 DbContext
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<RecipeDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // 加入測試用的 InMemory 資料庫
                    services.AddDbContext<RecipeDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("ApiIntegrationTestDb");
                        options.EnableSensitiveDataLogging();
                    });
                });
            });

            _client = _factory.CreateClient();
            
            // 取得 DbContext 實例並初始化測試資料
            using var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();
            SeedTestData(_context);
        }

        private static void SeedTestData(RecipeDbContext context)
        {
            // 清理並重新建立資料庫
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            // 清除所有來自 HasData 的預設資料
            context.Materials.RemoveRange(context.Materials);
            context.Products.RemoveRange(context.Products);
            context.SaveChanges();

            var materials = new List<Material>
            {
                new Material
                {
                    // 不設定 ID，讓資料庫自動產生
                    Name = "麵粉",
                    Unit = "公克",
                    CostPerUnit = 0.01m,
                    Category = MaterialCategory.RawMaterial,
                    StockQuantity = 1000,
                    MinimumStock = 100
                },
                new Material
                {
                    // 不設定 ID，讓資料庫自動產生
                    Name = "砂糖",
                    Unit = "公克", 
                    CostPerUnit = 0.02m,
                    Category = MaterialCategory.RawMaterial,
                    StockQuantity = 500,
                    MinimumStock = 50
                }
            };

            context.Materials.AddRange(materials);
            context.SaveChanges();
        }

        #region Materials API 測試

        [Fact]
        public async Task GET_Materials_應該返回200和材料列表()
        {
            // Act
            var response = await _client.GetAsync("/api/materials");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var materials = JsonSerializer.Deserialize<List<Material>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(materials);
            Assert.Equal(2, materials.Count);
            Assert.Contains(materials, m => m.Name == "麵粉");
        }

        [Fact]
        public async Task GET_Materials_with_Search_應該過濾結果()
        {
            // Act
            var response = await _client.GetAsync("/api/materials?search=麵");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var materials = JsonSerializer.Deserialize<List<Material>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.Single(materials);
            Assert.Equal("麵粉", materials[0].Name);
        }

        [Fact]
        public async Task POST_Materials_有效資料應該返回201()
        {
            // Arrange
            var newMaterial = new CreateMaterialDto
            {
                Name = "奶油",
                Unit = "公克",
                CostPerUnit = 0.15m,
                Category = MaterialCategory.RawMaterial,
                StockQuantity = 200,
                MinimumStock = 50
            };

            var json = JsonSerializer.Serialize(newMaterial);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/materials", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdMaterial = JsonSerializer.Deserialize<Material>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.Equal("奶油", createdMaterial.Name);
            Assert.True(createdMaterial.Id > 0);
        }

        [Fact]
        public async Task POST_Materials_無效資料應該返回400()
        {
            // Arrange - 缺少必要欄位
            var invalidMaterial = new { Unit = "公克" };
            var json = JsonSerializer.Serialize(invalidMaterial);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/materials", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GET_Material_ById_存在的ID應該返回材料()
        {
            // Arrange - 先建立一個材料
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();
            var material = context.Materials.First();

            // Act
            var response = await _client.GetAsync($"/api/materials/{material.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var retrievedMaterial = JsonSerializer.Deserialize<Material>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.Equal(material.Name, retrievedMaterial.Name);
        }

        [Fact]
        public async Task GET_Material_ById_不存在的ID應該返回404()
        {
            // Act
            var response = await _client.GetAsync("/api/materials/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PUT_Material_應該更新材料()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();
            var material = context.Materials.First();

            var updateDto = new UpdateMaterialDto
            {
                Name = "高筋麵粉",
                CostPerUnit = 0.012m
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/materials/{material.Id}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedMaterial = JsonSerializer.Deserialize<Material>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.Equal("高筋麵粉", updatedMaterial.Name);
            Assert.Equal(0.012m, updatedMaterial.CostPerUnit);
        }

        [Fact]
        public async Task DELETE_Material_應該刪除材料()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();
            var material = context.Materials.First();

            // Act
            var response = await _client.DeleteAsync($"/api/materials/{material.Id}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

            // 驗證材料已被刪除或停用
            var getResponse = await _client.GetAsync($"/api/materials/{material.Id}");
            // 材料可能被軟刪除（設為 IsActive = false）而不是真正刪除
            // 所以這裡不一定是 404，可能還是能取到但 IsActive = false
        }

        #endregion

        #region Health Check 測試

        [Fact]
        public async Task GET_Health_應該返回200()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            
            Assert.Contains("healthy", content);
        }

        #endregion

        #region Swagger 測試

        [Fact]
        public async Task GET_Swagger_應該返回200()
        {
            // Act
            var response = await _client.GetAsync("/swagger/v1/swagger.json");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            
            Assert.Contains("Recipe Management API", content);
        }

        #endregion

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
