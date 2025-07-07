using Microsoft.AspNetCore.Mvc;
using RecipeManagementApi.Models;
using RecipeManagementApi.Services;

namespace RecipeManagementApi.Controllers
{
    /// <summary>
    /// 配方項目管理控制器
    /// 提供配方項目的增刪改查功能
    /// </summary>
    [ApiController]
    [Route("api/recipe-items")]
    [Produces("application/json")]
    public class RecipeItemsController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="recipeService">配方服務</param>
        public RecipeItemsController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        /// <summary>
        /// 根據 ID 取得配方項目詳細資料
        /// </summary>
        /// <param name="id">配方項目 ID</param>
        /// <returns>配方項目詳細資料</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RecipeItem), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RecipeItem>> GetRecipeItem(int id)
        {
            var item = await _recipeService.GetRecipeItemByIdAsync(id);
            if (item == null)
                return NotFound($"找不到 ID 為 {id} 的配方項目");

            return Ok(item);
        }

        /// <summary>
        /// 更新配方項目資料
        /// </summary>
        /// <param name="id">配方項目 ID</param>
        /// <param name="dto">更新配方項目的資料</param>
        /// <returns>更新後的配方項目</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RecipeItem), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RecipeItem>> UpdateRecipeItem(int id, [FromBody] UpdateRecipeItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _recipeService.UpdateRecipeItemAsync(id, dto);
                if (item == null)
                    return NotFound($"找不到 ID 為 {id} 的配方項目");

                return Ok(item);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 刪除配方項目
        /// </summary>
        /// <param name="id">配方項目 ID</param>
        /// <returns>刪除結果</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteRecipeItem(int id)
        {
            try
            {
                var result = await _recipeService.DeleteRecipeItemAsync(id);
                if (!result)
                    return NotFound($"找不到 ID 為 {id} 的配方項目");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
