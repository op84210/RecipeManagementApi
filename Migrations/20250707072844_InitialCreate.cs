using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CostPerUnit = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Supplier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StockQuantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    MinimumStock = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProductCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    StandardYield = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    YieldUnit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EstimatedProductionTimeMinutes = table.Column<int>(type: "integer", nullable: false),
                    StandardPrice = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Version = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    BatchYield = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    Instructions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<int>(type: "integer", nullable: false),
                    MaterialId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ConversionRatio = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    IsOptional = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeItems_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeItems_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Category", "CostPerUnit", "CreatedDate", "Description", "IsActive", "LastUpdatedDate", "MinimumStock", "Name", "StockQuantity", "Supplier", "Unit" },
                values: new object[,]
                {
                    { 1, 0, 0.008m, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(4314), "蛋白質含量12%以上的麵粉", true, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(4827), 5000m, "高筋麵粉", 50000m, "統一麵粉公司", "公克" },
                    { 2, 0, 0.005m, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(5337), "細白砂糖", true, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(5337), 2000m, "砂糖", 20000m, "台糖公司", "公克" },
                    { 3, 0, 8.0m, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(5343), "新鮮雞蛋", true, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(5344), 50m, "雞蛋", 500m, "大成食品", "顆" },
                    { 4, 0, 0.02m, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(5632), "法國進口無鹽奶油", true, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(5633), 1000m, "無鹽奶油", 10000m, "法國乳品進口商", "公克" },
                    { 5, 0, 0.002m, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(5640), "精製食鹽", true, new DateTime(2025, 7, 7, 15, 28, 43, 98, DateTimeKind.Local).AddTicks(5640), 500m, "鹽", 5000m, "台鹽公司", "公克" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedDate", "Description", "EstimatedProductionTimeMinutes", "IsActive", "LastUpdatedDate", "Name", "ProductCode", "StandardPrice", "StandardYield", "YieldUnit" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2025, 7, 7, 15, 28, 43, 100, DateTimeKind.Local).AddTicks(4151), "經典白土司麵包，口感鬆軟香甜", 180, true, new DateTime(2025, 7, 7, 15, 28, 43, 100, DateTimeKind.Local).AddTicks(4748), "白土司麵包", "BREAD-001", 45.0m, 2m, "條" },
                    { 2, 0, new DateTime(2025, 7, 7, 15, 28, 43, 100, DateTimeKind.Local).AddTicks(5226), "香濃奶油小餐包，適合早餐", 150, true, new DateTime(2025, 7, 7, 15, 28, 43, 100, DateTimeKind.Local).AddTicks(5227), "奶油小餐包", "BREAD-002", 60.0m, 12m, "個" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Name",
                table: "Materials",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_MaterialId",
                table: "RecipeItems",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_RecipeId_MaterialId",
                table: "RecipeItems",
                columns: new[] { "RecipeId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProductId_Name_Version",
                table: "Recipes",
                columns: new[] { "ProductId", "Name", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeItems");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
