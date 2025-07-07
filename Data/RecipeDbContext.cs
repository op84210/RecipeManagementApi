using Microsoft.EntityFrameworkCore;
using RecipeManagementApi.Models;

namespace RecipeManagementApi.Data
{
    /// <summary>
    /// 配方管理系統的資料庫上下文
    /// 管理產品、配方、材料和配方項目的資料存取
    /// </summary>
    public class RecipeDbContext : DbContext
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="options">資料庫連線選項</param>
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// 材料資料集
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        /// <summary>
        /// 產品資料集
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// 配方資料集
        /// </summary>
        public DbSet<Recipe> Recipes { get; set; }

        /// <summary>
        /// 配方項目資料集
        /// </summary>
        public DbSet<RecipeItem> RecipeItems { get; set; }

        /// <summary>
        /// 模型建立時的配置
        /// </summary>
        /// <param name="modelBuilder">模型建構器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置 Material 實體
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CostPerUnit).HasPrecision(18, 4);
                entity.Property(e => e.StockQuantity).HasPrecision(18, 4);
                entity.Property(e => e.MinimumStock).HasPrecision(18, 4);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // 配置 Product 實體
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ProductCode).IsRequired().HasMaxLength(50);
                entity.Property(e => e.YieldUnit).IsRequired().HasMaxLength(20);
                entity.Property(e => e.StandardYield).HasPrecision(18, 4);
                entity.Property(e => e.StandardPrice).HasPrecision(18, 4);
                entity.HasIndex(e => e.ProductCode).IsUnique();
            });

            // 配置 Recipe 實體
            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Version).IsRequired().HasMaxLength(20);
                entity.Property(e => e.BatchYield).HasPrecision(18, 4);
                entity.Property(e => e.EstimatedCost).HasPrecision(18, 4);
                
                // 設定與 Product 的關聯
                entity.HasOne(e => e.Product)
                      .WithMany(p => p.Recipes)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                // 建立複合索引確保同一產品的配方名稱+版本唯一
                entity.HasIndex(e => new { e.ProductId, e.Name, e.Version }).IsUnique();
            });

            // 配置 RecipeItem 實體
            modelBuilder.Entity<RecipeItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasPrecision(18, 6);
                entity.Property(e => e.ConversionRatio).HasPrecision(18, 6);
                entity.Property(e => e.EstimatedCost).HasPrecision(18, 4);
                entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);

                // 設定與 Recipe 的關聯
                entity.HasOne(e => e.Recipe)
                      .WithMany(r => r.Items)
                      .HasForeignKey(e => e.RecipeId)
                      .OnDelete(DeleteBehavior.Cascade);

                // 設定與 Material 的關聯
                entity.HasOne(e => e.Material)
                      .WithMany(m => m.RecipeItems)
                      .HasForeignKey(e => e.MaterialId)
                      .OnDelete(DeleteBehavior.Restrict); // 防止刪除仍在使用的材料

                // 確保同一配方中的材料不重複
                entity.HasIndex(e => new { e.RecipeId, e.MaterialId }).IsUnique();
            });

            // 種子資料
            SeedData(modelBuilder);
        }

        /// <summary>
        /// 設定種子資料
        /// </summary>
        /// <param name="modelBuilder">模型建構器</param>
        private static void SeedData(ModelBuilder modelBuilder)
        {
            // 材料種子資料
            modelBuilder.Entity<Material>().HasData(
                new Material
                {
                    Id = 1,
                    Name = "高筋麵粉",
                    Description = "蛋白質含量12%以上的麵粉",
                    Category = MaterialCategory.RawMaterial,
                    Unit = "公克",
                    CostPerUnit = 0.008m,
                    Supplier = "統一麵粉公司",
                    StockQuantity = 50000,
                    MinimumStock = 5000,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                },
                new Material
                {
                    Id = 2,
                    Name = "砂糖",
                    Description = "細白砂糖",
                    Category = MaterialCategory.RawMaterial,
                    Unit = "公克",
                    CostPerUnit = 0.005m,
                    Supplier = "台糖公司",
                    StockQuantity = 20000,
                    MinimumStock = 2000,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                },
                new Material
                {
                    Id = 3,
                    Name = "雞蛋",
                    Description = "新鮮雞蛋",
                    Category = MaterialCategory.RawMaterial,
                    Unit = "顆",
                    CostPerUnit = 8.0m,
                    Supplier = "大成食品",
                    StockQuantity = 500,
                    MinimumStock = 50,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                },
                new Material
                {
                    Id = 4,
                    Name = "無鹽奶油",
                    Description = "法國進口無鹽奶油",
                    Category = MaterialCategory.RawMaterial,
                    Unit = "公克",
                    CostPerUnit = 0.02m,
                    Supplier = "法國乳品進口商",
                    StockQuantity = 10000,
                    MinimumStock = 1000,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                },
                new Material
                {
                    Id = 5,
                    Name = "鹽",
                    Description = "精製食鹽",
                    Category = MaterialCategory.RawMaterial,
                    Unit = "公克",
                    CostPerUnit = 0.002m,
                    Supplier = "台鹽公司",
                    StockQuantity = 5000,
                    MinimumStock = 500,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                }
            );

            // 產品種子資料
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "白土司麵包",
                    ProductCode = "BREAD-001",
                    Description = "經典白土司麵包，口感鬆軟香甜",
                    Category = ProductCategory.Food,
                    StandardYield = 2,
                    YieldUnit = "條",
                    EstimatedProductionTimeMinutes = 180,
                    StandardPrice = 45.0m,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 2,
                    Name = "奶油小餐包",
                    ProductCode = "BREAD-002",
                    Description = "香濃奶油小餐包，適合早餐",
                    Category = ProductCategory.Food,
                    StandardYield = 12,
                    YieldUnit = "個",
                    EstimatedProductionTimeMinutes = 150,
                    StandardPrice = 60.0m,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                }
            );
        }
    }
}
