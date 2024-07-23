namespace ProxyTests.TestServices
{
    public class MainService
    {
        private IProductService _productService;

        public MainService(IProductService productService)
        {
            _productService = productService;
        }

        public bool IsValidProduct()
        {
            var product = _productService.GetProduct();

            if (product.Id > 0)
            {
                return true;
            }

            return false;
        }
    }
}
