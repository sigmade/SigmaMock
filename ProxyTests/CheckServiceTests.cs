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

            var mockIProductService = new Mocker<IProductService>()
                .SetupMethod(c => c.GetProduct(), product, 2)
                .SetupMethod(c => c.SaveProduct(default!), Task.FromResult(true), 1);

            var mockILogger = new Mocker<ICustomLogger>()
                .SetupMethod(c => c.Log(), 1);

            var checkService = new CheckService(mockIProductService.Implement(), mockILogger.Implement());

            var isValid = await checkService.IsValidProduct();

            mockIProductService.CheckMethodCalls();
            mockILogger.CheckMethodCalls();

            Assert.True(isValid);
        }
    }
}