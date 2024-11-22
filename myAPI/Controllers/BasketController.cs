using Microsoft.AspNetCore.Mvc;
using myAPI.Models;
using myAPI.Services;

namespace myAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly BasketService _basketService;

        public BasketController(BasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{customerId}")]
        public ActionResult<Basket> GetBasket(int customerId)
        {
            var basket = _basketService.GetBasket(customerId);
            return Ok(basket);
        }
        [HttpPost("{customerId}/add")]
        public IActionResult AddProductToBasket(int customerId, [FromBody] BasketItem item)
        {
            if (item == null || item.Product == null || item.ProductId <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            var result = _basketService.AddProductToBasket(customerId, item);

            if (!result)
            {
                return BadRequest("Could not add product to basket.");
            }

            return Ok("Product added to basket.");
        }



        [HttpPost("{customerId}/checkout")]
        public IActionResult Checkout(int customerId, [FromBody] CheckoutInfo info)
        {
            if (info == null)
            {
                return BadRequest("Checkout info is required.");
            }

            var result = _basketService.Checkout(customerId, info);
            if (!result)
            {
                return BadRequest("Checkout failed.");
            }

            return Ok("Checkout successful.");
        }
    }
}