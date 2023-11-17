using MauiWithAPI.API.Services.Products;
using MauiWithAPI.API.Services.Products.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MauiWithAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? keyword = null)
        {
            var products = await _productService.GetProducts(page, pageSize, keyword);
            return StatusCode(products.StatusCode, products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);

            return StatusCode(product.StatusCode, product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromForm] CreateProductDto createProductDto)
        {
            var product = await _productService.AddProduct(createProductDto);
            return StatusCode(product.StatusCode, product);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromForm] UpdateProductDto productDto)
        {
            var product = await _productService.UpdateProduct(productDto);
            return StatusCode(product.StatusCode, product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var response = await _productService.DeleteProduct(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
