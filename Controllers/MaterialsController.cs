using Microsoft.AspNetCore.Mvc;
using RecipeManagementApi.Models;
using RecipeManagementApi.Services;

namespace RecipeManagementApi.Controllers
{
    /// <summary>
    /// 材料管理控制器
    /// 提供材料的增刪改查功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MaterialsController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="recipeService">配方服務</param>
        public MaterialsController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        /// <summary>
        /// 取得材料清單
        /// </summary>
        /// <param name="search">搜尋關鍵字</param>
        /// <param name="category">材料類別篩選</param>
        /// <param name="page">頁數（從 1 開始）</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>材料清單</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Material>), 200)]
        public async Task<ActionResult<IEnumerable<Material>>> GetMaterials(
            [FromQuery] string? search = null,
            [FromQuery] MaterialCategory? category = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new QueryDto
            {
                Search = search,
                Category = category?.ToString(),
                Page = page,
                PageSize = pageSize
            };

            var materials = await _recipeService.GetMaterialsAsync(query);
            return Ok(materials);
        }

        /// <summary>
        /// 根據 ID 取得材料詳細資料
        /// </summary>
        /// <param name="id">材料 ID</param>
        /// <returns>材料詳細資料</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Material), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            var material = await _recipeService.GetMaterialByIdAsync(id);
            if (material == null)
                return NotFound($"找不到 ID 為 {id} 的材料");

            return Ok(material);
        }

        /// <summary>
        /// 建立新材料
        /// </summary>
        /// <param name="dto">建立材料的資料</param>
        /// <returns>建立的材料</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Material), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Material>> CreateMaterial([FromBody] CreateMaterialDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var material = await _recipeService.CreateMaterialAsync(dto);
                return CreatedAtAction(nameof(GetMaterial), new { id = material.Id }, material);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 更新材料資料
        /// </summary>
        /// <param name="id">材料 ID</param>
        /// <param name="dto">更新材料的資料</param>
        /// <returns>更新後的材料</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Material), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Material>> UpdateMaterial(int id, [FromBody] UpdateMaterialDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var material = await _recipeService.UpdateMaterialAsync(id, dto);
                if (material == null)
                    return NotFound($"找不到 ID 為 {id} 的材料");

                return Ok(material);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 刪除材料
        /// </summary>
        /// <param name="id">材料 ID</param>
        /// <returns>刪除結果</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            try
            {
                var result = await _recipeService.DeleteMaterialAsync(id);
                if (!result)
                    return NotFound($"找不到 ID 為 {id} 的材料");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
