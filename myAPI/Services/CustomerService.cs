using System.Collections.Generic;
using System.Linq;
using myAPI.Models;

namespace myAPI.Services
{
    public class CustomerService
    {
        private readonly Dictionary<int, Customer> _customers = new();

        public IEnumerable<Customer> GetAllCustomers() => _customers.Values;

        public Customer GetCustomerById(int id) => _customers.TryGetValue(id, out var customer) ? customer : null;

        public void AddCustomer(Customer customer)
        {
            _customers[customer.Id] = customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            if (_customers.ContainsKey(customer.Id))
            {
                _customers[customer.Id] = customer;
            }
        }

        public void DeleteCustomer(int id) => _customers.Remove(id);

      
        public IEnumerable<Customer> GetCustomersWithoutOrders(IEnumerable<Order> allOrders)
        {
            // Sipariş vermiş olan müşteri ID'lerini buluyoruz
            var customersWithOrders = allOrders.Select(o => o.Customer.Id).Distinct();

            // Siparişi olmayan müşterileri filtreliyoruz
            var customersWithoutOrders = _customers.Values.Where(c => !customersWithOrders.Contains(c.Id));

            return customersWithoutOrders;
        }
    }
}
