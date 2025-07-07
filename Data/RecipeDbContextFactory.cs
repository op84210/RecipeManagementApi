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
            
            // 使用預設的 PostgreSQL 連接字串進行遷移
            // 實際部署時會使用環境變數中的連接字串
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=recipemanagementdb;Username=postgres;Password=password");
            
            return new RecipeDbContext(optionsBuilder.Options);
        }
    }
}
