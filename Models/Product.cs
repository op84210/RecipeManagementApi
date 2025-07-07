using System.ComponentModel.DataAnnotations;

namespace RecipeManagementApi.Models
{
    /// <summary>
    /// 產品實體模型
    /// 代表可以製造的最終產品
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 產品唯一識別碼
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        /// <example>白土司麵包</example>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 產品編號
        /// 用於內部管理的產品代碼
        /// </summary>
        /// <example>BREAD-001</example>
        [Required]
        [MaxLength(50)]
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 產品描述
        /// 詳細說明產品的特性和用途
        /// </summary>
        /// <example>使用高筋麵粉製作的白土司，口感鬆軟</example>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// 產品分類
        /// </summary>
        public ProductCategory Category { get; set; }

        /// <summary>
        /// 標準產量
        /// 單次生產的標準數量
        /// </summary>
        public decimal StandardYield { get; set; }

        /// <summary>
        /// 產量單位
        /// 標準產量的計量單位
        /// </summary>
        /// <example>條</example>
        [Required]
        [MaxLength(20)]
        public string YieldUnit { get; set; } = string.Empty;

        /// <summary>
        /// 預估製作時間（分鐘）
        /// 完成一批產品所需的時間
        /// </summary>
        public int EstimatedProductionTimeMinutes { get; set; }

        /// <summary>
        /// 標準售價
        /// 產品的建議零售價
        /// </summary>
        public decimal StandardPrice { get; set; }

        /// <summary>
        /// 是否啟用
        /// 產品是否仍在生產中
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 創建日期
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後更新日期
        /// </summary>
        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 產品配方
        /// 製作此產品的所有配方版本
        /// </summary>
        public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }

    /// <summary>
    /// 產品分類列舉
    /// 用於分類不同類型的產品
    /// </summary>
    public enum ProductCategory
    {
        /// <summary>
        /// 食品
        /// 可食用的產品
        /// </summary>
        Food = 0,

        /// <summary>
        /// 飲料
        /// 液體類產品
        /// </summary>
        Beverage = 1,

        /// <summary>
        /// 化妝品
        /// 美容保養產品
        /// </summary>
        Cosmetics = 2,

        /// <summary>
        /// 藥品
        /// 醫療用途產品
        /// </summary>
        Pharmaceutical = 3,

        /// <summary>
        /// 化學品
        /// 工業化學產品
        /// </summary>
        Chemical = 4,

        /// <summary>
        /// 電子產品
        /// 電子設備或元件
        /// </summary>
        Electronics = 5,

        /// <summary>
        /// 機械零件
        /// 機械設備零組件
        /// </summary>
        MechanicalParts = 6,

        /// <summary>
        /// 其他
        /// 未分類的產品
        /// </summary>
        Other = 7
    }
}
