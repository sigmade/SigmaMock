namespace ProxyTests.TestServices
{
    public class CheckService(
        IProductService productService,
        ICustomLogger customLogger)
    {
        private readonly IProductService _productService = productService;
        private readonly ICustomLogger _customLogger = customLogger;

        public async Task<bool> IsValidProduct()
        {
            var product = _productService.GetProduct();
            _ = _productService.GetProduct();

            _customLogger.Log();

            if (product.Id > 0)
            {
                product.IsValid = true;

                var res = await _productService.SaveProduct(product);

                if (res)
                {
                    if (product.Id > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }
    }
}
