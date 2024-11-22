using Microsoft.AspNetCore.Mvc;
using myAPI.Models;
using myAPI.Services;

namespace myAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                return product == null ? NotFound() : Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult PostProduct([FromBody] Product product)
        {
            try
            {
                _productService.AddProduct(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest();

            try
            {
                var existingProduct = _productService.GetProductById(id);
                if (existingProduct == null)
                    return NotFound();

                _productService.UpdateProduct(product);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var existingProduct = _productService.GetProductById(id);
                if (existingProduct == null)
                    return NotFound();

                _productService.DeleteProduct(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //  zam yapma işlemi
        [HttpPut("apply-price-increase/{percentage}")]
        public IActionResult ApplyPriceIncrease(double percentage)
        {
            try
            {
                _productService.ApplyPriceIncrease(percentage);
                return Ok($"Tüm ürünlere %{percentage} zam yapıldı.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("unit/{unitType}")]
        public IActionResult GetProductsByUnitType(string unitType)
        {
            if (!Enum.TryParse<UnitType>(unitType, true, out var parsedUnitType))
            {
                return BadRequest("Invalid unit type");
            }

            try
            {
                var products = _productService.GetProductsByUnitType(parsedUnitType);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
