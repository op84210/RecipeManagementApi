using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeManagementApi.Models
{
    /// <summary>
    /// 配方實體模型
    /// 代表製作特定產品的完整配方
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// 配方唯一識別碼
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 配方名稱
        /// </summary>
        /// <example>白土司麵包標準配方 v1.2</example>
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 配方版本
        /// 用於追蹤配方的版本變更
        /// </summary>
        /// <example>1.2</example>
        [Required]
        [MaxLength(20)]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 配方描述
        /// 詳細說明配方的特色和注意事項
        /// </summary>
        /// <example>改良版本，增加蛋液用量提升鬆軟度</example>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// 關聯的產品 ID
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// 關聯的產品
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        /// <summary>
        /// 配方產量
        /// 使用此配方可生產的數量
        /// </summary>
        public decimal BatchYield { get; set; }

        /// <summary>
        /// 配方狀態
        /// </summary>
        public RecipeStatus Status { get; set; } = RecipeStatus.Draft;

        /// <summary>
        /// 是否為主要配方
        /// 標示此配方是否為該產品的主要使用配方
        /// </summary>
        public bool IsPrimary { get; set; } = false;

        /// <summary>
        /// 製作步驟說明
        /// 詳細的製作程序
        /// </summary>
        [MaxLength(2000)]
        public string? Instructions { get; set; }

        /// <summary>
        /// 預估總成本
        /// 根據材料用量計算的總成本
        /// </summary>
        public decimal EstimatedCost { get; set; }

        /// <summary>
        /// 創建者
        /// 配方的創建人
        /// </summary>
        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 審核者
        /// 配方的審核人
        /// </summary>
        [MaxLength(100)]
        public string? ApprovedBy { get; set; }

        /// <summary>
        /// 創建日期
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 最後更新日期
        /// </summary>
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 審核日期
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// 配方項目清單
        /// 此配方所需的所有材料和用量
        /// </summary>
        public virtual ICollection<RecipeItem> Items { get; set; } = new List<RecipeItem>();
    }

    /// <summary>
    /// 配方狀態列舉
    /// 表示配方在生命週期中的狀態
    /// </summary>
    public enum RecipeStatus
    {
        /// <summary>
        /// 草稿
        /// 配方正在編輯中，尚未完成
        /// </summary>
        Draft = 0,

        /// <summary>
        /// 待審核
        /// 配方已完成，等待審核
        /// </summary>
        PendingApproval = 1,

        /// <summary>
        /// 已審核
        /// 配方已通過審核，可以使用
        /// </summary>
        Approved = 2,

        /// <summary>
        /// 已發布
        /// 配方正在生產使用中
        /// </summary>
        Published = 3,

        /// <summary>
        /// 已停用
        /// 配方不再使用
        /// </summary>
        Inactive = 4,

        /// <summary>
        /// 已棄用
        /// 配方被新版本取代
        /// </summary>
        Deprecated = 5
    }
}
