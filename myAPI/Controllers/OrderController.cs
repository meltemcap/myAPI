using Microsoft.AspNetCore.Mvc;
using myAPI.Models;
using myAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace myAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService; // CustomerService eklendi

        public OrdersController(OrderService orderService, CustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService; // CustomerService kullanıldı
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var order = _orderService.CreateOrderFromBasket(request.Basket, request.Customer, request.PaymentMethod, request.Address);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            try
            {
                var orders = _orderService.GetAllOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromQuery] OrderStatus newStatus)
        {
            try
            {
                _orderService.UpdateOrderStatus(id, newStatus);
                return Ok($"Order status updated to {newStatus}.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Order not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("status/{status}")]
        public IActionResult GetOrdersByStatus(string status)
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
            {
                return BadRequest("Invalid order status");
            }

            try
            {
                var orders = _orderService.GetOrdersByStatus(parsedStatus);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("bydate")]
        public IActionResult GetOrdersByDate([FromQuery] bool ascending = true)
        {
            try
            {
                var orders = _orderService.GetOrdersByDate(ascending);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Siparişi olmayan müşterileri listeleyen yeni endpoint
        [HttpGet("customers/withoutorders")]
        public IActionResult GetCustomersWithoutOrders()
        {
            try
            {
                var orders = _orderService.GetAllOrders(); // Tüm siparişleri alıyoruz
                var customersWithoutOrders = _customerService.GetCustomersWithoutOrders(orders); // Siparişi olmayan müşterileri buluyoruz
                return Ok(customersWithoutOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class CreateOrderRequest
    {
        public Basket Basket { get; set; }
        public Customer Customer { get; set; }
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
    }
}