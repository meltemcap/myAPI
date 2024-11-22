using System.Collections.Generic;
using myAPI.Models;

namespace myAPI.Services
{
    public class BasketService
    {
        private readonly Dictionary<int, Basket> _baskets = new();

        public Basket GetBasket(int customerId) => _baskets.TryGetValue(customerId, out var basket) ? basket : new Basket();

        public bool AddProductToBasket(int customerId, BasketItem item)
        {
            var basket = GetBasket(customerId);
            if (basket.Items.ContainsKey(item.ProductId))
            {
                basket.Items[item.ProductId].Quantity += item.Quantity;
            }
            else
            {
                basket.Items[item.ProductId] = item;
            }
            _baskets[customerId] = basket;
            return true;
        }

        public bool Checkout(int customerId, CheckoutInfo info)
        {
            var basket = GetBasket(customerId);
            if (basket.Items.Count == 0)
                return false;

            
            _baskets.Remove(customerId);
            return true;
        }
    }
}