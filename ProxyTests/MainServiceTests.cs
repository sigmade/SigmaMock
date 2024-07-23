using ProxyTests.TestServices;
using SigmaMock;

namespace ProxyTests
{
    public class MainServiceTests
    {
        [Fact]
        public void ShouldValidProduct()
        {
            var mockProduct = new Product { Id = 22 };

            var mockService = new ProxyMock<IProductService>()
                .SetupReturnValue(nameof(IProductService.GetProduct), mockProduct)
                .Create();

            var mainService = new MainService(mockService);
            var result = mainService.IsValidProduct();

            Assert.True(result);
        }
    }
}