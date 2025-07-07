using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RecipeManagementApi.Data
{
    /// <summary>
    /// 設計時 DbContext 工廠
    /// 用於 Entity Framework Core 工具（如 migrations）在設計時創建 DbContext
    /// </summary>
    public class RecipeDbContextFactory : IDesignTimeDbContextFactory<RecipeDbContext>
    {
        public RecipeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RecipeDbContext>();
            
            // 設計時使用 PostgreSQL 提供者來生成正確的遷移
            // 連線字串只是用於模型建構，不會實際連線
            optionsBuilder.UseNpgsql("Host=dummy;Database=dummy;Username=dummy;Password=dummy");
            
            return new RecipeDbContext(optionsBuilder.Options);
        }
    }
}
