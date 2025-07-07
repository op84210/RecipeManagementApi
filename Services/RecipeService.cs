using Microsoft.EntityFrameworkCore;
using RecipeManagementApi.Data;
using RecipeManagementApi.Models;

namespace RecipeManagementApi.Services
{
    /// <summary>
    /// 配方管理服務介面
    /// 定義配方相關的業務邏輯操作
    /// </summary>
    public interface IRecipeService
    {
        // 產品管理
        Task<IEnumerable<Product>> GetProductsAsync(QueryDto query);
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(CreateProductDto dto);
        Task<Product?> UpdateProductAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteProductAsync(int id);

        // 材料管理
        Task<IEnumerable<Material>> GetMaterialsAsync(QueryDto query);
        Task<Material?> GetMaterialByIdAsync(int id);
        Task<Material> CreateMaterialAsync(CreateMaterialDto dto);
        Task<Material?> UpdateMaterialAsync(int id, UpdateMaterialDto dto);
        Task<bool> DeleteMaterialAsync(int id);

        // 配方管理
        Task<IEnumerable<Recipe>> GetRecipesAsync(QueryDto query);
        Task<Recipe?> GetRecipeByIdAsync(int id);
        Task<IEnumerable<Recipe>> GetRecipesByProductIdAsync(int productId);
        Task<Recipe> CreateRecipeAsync(CreateRecipeDto dto);
        Task<Recipe?> UpdateRecipeAsync(int id, UpdateRecipeDto dto);
        Task<bool> DeleteRecipeAsync(int id);
        Task<Recipe?> SetPrimaryRecipeAsync(int recipeId);

        // 配方項目管理
        Task<IEnumerable<RecipeItem>> GetRecipeItemsAsync(int recipeId);
        Task<RecipeItem?> GetRecipeItemByIdAsync(int id);
        Task<RecipeItem> AddRecipeItemAsync(int recipeId, CreateRecipeItemDto dto);
        Task<RecipeItem?> UpdateRecipeItemAsync(int id, UpdateRecipeItemDto dto);
        Task<bool> DeleteRecipeItemAsync(int id);

        // 成本計算
        Task<decimal> CalculateRecipeCostAsync(int recipeId);
        Task UpdateRecipeCostAsync(int recipeId);
    }

    /// <summary>
    /// 配方管理服務實作
    /// </summary>
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDbContext _context;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        public RecipeService(RecipeDbContext context)
        {
            _context = context;
        }

        #region 產品管理

