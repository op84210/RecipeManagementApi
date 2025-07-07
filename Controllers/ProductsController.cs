using Microsoft.AspNetCore.Mvc;
using RecipeManagementApi.Models;
using RecipeManagementApi.Services;

namespace RecipeManagementApi.Controllers
{
    /// <summary>
    /// 產品管理控制器
    /// 提供產品的增刪改查功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="recipeService">配方服務</param>
        public ProductsController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        /// <summary>
        /// 取得產品清單
        /// </summary>
        /// <param name="search">搜尋關鍵字</param>
        /// <param name="category">產品類別篩選</param>
        /// <param name="page">頁數（從 1 開始）</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>產品清單</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            [FromQuery] string? search = null,
            [FromQuery] ProductCategory? category = null,
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

            var products = await _recipeService.GetProductsAsync(query);
            return Ok(products);
        }

        /// <summary>
        /// 根據 ID 取得產品詳細資料
        /// </summary>
        /// <param name="id">產品 ID</param>
        /// <returns>產品詳細資料</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _recipeService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"找不到 ID 為 {id} 的產品");

            return Ok(product);
        }

        /// <summary>
        /// 建立新產品
        /// </summary>
        /// <param name="dto">建立產品的資料</param>
        /// <returns>建立的產品</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Product), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var product = await _recipeService.CreateProductAsync(dto);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 更新產品資料
        /// </summary>
        /// <param name="id">產品 ID</param>
        /// <param name="dto">更新產品的資料</param>
        /// <returns>更新後的產品</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var product = await _recipeService.UpdateProductAsync(id, dto);
                if (product == null)
                    return NotFound($"找不到 ID 為 {id} 的產品");

                return Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 刪除產品
        /// </summary>
        /// <param name="id">產品 ID</param>
        /// <returns>刪除結果</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _recipeService.DeleteProductAsync(id);
                if (!result)
                    return NotFound($"找不到 ID 為 {id} 的產品");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 取得產品的配方清單
        /// </summary>
        /// <param name="id">產品 ID</param>
        /// <returns>產品的配方清單</returns>
        [HttpGet("{id}/recipes")]
        [ProducesResponseType(typeof(IEnumerable<Recipe>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetProductRecipes(int id)
        {
            var product = await _recipeService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"找不到 ID 為 {id} 的產品");

            var recipes = await _recipeService.GetRecipesByProductIdAsync(id);
            return Ok(recipes);
        }
    }
}
