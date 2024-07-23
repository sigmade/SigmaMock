using ProxyTests.TestServices;
using SigmaMock;

namespace ProxyTests
{
    public class CheckServiceTests
    {
        [Fact]
        public void ShouldValidProduct()
        {
            var product = new Product { Id = 22 };

            var mockProductService = new ProxyMock<IProductService>()
                .SetupReturnValue(nameof(IProductService.GetProduct), product)
                .Create();

            var checkService = new CheckService(mockProductService);
            var result = checkService.IsValidProduct();

            Assert.True(result);
        }
    }
}