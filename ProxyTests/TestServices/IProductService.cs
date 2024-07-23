namespace ProxyTests.TestServices
{
    public interface IProductService
    {
        Product GetProduct();
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}