using ProxyTests.TestServices;
using SigmaMock;

namespace ProxyTests
{
    public class CheckServiceTests
    {
        [Fact]
        public async void ShouldValidProduct()
        {
            var product = new Product { Id = 22 };
            var mockProductService = new ProxyMock<IProductService>()
                .SetupMethod(c => c.GetProduct(), product, 1)
                .SetupMethod(c => c.SaveProduct(new Product()), Task.FromResult(true), 1)
                //.SetupMethodAsync(c => c.SaveProduct(new Product()), true, 1)
                .Create();

            var checkService = new CheckService(mockProductService);
            var result = await checkService.IsValidProduct();

            Assert.True(result);
        }
    }
}