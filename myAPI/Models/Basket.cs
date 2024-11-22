
namespace myAPI.Models
{
    public class Basket
    {
        public Dictionary<int, BasketItem> Items { get; set; } = new Dictionary<int, BasketItem>();

        public decimal CalculateTotal()
        {
            decimal total = 0m;
            foreach (var item in Items.Values)
            {
                total += item.Product.Price * item.Quantity;
            }
            return total;
        }
    }
}