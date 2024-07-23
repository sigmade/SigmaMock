namespace ProxyTests.TestServices
{
    public class ProductService : IProductService
    {
        public Product GetProduct()
        {
            return new Product();
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}