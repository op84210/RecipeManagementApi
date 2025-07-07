using Microsoft.AspNetCore.Mvc;
using RecipeManagementApi.Models;
using RecipeManagementApi.Services;

namespace RecipeManagementApi.Controllers
{
    /// <summary>
    /// 配方管理控制器
    /// 提供配方的增刪改查功能與成本計算
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="recipeService">配方服務</param>
        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        /// <summary>
        /// 取得配方清單
        /// </summary>
        /// <param name="search">搜尋關鍵字</param>
        /// <param name="status">配方狀態篩選</param>
        /// <param name="productId">產品 ID 篩選</param>
        /// <param name="page">頁數（從 1 開始）</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>配方清單</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Recipe>), 200)]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes(
            [FromQuery] string? search = null,
            [FromQuery] RecipeStatus? status = null,
            [FromQuery] int? productId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (productId.HasValue)
            {
                var recipes = await _recipeService.GetRecipesByProductIdAsync(productId.Value);
                return Ok(recipes);
            }

            var query = new QueryDto
            {
                Search = search,
                Category = status?.ToString(),
                Page = page,
                PageSize = pageSize
            };

            var allRecipes = await _recipeService.GetRecipesAsync(query);
            return Ok(allRecipes);
        }

        /// <summary>
        /// 根據 ID 取得配方詳細資料
        /// </summary>
        /// <param name="id">配方 ID</param>
        /// <returns>配方詳細資料</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Recipe), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
                return NotFound($"找不到 ID 為 {id} 的配方");

            return Ok(recipe);
        }

        /// <summary>
        /// 建立新配方
        /// </summary>
        /// <param name="dto">建立配方的資料</param>
        /// <returns>建立的配方</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Recipe), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] CreateRecipeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var recipe = await _recipeService.CreateRecipeAsync(dto);
                return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 更新配方資料
        /// </summary>
        /// <param name="id">配方 ID</param>
        /// <param name="dto">更新配方的資料</param>
        /// <returns>更新後的配方</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Recipe), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Recipe>> UpdateRecipe(int id, [FromBody] UpdateRecipeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var recipe = await _recipeService.UpdateRecipeAsync(id, dto);
                if (recipe == null)
                    return NotFound($"找不到 ID 為 {id} 的配方");

                return Ok(recipe);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 刪除配方
        /// </summary>
        /// <param name="id">配方 ID</param>
        /// <returns>刪除結果</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try
            {
                var result = await _recipeService.DeleteRecipeAsync(id);
                if (!result)
                    return NotFound($"找不到 ID 為 {id} 的配方");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 設定為主要配方
        /// </summary>
        /// <param name="id">配方 ID</param>
        /// <returns>更新後的配方</returns>
        [HttpPost("{id}/set-primary")]
        [ProducesResponseType(typeof(Recipe), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Recipe>> SetPrimaryRecipe(int id)
        {
            try
            {
                var recipe = await _recipeService.SetPrimaryRecipeAsync(id);
                if (recipe == null)
                    return NotFound($"找不到 ID 為 {id} 的配方");

                return Ok(recipe);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 取得配方項目清單
        /// </summary>
        /// <param name="id">配方 ID</param>
        /// <returns>配方項目清單</returns>
        [HttpGet("{id}/items")]
        [ProducesResponseType(typeof(IEnumerable<RecipeItem>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<RecipeItem>>> GetRecipeItems(int id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
                return NotFound($"找不到 ID 為 {id} 的配方");

            var items = await _recipeService.GetRecipeItemsAsync(id);
            return Ok(items);
        }

        /// <summary>
        /// 新增配方項目
        /// </summary>
        /// <param name="id">配方 ID</param>
        /// <param name="dto">配方項目資料</param>
        /// <returns>新增的配方項目</returns>
        [HttpPost("{id}/items")]
        [ProducesResponseType(typeof(RecipeItem), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RecipeItem>> AddRecipeItem(int id, [FromBody] CreateRecipeItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _recipeService.AddRecipeItemAsync(id, dto);
                return CreatedAtAction("GetRecipeItem", "RecipeItems", new { id = item.Id }, item);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 計算配方成本
        /// </summary>
        /// <param name="id">配方 ID</param>
        /// <returns>配方總成本</returns>
        [HttpGet("{id}/cost")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<decimal>> CalculateRecipeCost(int id)
        {
            try
            {
                var cost = await _recipeService.CalculateRecipeCostAsync(id);
                return Ok(cost);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// 更新配方成本
        /// </summary>
        /// <param name="id">配方 ID</param>
        /// <returns>更新結果</returns>
        [HttpPost("{id}/update-cost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRecipeCost(int id)
        {
            try
            {
                await _recipeService.UpdateRecipeCostAsync(id);
                return Ok(new { message = "配方成本已更新" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