        /// <summary>
        /// 取得產品清單
        /// </summary>
        public async Task<IEnumerable<Product>> GetProductsAsync(QueryDto query)
        {
            var products = _context.Products.AsQueryable();

            if (query.OnlyActive == true)
                products = products.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(query.Search))
            {
                var searchTerm = query.Search.ToLower();
                products = products.Where(p => 
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.ProductCode.ToLower().Contains(searchTerm) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchTerm)));
            }

            return await products
                .OrderBy(p => p.Name)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 取得特定產品
        /// </summary>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Recipes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// 創建產品
        /// </summary>
        public async Task<Product> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                ProductCode = dto.ProductCode,
                Description = dto.Description,
                Category = dto.Category,
                StandardYield = dto.StandardYield,
                YieldUnit = dto.YieldUnit,
                EstimatedProductionTimeMinutes = dto.EstimatedProductionTimeMinutes,
                StandardPrice = dto.StandardPrice
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// 更新產品
        /// </summary>
        public async Task<Product?> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            if (!string.IsNullOrEmpty(dto.Name))
                product.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.ProductCode))
                product.ProductCode = dto.ProductCode;
            if (dto.Description != null)
                product.Description = dto.Description;
            if (dto.Category.HasValue)
                product.Category = dto.Category.Value;
            if (dto.StandardYield.HasValue)
                product.StandardYield = dto.StandardYield.Value;
            if (!string.IsNullOrEmpty(dto.YieldUnit))
                product.YieldUnit = dto.YieldUnit;
            if (dto.EstimatedProductionTimeMinutes.HasValue)
                product.EstimatedProductionTimeMinutes = dto.EstimatedProductionTimeMinutes.Value;
            if (dto.StandardPrice.HasValue)
                product.StandardPrice = dto.StandardPrice.Value;
            if (dto.IsActive.HasValue)
                product.IsActive = dto.IsActive.Value;

            product.LastUpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// 刪除產品
        /// </summary>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region 材料管理

        /// <summary>
        /// 取得材料清單
        /// </summary>
        public async Task<IEnumerable<Material>> GetMaterialsAsync(QueryDto query)
        {
            var materials = _context.Materials.AsQueryable();

            if (query.OnlyActive == true)
                materials = materials.Where(m => m.IsActive);

            if (!string.IsNullOrEmpty(query.Search))
            {
                var searchTerm = query.Search.ToLower();
                materials = materials.Where(m => 
                    m.Name.ToLower().Contains(searchTerm) ||
                    (m.Description != null && m.Description.ToLower().Contains(searchTerm)) ||
                    (m.Supplier != null && m.Supplier.ToLower().Contains(searchTerm)));
            }

            return await materials
                .OrderBy(m => m.Name)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 取得特定材料
        /// </summary>
        public async Task<Material?> GetMaterialByIdAsync(int id)
        {
            return await _context.Materials
                .Include(m => m.RecipeItems)
                .ThenInclude(ri => ri.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// 創建材料
        /// </summary>
        public async Task<Material> CreateMaterialAsync(CreateMaterialDto dto)
        {
            var material = new Material
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                Unit = dto.Unit,
                CostPerUnit = dto.CostPerUnit,
                Supplier = dto.Supplier,
                StockQuantity = dto.StockQuantity,
                MinimumStock = dto.MinimumStock
            };

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
            return material;
        }

        /// <summary>
        /// 更新材料
        /// </summary>
        public async Task<Material?> UpdateMaterialAsync(int id, UpdateMaterialDto dto)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return null;

            if (!string.IsNullOrEmpty(dto.Name))
                material.Name = dto.Name;
            if (dto.Description != null)
                material.Description = dto.Description;
            if (dto.Category.HasValue)
                material.Category = dto.Category.Value;
            if (!string.IsNullOrEmpty(dto.Unit))
                material.Unit = dto.Unit;
            if (dto.CostPerUnit.HasValue)
                material.CostPerUnit = dto.CostPerUnit.Value;
            if (dto.Supplier != null)
                material.Supplier = dto.Supplier;
            if (dto.StockQuantity.HasValue)
                material.StockQuantity = dto.StockQuantity.Value;
            if (dto.MinimumStock.HasValue)
                material.MinimumStock = dto.MinimumStock.Value;
            if (dto.IsActive.HasValue)
                material.IsActive = dto.IsActive.Value;

            material.LastUpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return material;
        }

        /// <summary>
        /// 刪除材料
        /// </summary>
        public async Task<bool> DeleteMaterialAsync(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return false;

            // 檢查是否有配方在使用此材料
            var isInUse = await _context.RecipeItems.AnyAsync(ri => ri.MaterialId == id);
            if (isInUse)
            {
                // 如果有在使用，則設為停用而不是刪除
                material.IsActive = false;
                material.LastUpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region 配方管理

        /// <summary>
        /// 取得配方清單
        /// </summary>
        public async Task<IEnumerable<Recipe>> GetRecipesAsync(QueryDto query)
        {
            var recipes = _context.Recipes
                .Include(r => r.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                var searchTerm = query.Search.ToLower();
                recipes = recipes.Where(r => 
                    r.Name.ToLower().Contains(searchTerm) ||
                    r.Product.Name.ToLower().Contains(searchTerm) ||
                    (r.Description != null && r.Description.ToLower().Contains(searchTerm)));
            }

            return await recipes
                .OrderBy(r => r.Product.Name)
                .ThenBy(r => r.Name)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 取得特定配方
        /// </summary>
        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipes
                .Include(r => r.Product)
                .Include(r => r.Items)
                .ThenInclude(ri => ri.Material)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// 取得特定產品的所有配方
        /// </summary>
        public async Task<IEnumerable<Recipe>> GetRecipesByProductIdAsync(int productId)
        {
            return await _context.Recipes
                .Include(r => r.Items)
                .ThenInclude(ri => ri.Material)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.IsPrimary)
                .ThenBy(r => r.Name)
                .ToListAsync();
        }

        /// <summary>
        /// 創建配方
        /// </summary>
        public async Task<Recipe> CreateRecipeAsync(CreateRecipeDto dto)
        {
            var recipe = new Recipe
            {
                Name = dto.Name,
                Version = dto.Version,
                Description = dto.Description,
                ProductId = dto.ProductId,
                BatchYield = dto.BatchYield,
                Instructions = dto.Instructions,
                CreatedBy = dto.CreatedBy
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        /// <summary>
        /// 更新配方
        /// </summary>
        public async Task<Recipe?> UpdateRecipeAsync(int id, UpdateRecipeDto dto)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return null;

            if (!string.IsNullOrEmpty(dto.Name))
                recipe.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Version))
                recipe.Version = dto.Version;
            if (dto.Description != null)
                recipe.Description = dto.Description;
            if (dto.BatchYield.HasValue)
                recipe.BatchYield = dto.BatchYield.Value;
            if (dto.Status.HasValue)
                recipe.Status = dto.Status.Value;
            if (dto.IsPrimary.HasValue)
            {
                // 如果設為主要配方，需要將同產品的其他配方設為非主要
                if (dto.IsPrimary.Value)
                {
                    var otherRecipes = await _context.Recipes
                        .Where(r => r.ProductId == recipe.ProductId && r.Id != id)
                        .ToListAsync();
                    foreach (var other in otherRecipes)
                        other.IsPrimary = false;
                }
                recipe.IsPrimary = dto.IsPrimary.Value;
            }
            if (dto.Instructions != null)
                recipe.Instructions = dto.Instructions;
            if (!string.IsNullOrEmpty(dto.ApprovedBy))
            {
                recipe.ApprovedBy = dto.ApprovedBy;
                recipe.ApprovedDate = DateTime.Now;
            }

            recipe.LastUpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            // 更新成本
            await UpdateRecipeCostAsync(id);

            return recipe;
        }

        /// <summary>
        /// 刪除配方
        /// </summary>
        public async Task<bool> DeleteRecipeAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return false;

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 設定主要配方
        /// </summary>
        public async Task<Recipe?> SetPrimaryRecipeAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null) return null;

            // 將同產品的其他配方設為非主要
            var otherRecipes = await _context.Recipes
                .Where(r => r.ProductId == recipe.ProductId && r.Id != recipeId)
                .ToListAsync();
            foreach (var other in otherRecipes)
                other.IsPrimary = false;

            recipe.IsPrimary = true;
            recipe.LastUpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return recipe;
        }

        #endregion

        #region 配方項目管理

        /// <summary>
        /// 取得配方項目清單
        /// </summary>
        public async Task<IEnumerable<RecipeItem>> GetRecipeItemsAsync(int recipeId)
        {
            return await _context.RecipeItems
                .Include(ri => ri.Material)
                .Where(ri => ri.RecipeId == recipeId)
                .OrderBy(ri => ri.SortOrder)
                .ThenBy(ri => ri.Material.Name)
                .ToListAsync();
        }

        /// <summary>
        /// 取得特定配方項目
        /// </summary>
        public async Task<RecipeItem?> GetRecipeItemByIdAsync(int id)
        {
            return await _context.RecipeItems
                .Include(ri => ri.Material)
                .Include(ri => ri.Recipe)
                .FirstOrDefaultAsync(ri => ri.Id == id);
        }

        /// <summary>
        /// 新增配方項目
        /// </summary>
        public async Task<RecipeItem> AddRecipeItemAsync(int recipeId, CreateRecipeItemDto dto)
        {
            var material = await _context.Materials.FindAsync(dto.MaterialId);
            if (material == null)
                throw new ArgumentException("找不到指定的材料");

            var recipeItem = new RecipeItem
            {
                RecipeId = recipeId,
                MaterialId = dto.MaterialId,
                Quantity = dto.Quantity,
                Unit = dto.Unit,
                ConversionRatio = dto.ConversionRatio,
                SortOrder = dto.SortOrder,
                ItemType = dto.ItemType,
                IsOptional = dto.IsOptional,
                Notes = dto.Notes
            };

            // 計算預估成本
            var standardQuantity = dto.Quantity * dto.ConversionRatio;
            recipeItem.EstimatedCost = standardQuantity * material.CostPerUnit;

            _context.RecipeItems.Add(recipeItem);
            await _context.SaveChangesAsync();

            // 更新配方總成本
            await UpdateRecipeCostAsync(recipeId);

            return recipeItem;
        }

        /// <summary>
        /// 更新配方項目
        /// </summary>
        public async Task<RecipeItem?> UpdateRecipeItemAsync(int id, UpdateRecipeItemDto dto)
        {
            var recipeItem = await _context.RecipeItems
                .Include(ri => ri.Material)
                .FirstOrDefaultAsync(ri => ri.Id == id);
            if (recipeItem == null) return null;

            if (dto.Quantity.HasValue)
                recipeItem.Quantity = dto.Quantity.Value;
            if (!string.IsNullOrEmpty(dto.Unit))
                recipeItem.Unit = dto.Unit;
            if (dto.ConversionRatio.HasValue)
                recipeItem.ConversionRatio = dto.ConversionRatio.Value;
            if (dto.SortOrder.HasValue)
                recipeItem.SortOrder = dto.SortOrder.Value;
            if (dto.ItemType.HasValue)
                recipeItem.ItemType = dto.ItemType.Value;
            if (dto.IsOptional.HasValue)
                recipeItem.IsOptional = dto.IsOptional.Value;
            if (dto.Notes != null)
                recipeItem.Notes = dto.Notes;

            // 重新計算預估成本
            var standardQuantity = recipeItem.Quantity * recipeItem.ConversionRatio;
            recipeItem.EstimatedCost = standardQuantity * recipeItem.Material.CostPerUnit;

            recipeItem.LastUpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            // 更新配方總成本
            await UpdateRecipeCostAsync(recipeItem.RecipeId);

            return recipeItem;
        }

        /// <summary>
        /// 刪除配方項目
        /// </summary>
        public async Task<bool> DeleteRecipeItemAsync(int id)
        {
            var recipeItem = await _context.RecipeItems.FindAsync(id);
            if (recipeItem == null) return false;

            var recipeId = recipeItem.RecipeId;
            _context.RecipeItems.Remove(recipeItem);
            await _context.SaveChangesAsync();

            // 更新配方總成本
            await UpdateRecipeCostAsync(recipeId);

            return true;
        }

        #endregion

        #region 成本計算

        /// <summary>
        /// 計算配方成本
        /// </summary>
        public async Task<decimal> CalculateRecipeCostAsync(int recipeId)
        {
            var items = await _context.RecipeItems
                .Where(ri => ri.RecipeId == recipeId)
                .ToListAsync();

            return items.Sum(item => item.EstimatedCost);
        }

        /// <summary>
        /// 更新配方成本
        /// </summary>
        public async Task UpdateRecipeCostAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null) return;

            recipe.EstimatedCost = await CalculateRecipeCostAsync(recipeId);
            recipe.LastUpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
