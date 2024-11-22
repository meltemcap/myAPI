namespace myAPI.Models
{
    public class BasketItem
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public Product Product { get; set; }
    }
}