namespace ProxyTests.TestServices
{
    public interface IProductService
    {
        Product GetProduct();
        Task<bool> SaveProduct(Product product);
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsValid { get; set; }
    }
}