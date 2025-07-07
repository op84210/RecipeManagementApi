using System.ComponentModel.DataAnnotations;
using RecipeManagementApi.Models;

namespace RecipeManagementApi.Models
{
    /// <summary>
    /// 創建材料的資料傳輸物件
    /// </summary>
    public class CreateMaterialDto
    {
        /// <summary>
        /// 材料名稱
        /// </summary>
        [Required(ErrorMessage = "材料名稱為必填")]
        [MaxLength(100, ErrorMessage = "材料名稱不可超過100字")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 材料描述
        /// </summary>
        [MaxLength(500, ErrorMessage = "材料描述不可超過500字")]
        public string? Description { get; set; }

        /// <summary>
        /// 材料分類
        /// </summary>
        public MaterialCategory Category { get; set; }

        /// <summary>
        /// 標準單位
        /// </summary>
        [Required(ErrorMessage = "單位為必填")]
        [MaxLength(20, ErrorMessage = "單位不可超過20字")]
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// 每單位成本
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "成本不可為負數")]
        public decimal CostPerUnit { get; set; }

        /// <summary>
        /// 供應商
        /// </summary>
        [MaxLength(200, ErrorMessage = "供應商名稱不可超過200字")]
        public string? Supplier { get; set; }

        /// <summary>
        /// 庫存量
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "庫存量不可為負數")]
        public decimal StockQuantity { get; set; }

        /// <summary>
        /// 最低庫存警戒線
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "最低庫存不可為負數")]
        public decimal MinimumStock { get; set; }
    }

    /// <summary>
    /// 更新材料的資料傳輸物件
    /// </summary>
    public class UpdateMaterialDto
    {
        /// <summary>
        /// 材料名稱
        /// </summary>
        [MaxLength(100, ErrorMessage = "材料名稱不可超過100字")]
        public string? Name { get; set; }

        /// <summary>
        /// 材料描述
        /// </summary>
        [MaxLength(500, ErrorMessage = "材料描述不可超過500字")]
        public string? Description { get; set; }

        /// <summary>
        /// 材料分類
        /// </summary>
        public MaterialCategory? Category { get; set; }

        /// <summary>
        /// 標準單位
        /// </summary>
        [MaxLength(20, ErrorMessage = "單位不可超過20字")]
        public string? Unit { get; set; }

        /// <summary>
        /// 每單位成本
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "成本不可為負數")]
        public decimal? CostPerUnit { get; set; }

        /// <summary>
        /// 供應商
        /// </summary>
        [MaxLength(200, ErrorMessage = "供應商名稱不可超過200字")]
        public string? Supplier { get; set; }

        /// <summary>
        /// 庫存量
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "庫存量不可為負數")]
        public decimal? StockQuantity { get; set; }

        /// <summary>
        /// 最低庫存警戒線
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "最低庫存不可為負數")]
        public decimal? MinimumStock { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// 創建產品的資料傳輸物件
    /// </summary>
    public class CreateProductDto
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        [Required(ErrorMessage = "產品名稱為必填")]
        [MaxLength(100, ErrorMessage = "產品名稱不可超過100字")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 產品編號
        /// </summary>
        [Required(ErrorMessage = "產品編號為必填")]
        [MaxLength(50, ErrorMessage = "產品編號不可超過50字")]
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 產品描述
        /// </summary>
        [MaxLength(1000, ErrorMessage = "產品描述不可超過1000字")]
        public string? Description { get; set; }

        /// <summary>
        /// 產品分類
        /// </summary>
        public ProductCategory Category { get; set; }

        /// <summary>
        /// 標準產量
        /// </summary>
        [Range(0.001, double.MaxValue, ErrorMessage = "標準產量必須大於0")]
        public decimal StandardYield { get; set; }

        /// <summary>
        /// 產量單位
        /// </summary>
        [Required(ErrorMessage = "產量單位為必填")]
        [MaxLength(20, ErrorMessage = "產量單位不可超過20字")]
        public string YieldUnit { get; set; } = string.Empty;

        /// <summary>
        /// 預估製作時間（分鐘）
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "製作時間必須大於0")]
        public int EstimatedProductionTimeMinutes { get; set; }

        /// <summary>
        /// 標準售價
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "售價不可為負數")]
        public decimal StandardPrice { get; set; }
    }

    /// <summary>
    /// 更新產品的資料傳輸物件
    /// </summary>
    public class UpdateProductDto
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        [MaxLength(100, ErrorMessage = "產品名稱不可超過100字")]
        public string? Name { get; set; }

        /// <summary>
        /// 產品編號
        /// </summary>
        [MaxLength(50, ErrorMessage = "產品編號不可超過50字")]
        public string? ProductCode { get; set; }

        /// <summary>
        /// 產品描述
        /// </summary>
        [MaxLength(1000, ErrorMessage = "產品描述不可超過1000字")]
        public string? Description { get; set; }

        /// <summary>
        /// 產品分類
        /// </summary>
        public ProductCategory? Category { get; set; }

        /// <summary>
        /// 標準產量
        /// </summary>
        [Range(0.001, double.MaxValue, ErrorMessage = "標準產量必須大於0")]
        public decimal? StandardYield { get; set; }

        /// <summary>
        /// 產量單位
        /// </summary>
        [MaxLength(20, ErrorMessage = "產量單位不可超過20字")]
        public string? YieldUnit { get; set; }

        /// <summary>
        /// 預估製作時間（分鐘）
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "製作時間必須大於0")]
        public int? EstimatedProductionTimeMinutes { get; set; }

        /// <summary>
        /// 標準售價
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "售價不可為負數")]
        public decimal? StandardPrice { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// 創建配方的資料傳輸物件
    /// </summary>
    public class CreateRecipeDto
    {
        /// <summary>
        /// 配方名稱
        /// </summary>
        [Required(ErrorMessage = "配方名稱為必填")]
        [MaxLength(150, ErrorMessage = "配方名稱不可超過150字")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 配方版本
        /// </summary>
        [Required(ErrorMessage = "配方版本為必填")]
        [MaxLength(20, ErrorMessage = "配方版本不可超過20字")]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 配方描述
        /// </summary>
        [MaxLength(1000, ErrorMessage = "配方描述不可超過1000字")]
        public string? Description { get; set; }

        /// <summary>
        /// 關聯的產品 ID
        /// </summary>
        [Required(ErrorMessage = "產品ID為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "產品ID必須大於0")]
        public int ProductId { get; set; }

        /// <summary>
        /// 配方產量
        /// </summary>
        [Range(0.001, double.MaxValue, ErrorMessage = "配方產量必須大於0")]
        public decimal BatchYield { get; set; }

        /// <summary>
        /// 製作步驟說明
        /// </summary>
        [MaxLength(2000, ErrorMessage = "製作步驟不可超過2000字")]
        public string? Instructions { get; set; }

        /// <summary>
        /// 創建者
        /// </summary>
        [MaxLength(100, ErrorMessage = "創建者姓名不可超過100字")]
        public string? CreatedBy { get; set; }
    }

    /// <summary>
    /// 更新配方的資料傳輸物件
    /// </summary>
    public class UpdateRecipeDto
    {
        /// <summary>
        /// 配方名稱
        /// </summary>
        [MaxLength(150, ErrorMessage = "配方名稱不可超過150字")]
        public string? Name { get; set; }

        /// <summary>
        /// 配方版本
        /// </summary>
        [MaxLength(20, ErrorMessage = "配方版本不可超過20字")]
        public string? Version { get; set; }

        /// <summary>
        /// 配方描述
        /// </summary>
        [MaxLength(1000, ErrorMessage = "配方描述不可超過1000字")]
        public string? Description { get; set; }

        /// <summary>
        /// 配方產量
        /// </summary>
        [Range(0.001, double.MaxValue, ErrorMessage = "配方產量必須大於0")]
        public decimal? BatchYield { get; set; }

        /// <summary>
        /// 配方狀態
        /// </summary>
        public RecipeStatus? Status { get; set; }

        /// <summary>
        /// 是否為主要配方
        /// </summary>
        public bool? IsPrimary { get; set; }

        /// <summary>
        /// 製作步驟說明
        /// </summary>
        [MaxLength(2000, ErrorMessage = "製作步驟不可超過2000字")]
        public string? Instructions { get; set; }

        /// <summary>
        /// 審核者
        /// </summary>
        [MaxLength(100, ErrorMessage = "審核者姓名不可超過100字")]
        public string? ApprovedBy { get; set; }
    }

    /// <summary>
    /// 創建配方項目的資料傳輸物件
    /// </summary>
    public class CreateRecipeItemDto
    {
        /// <summary>
        /// 關聯的材料 ID
        /// </summary>
        [Required(ErrorMessage = "材料ID為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "材料ID必須大於0")]
        public int MaterialId { get; set; }

        /// <summary>
        /// 所需數量
        /// </summary>
        [Required(ErrorMessage = "數量為必填")]
        [Range(0.001, double.MaxValue, ErrorMessage = "數量必須大於0")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// 計量單位
        /// </summary>
        [Required(ErrorMessage = "單位為必填")]
        [MaxLength(20, ErrorMessage = "單位不可超過20字")]
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// 單位換算比例
        /// </summary>
        [Range(0.000001, double.MaxValue, ErrorMessage = "換算比例必須大於0")]
        public decimal ConversionRatio { get; set; } = 1.0m;

        /// <summary>
        /// 排序順序
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 材料類型
        /// </summary>
        public RecipeItemType ItemType { get; set; } = RecipeItemType.MainIngredient;

        /// <summary>
        /// 是否為可選材料
        /// </summary>
        public bool IsOptional { get; set; } = false;

        /// <summary>
        /// 備註說明
        /// </summary>
        [MaxLength(300, ErrorMessage = "備註不可超過300字")]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// 更新配方項目的資料傳輸物件
    /// </summary>
    public class UpdateRecipeItemDto
    {
        /// <summary>
        /// 所需數量
        /// </summary>
        [Range(0.001, double.MaxValue, ErrorMessage = "數量必須大於0")]
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 計量單位
        /// </summary>
        [MaxLength(20, ErrorMessage = "單位不可超過20字")]
        public string? Unit { get; set; }

        /// <summary>
        /// 單位換算比例
        /// </summary>
        [Range(0.000001, double.MaxValue, ErrorMessage = "換算比例必須大於0")]
        public decimal? ConversionRatio { get; set; }

        /// <summary>
        /// 排序順序
        /// </summary>
        public int? SortOrder { get; set; }

        /// <summary>
        /// 材料類型
        /// </summary>
        public RecipeItemType? ItemType { get; set; }

        /// <summary>
        /// 是否為可選材料
        /// </summary>
        public bool? IsOptional { get; set; }

        /// <summary>
        /// 備註說明
        /// </summary>
        [MaxLength(300, ErrorMessage = "備註不可超過300字")]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// 查詢條件的資料傳輸物件
    /// </summary>
    public class QueryDto
    {
        /// <summary>
        /// 關鍵字搜尋
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// 分類或狀態篩選
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "頁碼必須大於0")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每頁顯示數量
        /// </summary>
        [Range(1, 100, ErrorMessage = "每頁顯示數量必須在1-100之間")]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 是否只顯示啟用的項目
        /// </summary>
        public bool? OnlyActive { get; set; }
    }
}
