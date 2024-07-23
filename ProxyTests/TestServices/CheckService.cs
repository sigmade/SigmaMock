namespace ProxyTests.TestServices
{
    public class CheckService
    {
        private IProductService _productService;

        public CheckService(IProductService productService)
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
