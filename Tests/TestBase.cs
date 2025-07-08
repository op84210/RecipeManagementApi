using Microsoft.EntityFrameworkCore;
using RecipeManagementApi.Data;
using RecipeManagementApi.Models;

namespace RecipeManagementApi.Tests
{
    /// <summary>
    /// 測試基礎類別
    /// 提供測試所需的共用設定和方法
    /// </summary>
    public class TestBase : IDisposable
    {
        protected RecipeDbContext Context { get; private set; }

        public TestBase()
        {
            // 建立 InMemory 測試資料庫，每個測試用不同的資料庫名稱
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            Context = new RecipeDbContext(options);

            // 不要使用 EnsureCreated()，避免執行 HasData 種子資料
            // Context.Database.EnsureCreated();

            // 初始化測試資料
            SeedTestData();
        }

        /// <summary>
        /// 初始化測試資料
        /// </summary>
        protected virtual void SeedTestData()
        {
            // 測試材料
            var materials = new List<Material>
            {
                new Material
                {
                    Id = 1,
                    Name = "麵粉",
                    Unit = "公克",
                    CostPerUnit = 0.01m,
                    Category = MaterialCategory.RawMaterial,
                    StockQuantity = 1000,
                    MinimumStock = 100,
                    Supplier = "統一麵粉公司"
                },
                new Material
                {
                    Id = 2,
                    Name = "砂糖",
                    Unit = "公克",
                    CostPerUnit = 0.02m,
                    Category = MaterialCategory.RawMaterial,
                    StockQuantity = 500,
                    MinimumStock = 50
                },
                new Material
                {
                    Id = 3,
                    Name = "雞蛋",
                    Unit = "顆",
                    CostPerUnit = 8.0m,
                    Category = MaterialCategory.RawMaterial,
                    StockQuantity = 100,
                    MinimumStock = 20
                }
            };

            // 測試產品
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "巧克力蛋糕",
                    ProductCode = "CAKE001",
                    Category = ProductCategory.Food,
                    StandardYield = 8,
                    YieldUnit = "片",
                    StandardPrice = 350m,
                    EstimatedProductionTimeMinutes = 120
                },
                new Product
                {
                    Id = 2,
                    Name = "香草餅乾",
                    ProductCode = "COOKIE001",
                    Category = ProductCategory.Food,
                    StandardYield = 24,
                    YieldUnit = "片",
                    StandardPrice = 180m,
                    EstimatedProductionTimeMinutes = 60
                }
            };

            // 測試配方
            var recipes = new List<Recipe>
            {
                new Recipe
                {
                    Id = 1,
                    Name = "經典巧克力蛋糕",
                    Version = "1.0",
                    ProductId = 1,
                    BatchYield = 8,
                    Status = RecipeStatus.Approved,
                    IsPrimary = true,
                    CreatedBy = "測試廚師",
                    Instructions = "1. 混合乾材料\n2. 加入濕材料\n3. 烘烤180度30分鐘"
                }
            };

            // 測試配方項目
            var recipeItems = new List<RecipeItem>
            {
                new RecipeItem
                {
                    Id = 1,
                    RecipeId = 1,
                    MaterialId = 1,
                    Quantity = 200,
                    Unit = "公克",
                    ConversionRatio = 1.0m,
                    EstimatedCost = 2.0m,
                    SortOrder = 1,
                    ItemType = RecipeItemType.MainIngredient
                },
                new RecipeItem
                {
                    Id = 2,
                    RecipeId = 1,
                    MaterialId = 2,
                    Quantity = 150,
                    Unit = "公克",
                    ConversionRatio = 1.0m,
                    EstimatedCost = 3.0m,
                    SortOrder = 2,
                    ItemType = RecipeItemType.MainIngredient
                },
                new RecipeItem
                {
                    Id = 3,
                    RecipeId = 1,
                    MaterialId = 3,
                    Quantity = 3,
                    Unit = "顆",
                    ConversionRatio = 1.0m,
                    EstimatedCost = 24.0m,
                    SortOrder = 3,
                    ItemType = RecipeItemType.MainIngredient
                }
            };

            Context.Materials.AddRange(materials);
            Context.Products.AddRange(products);
            Context.Recipes.AddRange(recipes);
            Context.RecipeItems.AddRange(recipeItems);
            Context.SaveChanges();
        }

        /// <summary>
        /// 清理測試資料庫
        /// </summary>
        public void Dispose()
        {
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
