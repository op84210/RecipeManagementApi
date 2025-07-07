using System.ComponentModel.DataAnnotations;

namespace RecipeManagementApi.Models
{
    /// <summary>
    /// 材料實體模型
    /// 代表製作產品所需的原物料或材料
    /// </summary>
    public class Material
    {
        /// <summary>
        /// 材料唯一識別碼
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 材料名稱
        /// </summary>
        /// <example>麵粉</example>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 材料描述
        /// 詳細說明材料的特性或用途
        /// </summary>
        /// <example>高筋麵粉，蛋白質含量12%以上</example>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// 材料分類
        /// </summary>
        public MaterialCategory Category { get; set; }

        /// <summary>
        /// 標準單位
        /// 該材料的標準計量單位
        /// </summary>
        /// <example>公克</example>
        [Required]
        [MaxLength(20)]
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// 每單位成本
        /// 材料的單位成本價格
        /// </summary>
        public decimal CostPerUnit { get; set; }

        /// <summary>
        /// 供應商
        /// 材料的主要供應商
        /// </summary>
        /// <example>統一麵粉股份有限公司</example>
        [MaxLength(200)]
        public string? Supplier { get; set; }

        /// <summary>
        /// 庫存量
        /// 目前的庫存數量
        /// </summary>
        public decimal StockQuantity { get; set; }

        /// <summary>
        /// 最低庫存警戒線
        /// 當庫存低於此數量時需要補貨
        /// </summary>
        public decimal MinimumStock { get; set; }

        /// <summary>
        /// 是否啟用
        /// 材料是否仍在使用中
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
        /// 使用此材料的配方項目
        /// </summary>
        public virtual ICollection<RecipeItem> RecipeItems { get; set; } = new List<RecipeItem>();
    }

    /// <summary>
    /// 材料分類列舉
    /// 用於分類不同類型的材料
    /// </summary>
    public enum MaterialCategory
    {
        /// <summary>
        /// 原料
        /// 基本的原始材料
        /// </summary>
        RawMaterial = 0,

        /// <summary>
        /// 化學品
        /// 化學物質或添加劑
        /// </summary>
        Chemical = 1,

        /// <summary>
        /// 包裝材料
        /// 用於包裝的材料
        /// </summary>
        Packaging = 2,

        /// <summary>
        /// 半成品
        /// 部分加工的中間產品
        /// </summary>
        SemiFinished = 3,

        /// <summary>
        /// 工具耗材
        /// 製程中消耗的工具材料
        /// </summary>
        Consumable = 4,

        /// <summary>
        /// 其他
        /// 未分類的材料
        /// </summary>
        Other = 5
    }
}
