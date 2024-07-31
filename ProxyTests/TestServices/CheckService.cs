namespace ProxyTests.TestServices
{
    public class CheckService
    {
        private IProductService _productService;

        public CheckService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<bool> IsValidProduct()
        {
            var product = _productService.GetProduct();
            _ = _productService.GetProduct();

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
