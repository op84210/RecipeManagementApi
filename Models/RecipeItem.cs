using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeManagementApi.Models
{
    /// <summary>
    /// 配方項目實體模型
    /// 代表配方中的單一材料及其用量
    /// </summary>
    public class RecipeItem
    {
        /// <summary>
        /// 配方項目唯一識別碼
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 關聯的配方 ID
        /// </summary>
        [Required]
        public int RecipeId { get; set; }

        /// <summary>
        /// 關聯的配方
        /// </summary>
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; } = null!;

        /// <summary>
        /// 關聯的材料 ID
        /// </summary>
        [Required]
        public int MaterialId { get; set; }

        /// <summary>
        /// 關聯的材料
        /// </summary>
        [ForeignKey("MaterialId")]
        public virtual Material Material { get; set; } = null!;

        /// <summary>
        /// 所需數量
        /// 在此配方中需要的材料數量
        /// </summary>
        [Required]
        [Range(0.001, double.MaxValue, ErrorMessage = "數量必須大於 0")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// 計量單位
        /// 該材料在此配方中使用的單位
        /// </summary>
        /// <example>公克</example>
        [Required]
        [MaxLength(20)]
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// 單位換算比例
        /// 與材料標準單位的換算比例（此單位 = 標準單位 × 換算比例）
        /// </summary>
        public decimal ConversionRatio { get; set; } = 1.0m;

        /// <summary>
        /// 排序順序
        /// 在配方中的顯示順序
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 材料類型
        /// 在此配方中的材料用途分類
        /// </summary>
        public RecipeItemType ItemType { get; set; } = RecipeItemType.MainIngredient;

        /// <summary>
        /// 是否為可選材料
        /// 標示此材料是否為必需的
        /// </summary>
        public bool IsOptional { get; set; } = false;

        /// <summary>
        /// 備註說明
        /// 關於此材料在配方中的特殊說明
        /// </summary>
        /// <example>需過篩後使用</example>
        [MaxLength(300)]
        public string? Notes { get; set; }

        /// <summary>
        /// 預估成本
        /// 此項目的預估成本（數量 × 單位成本）
        /// </summary>
        public decimal EstimatedCost { get; set; }

        /// <summary>
        /// 創建日期
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後更新日期
        /// </summary>
        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// 配方項目類型列舉
    /// 分類材料在配方中的用途
    /// </summary>
    public enum RecipeItemType
    {
        /// <summary>
        /// 主要成分
        /// 產品的主要組成材料
        /// </summary>
        MainIngredient = 0,

        /// <summary>
        /// 輔助成分
        /// 輔助性的材料
        /// </summary>
        Additive = 1,

        /// <summary>
        /// 調味料
        /// 用於調味的材料
        /// </summary>
        Seasoning = 2,

        /// <summary>
        /// 防腐劑
        /// 用於保存的化學物質
        /// </summary>
        Preservative = 3,

        /// <summary>
        /// 色素
        /// 用於調色的材料
        /// </summary>
        Colorant = 4,

        /// <summary>
        /// 香料
        /// 用於增加香味的材料
        /// </summary>
        Fragrance = 5,

        /// <summary>
        /// 催化劑
        /// 促進化學反應的物質
        /// </summary>
        Catalyst = 6,

        /// <summary>
        /// 其他
        /// 未分類的材料類型
        /// </summary>
        Other = 7
    }
}
