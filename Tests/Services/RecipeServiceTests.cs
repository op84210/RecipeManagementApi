using Xunit;
using RecipeManagementApi.Services;
using RecipeManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace RecipeManagementApi.Tests.Services
{
    /// <summary>
    /// RecipeService 單元測試
    /// 測試配方管理服務的各項功能
    /// </summary>
    public class RecipeServiceTests : TestBase
    {
        private readonly RecipeService _recipeService;

        public RecipeServiceTests()
        {
            _recipeService = new RecipeService(Context);
        }

        #region 產品管理測試

        [Fact]
        public async Task GetProductsAsync_應該返回產品列表()
        {
            // Arrange
            var query = new QueryDto { Page = 1, PageSize = 10 };

            // Act
            var result = await _recipeService.GetProductsAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "巧克力蛋糕");
            Assert.Contains(result, p => p.Name == "香草餅乾");
        }

        [Fact]
        public async Task GetProductsAsync_搜尋功能應該正常運作()
        {
            // Arrange
            var query = new QueryDto { Search = "巧克力", Page = 1, PageSize = 10 };

            // Act
            var result = await _recipeService.GetProductsAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Equal("巧克力蛋糕", result.First().Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_應該返回正確的產品()
        {
            // Act
            var result = await _recipeService.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("巧克力蛋糕", result.Name);
            Assert.Equal("CAKE001", result.ProductCode);
        }

        [Fact]
        public async Task GetProductByIdAsync_不存在的ID應該返回null()
        {
            // Act
            var result = await _recipeService.GetProductByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateProductAsync_應該成功建立產品()
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = "提拉米蘇",
                ProductCode = "DESSERT001",
                Category = ProductCategory.Food,
                StandardYield = 6,
                YieldUnit = "片",
                StandardPrice = 420m,
                EstimatedProductionTimeMinutes = 180
            };

            // Act
            var result = await _recipeService.CreateProductAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("提拉米蘇", result.Name);
            Assert.Equal("DESSERT001", result.ProductCode);
            Assert.True(result.Id > 0);

            // 驗證資料庫中確實有新增
            var productInDb = await Context.Products.FindAsync(result.Id);
            Assert.NotNull(productInDb);
            Assert.Equal("提拉米蘇", productInDb.Name);
        }

        [Fact]
        public async Task UpdateProductAsync_應該成功更新產品()
        {
            // Arrange
            var updateDto = new UpdateProductDto
            {
                Name = "頂級巧克力蛋糕",
                StandardPrice = 500m
            };

            // Act
            var result = await _recipeService.UpdateProductAsync(1, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("頂級巧克力蛋糕", result.Name);
            Assert.Equal(500m, result.StandardPrice);

            // 驗證資料庫中確實有更新
            var productInDb = await Context.Products.FindAsync(1);
            Assert.Equal("頂級巧克力蛋糕", productInDb.Name);
            Assert.Equal(500m, productInDb.StandardPrice);
        }

        [Fact]
        public async Task DeleteProductAsync_應該成功刪除產品()
        {
            // Act
            var result = await _recipeService.DeleteProductAsync(2);

            // Assert
            Assert.True(result);

            // 驗證資料庫中確實已刪除
            var productInDb = await Context.Products.FindAsync(2);
            Assert.Null(productInDb);
        }

        #endregion

        #region 材料管理測試

        [Fact]
        public async Task GetMaterialsAsync_應該返回材料列表()
        {
            // Arrange
            var query = new QueryDto { Page = 1, PageSize = 10 };

            // Act
            var result = await _recipeService.GetMaterialsAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(result, m => m.Name == "麵粉");
            Assert.Contains(result, m => m.Name == "砂糖");
            Assert.Contains(result, m => m.Name == "雞蛋");
        }

        [Fact]
        public async Task GetMaterialByIdAsync_應該返回正確的材料()
        {
            // Act
            var result = await _recipeService.GetMaterialByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("麵粉", result.Name);
            Assert.Equal("公克", result.Unit);
            Assert.Equal(0.01m, result.CostPerUnit);
        }

        [Fact]
        public async Task CreateMaterialAsync_應該成功建立材料()
        {
            // Arrange
            var createDto = new CreateMaterialDto
            {
                Name = "奶油",
                Unit = "公克",
                CostPerUnit = 0.15m,
                Category = MaterialCategory.RawMaterial,
                StockQuantity = 200,
                MinimumStock = 50
            };

            // Act
            var result = await _recipeService.CreateMaterialAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("奶油", result.Name);
            Assert.Equal(0.15m, result.CostPerUnit);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task DeleteMaterialAsync_有使用的材料應該設為停用()
        {
            // Act - 刪除麵粉（在配方中有使用）
            var result = await _recipeService.DeleteMaterialAsync(1);

            // Assert
            Assert.True(result);

            // 驗證材料沒有被真正刪除，而是設為停用
            var materialInDb = await Context.Materials.FindAsync(1);
            Assert.NotNull(materialInDb);
            Assert.False(materialInDb.IsActive);
        }

        #endregion

        #region 配方管理測試

        [Fact]
        public async Task GetRecipesAsync_應該返回配方列表()
        {
            // Arrange
            var query = new QueryDto { Page = 1, PageSize = 10 };

            // Act
            var result = await _recipeService.GetRecipesAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("經典巧克力蛋糕", result.First().Name);
        }

        [Fact]
        public async Task GetRecipeByIdAsync_應該返回完整的配方資訊()
        {
            // Act
            var result = await _recipeService.GetRecipeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("經典巧克力蛋糕", result.Name);
            Assert.NotNull(result.Product);
            Assert.Equal("巧克力蛋糕", result.Product.Name);
            Assert.NotEmpty(result.Items);
            Assert.Equal(3, result.Items.Count);
        }

        [Fact]
        public async Task CreateRecipeAsync_應該成功建立配方()
        {
            // Arrange
            var createDto = new CreateRecipeDto
            {
                Name = "香草餅乾配方",
                Version = "1.0",
                ProductId = 2,
                BatchYield = 24,
                CreatedBy = "測試廚師",
                Instructions = "製作香草餅乾的步驟"
            };

            // Act
            var result = await _recipeService.CreateRecipeAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("香草餅乾配方", result.Name);
            Assert.Equal(2, result.ProductId);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task SetPrimaryRecipeAsync_應該正確設定主要配方()
        {
            // Arrange - 先建立第二個配方
            var recipe2 = new Recipe
            {
                Name = "巧克力蛋糕配方2",
                Version = "2.0",
                ProductId = 1,
                BatchYield = 8,
                CreatedBy = "測試廚師"
            };
            Context.Recipes.Add(recipe2);
            await Context.SaveChangesAsync();

            // Act - 將新配方設為主要配方
            var result = await _recipeService.SetPrimaryRecipeAsync(recipe2.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsPrimary);

            // 驗證原本的主要配方已不是主要配方
            var originalRecipe = await Context.Recipes.FindAsync(1);
            Assert.False(originalRecipe.IsPrimary);
        }

        #endregion

        #region 成本計算測試

        [Fact]
        public async Task CalculateRecipeCostAsync_應該正確計算配方成本()
        {
            // Act
            var result = await _recipeService.CalculateRecipeCostAsync(1);

            // Assert
            // 麵粉 200g * 0.01 = 2元
            // 砂糖 150g * 0.02 = 3元  
            // 雞蛋 3顆 * 8 = 24元
            // 總計 = 29元
            Assert.Equal(29m, result);
        }

        [Fact]
        public async Task UpdateRecipeCostAsync_應該更新配方成本到資料庫()
        {
            // Act
            await _recipeService.UpdateRecipeCostAsync(1);

            // Assert
            var recipe = await Context.Recipes.FindAsync(1);
            Assert.Equal(29m, recipe.EstimatedCost);
        }

        #endregion

        #region 配方項目管理測試

        [Fact]
        public async Task AddRecipeItemAsync_應該成功新增配方項目()
        {
            // Arrange
            var createDto = new CreateRecipeItemDto
            {
                MaterialId = 2, // 砂糖
                Quantity = 100,
                Unit = "公克",
                ConversionRatio = 1.0m,
                SortOrder = 4,
                ItemType = RecipeItemType.MainIngredient
            };

            // Act
            var result = await _recipeService.AddRecipeItemAsync(1, createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.MaterialId);
            Assert.Equal(100, result.Quantity);
            Assert.Equal(2.0m, result.EstimatedCost); // 100 * 0.02

            // 驗證配方總成本有更新
            var recipe = await Context.Recipes.FindAsync(1);
            Assert.Equal(31m, recipe.EstimatedCost); // 原本29 + 新增2
        }

        [Fact]
        public async Task DeleteRecipeItemAsync_應該成功刪除配方項目並更新成本()
        {
            // Arrange
            var initialCost = await _recipeService.CalculateRecipeCostAsync(1);

            // Act - 刪除砂糖項目（ID=2）
            var result = await _recipeService.DeleteRecipeItemAsync(2);

            // Assert
            Assert.True(result);

            // 驗證項目已刪除
            var deletedItem = await Context.RecipeItems.FindAsync(2);
            Assert.Null(deletedItem);

            // 驗證配方成本已更新
            var recipe = await Context.Recipes.FindAsync(1);
            Assert.Equal(26m, recipe.EstimatedCost); // 29 - 3 = 26
        }

        #endregion
    }
}
