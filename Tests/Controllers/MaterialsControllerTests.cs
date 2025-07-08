using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RecipeManagementApi.Controllers;
using RecipeManagementApi.Services;
using RecipeManagementApi.Models;
using Moq;
using System.Text.Json;

namespace RecipeManagementApi.Tests.Controllers
{
    /// <summary>
    /// MaterialsController 單元測試
    /// 測試材料控制器的各項 API 端點
    /// </summary>
    public class MaterialsControllerTests
    {
        private readonly Mock<IRecipeService> _mockRecipeService;
        private readonly MaterialsController _controller;

        public MaterialsControllerTests()
        {
            _mockRecipeService = new Mock<IRecipeService>();
            _controller = new MaterialsController(_mockRecipeService.Object);
        }

        #region GET /api/materials 測試

        [Fact]
        public async Task GetMaterials_應該返回材料列表()
        {
            // Arrange
            var expectedMaterials = new List<Material>
            {
                new Material { Id = 1, Name = "麵粉", Unit = "公克", CostPerUnit = 0.01m },
                new Material { Id = 2, Name = "砂糖", Unit = "公克", CostPerUnit = 0.02m }
            };

            _mockRecipeService
                .Setup(s => s.GetMaterialsAsync(It.IsAny<QueryDto>()))
                .ReturnsAsync(expectedMaterials);

            // Act
            var result = await _controller.GetMaterials();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var materials = Assert.IsAssignableFrom<IEnumerable<Material>>(okResult.Value);
            Assert.Equal(2, materials.Count());
        }

        [Fact]
        public async Task GetMaterials_服務拋出例外應該拋出例外()
        {
            // Arrange
            _mockRecipeService
                .Setup(s => s.GetMaterialsAsync(It.IsAny<QueryDto>()))
                .ThrowsAsync(new Exception("資料庫連線錯誤"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetMaterials());
            Assert.Equal("資料庫連線錯誤", exception.Message);
        }

        #endregion

        #region GET /api/materials/{id} 測試

        [Fact]
        public async Task GetMaterial_存在的ID應該返回材料()
        {
            // Arrange
            var expectedMaterial = new Material { Id = 1, Name = "麵粉", Unit = "公克" };
            _mockRecipeService
                .Setup(s => s.GetMaterialByIdAsync(1))
                .ReturnsAsync(expectedMaterial);

            // Act
            var result = await _controller.GetMaterial(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var material = Assert.IsType<Material>(okResult.Value);
            Assert.Equal("麵粉", material.Name);
        }

        [Fact]
        public async Task GetMaterial_不存在的ID應該返回404()
        {
            // Arrange
            _mockRecipeService
                .Setup(s => s.GetMaterialByIdAsync(999))
                .ReturnsAsync((Material)null);

            // Act
            var result = await _controller.GetMaterial(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        #endregion

        #region POST /api/materials 測試

        [Fact]
        public async Task CreateMaterial_有效資料應該返回201()
        {
            // Arrange
            var createDto = new CreateMaterialDto
            {
                Name = "奶油",
                Unit = "公克",
                CostPerUnit = 0.15m,
                Category = MaterialCategory.RawMaterial
            };

            var createdMaterial = new Material
            {
                Id = 1,
                Name = createDto.Name,
                Unit = createDto.Unit,
                CostPerUnit = createDto.CostPerUnit,
                Category = createDto.Category
            };

            _mockRecipeService
                .Setup(s => s.CreateMaterialAsync(It.IsAny<CreateMaterialDto>()))
                .ReturnsAsync(createdMaterial);

            // Act
            var result = await _controller.CreateMaterial(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);
            var material = Assert.IsType<Material>(createdResult.Value);
            Assert.Equal("奶油", material.Name);
        }

        [Theory]
        [InlineData("", "公克", 0.15)] // 空名稱
        [InlineData("奶油", "", 0.15)] // 空單位
        [InlineData("奶油", "公克", -0.15)] // 負成本
        public async Task CreateMaterial_無效資料應該返回400(string name, string unit, decimal cost)
        {
            // Arrange
            var createDto = new CreateMaterialDto
            {
                Name = name,
                Unit = unit,
                CostPerUnit = cost,
                Category = MaterialCategory.RawMaterial
            };

            // 模擬 ModelState 驗證錯誤
            if (string.IsNullOrEmpty(name))
                _controller.ModelState.AddModelError("Name", "名稱為必填");
            if (string.IsNullOrEmpty(unit))
                _controller.ModelState.AddModelError("Unit", "單位為必填");
            if (cost < 0)
                _controller.ModelState.AddModelError("CostPerUnit", "成本不可為負數");

            // Act
            var result = await _controller.CreateMaterial(createDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        #endregion

        #region PUT /api/materials/{id} 測試

        [Fact]
        public async Task UpdateMaterial_存在的ID應該返回更新的材料()
        {
            // Arrange
            var updateDto = new UpdateMaterialDto
            {
                Name = "高筋麵粉",
                CostPerUnit = 0.012m
            };

            var updatedMaterial = new Material
            {
                Id = 1,
                Name = "高筋麵粉",
                Unit = "公克",
                CostPerUnit = 0.012m
            };

            _mockRecipeService
                .Setup(s => s.UpdateMaterialAsync(1, It.IsAny<UpdateMaterialDto>()))
                .ReturnsAsync(updatedMaterial);

            // Act
            var result = await _controller.UpdateMaterial(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var material = Assert.IsType<Material>(okResult.Value);
            Assert.Equal("高筋麵粉", material.Name);
            Assert.Equal(0.012m, material.CostPerUnit);
        }

        [Fact]
        public async Task UpdateMaterial_不存在的ID應該返回404()
        {
            // Arrange
            var updateDto = new UpdateMaterialDto { Name = "測試" };
            
            _mockRecipeService
                .Setup(s => s.UpdateMaterialAsync(999, It.IsAny<UpdateMaterialDto>()))
                .ReturnsAsync((Material)null);

            // Act
            var result = await _controller.UpdateMaterial(999, updateDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        #endregion

        #region DELETE /api/materials/{id} 測試

        [Fact]
        public async Task DeleteMaterial_存在的ID應該返回204()
        {
            // Arrange
            _mockRecipeService
                .Setup(s => s.DeleteMaterialAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteMaterial(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteMaterial_不存在的ID應該返回404()
        {
            // Arrange
            _mockRecipeService
                .Setup(s => s.DeleteMaterialAsync(999))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteMaterial(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region 驗證 Mock 呼叫

        [Fact]
        public async Task GetMaterials_應該使用正確的查詢參數()
        {
            // Arrange
            var query = new QueryDto { Page = 2, PageSize = 5, Search = "麵" };
            
            _mockRecipeService
                .Setup(s => s.GetMaterialsAsync(It.IsAny<QueryDto>()))
                .ReturnsAsync(new List<Material>());

            // Act
            await _controller.GetMaterials(page: 2, pageSize: 5, search: "麵");

            // Assert
            _mockRecipeService.Verify(
                s => s.GetMaterialsAsync(It.Is<QueryDto>(q => 
                    q.Page == 2 && 
                    q.PageSize == 5 && 
                    q.Search == "麵")), 
                Times.Once);
        }

        #endregion
    }
}
