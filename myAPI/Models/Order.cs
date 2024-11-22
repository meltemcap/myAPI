using System;
using System.Collections.Generic;
using System.Linq;

namespace myAPI.Models
{
    public enum OrderStatus
    {
        Pending,    // işlenmemiş
        Completed,  // Sipariş tamamlanmış
        Canceled    // Sipariş iptal edilmiş
    }

    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public Dictionary<int, BasketItem> Items { get; set; } = new Dictionary<int, BasketItem>();
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
        public OrderStatus Status { get; set; } // Sipariş durumu

        public decimal TotalPrice
        {
            get
            {
                return Items.Values.Sum(item => item.Product.Price * item.Quantity);
            }
        }
    }
}
