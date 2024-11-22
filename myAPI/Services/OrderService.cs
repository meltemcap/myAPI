using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using myAPI.Models;

namespace myAPI.Services
{
    public class OrderService
    {
        private readonly ConcurrentDictionary<int, Order> _orders = new ConcurrentDictionary<int, Order>();
        private int _nextOrderId = 1;

        public Order CreateOrderFromBasket(Basket basket, Customer customer, string paymentMethod, string address)
        {
            var orderId = _nextOrderId++;
            var order = new Order
            {
                Id = orderId,
                Customer = customer,
                OrderDate = DateTime.UtcNow,
                PaymentMethod = paymentMethod,
                Address = address,
                Items = new Dictionary<int, BasketItem>(basket.Items),
                Status = OrderStatus.Pending // Yeni siparişin başlangıç durumu
            };

            _orders.TryAdd(orderId, order);
            return order;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orders.Values;
        }

        public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            if (_orders.TryGetValue(orderId, out var order))
            {
                order.Status = newStatus;
            }
            else
            {
                throw new KeyNotFoundException("Order not found.");
            }
        }
        public bool HasCustomerOrders(int customerId)
        {
            return _orders.Values.Any(order => order.Customer.Id == customerId);
        }
        public IEnumerable<Order> GetOrdersByStatus(OrderStatus status)
        {
            return _orders.Values.Where(order => order.Status == status);
        }
        public IEnumerable<Order> GetOrdersByDate(bool ascending = true)
        {
            return ascending
                ? _orders.Values.OrderBy(order => order.OrderDate)
                : _orders.Values.OrderByDescending(order => order.OrderDate);
        }
    }
}