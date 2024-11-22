
using myAPI.Models;
using static myAPI.Models.Product;

namespace myAPI.Services
{
    public class ProductService
    {
        private readonly Dictionary<int, Product> _products = new();

        public ProductService()
        {
            // Örnek veri
            _products.Add(1, new Product { Id = 1, Name = "Bisküvi", Price = 20.0m, Quantity = 100, Barcode = "PB-A-0001", Unit= UnitType.Piece });
            _products.Add(2, new Product { Id = 2, Name = "Su", Price = 5.25m, Quantity = 200, Barcode = "PB-A-0002", Unit= UnitType.Piece });
            _products.Add(3, new Product { Id = 3, Name = "Tavuk", Price = 121.0m, Quantity = 300, Barcode = "PB-K-00003", Unit = UnitType.Kilo });
            _products.Add(4, new Product { Id = 4, Name = "Glütensiz Ekmek", Price = 45.0m, Quantity = 400, Barcode = "PK-A-0004", Unit = UnitType.Piece });
        }

        public IEnumerable<Product> GetAllProducts() => _products.Values;

        public Product GetProductById(int id) => _products.TryGetValue(id, out var product) ? product : null;

        public void AddProduct(Product product) => _products[product.Id] = product;

        public void UpdateProduct(Product product)
        {
            if (_products.ContainsKey(product.Id))
            {
                _products[product.Id] = product;
            }
        }

        public void DeleteProduct(int id) => _products.Remove(id);
        public void ApplyPriceIncrease(double percentage)
        {
            foreach (var product in _products.Values)
            {
                // Ürünlerin fiyatlarına zam oranını ekliyoruz
                product.Price += product.Price * (decimal)(percentage / 100);
            }
        }
        public IEnumerable<Product> GetProductsByUnitType(UnitType unitType)
        {
            return _products.Values.Where(p => p.Unit == unitType);
        }

    }
}
