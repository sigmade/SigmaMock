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
            var product2 = new Product { Id = 0 };

            var mockIProductService = new Mocker<IProductService>()
                .SetupMethod(c => c.GetProduct(), product, 1)
                .SetupMethod(c => c.SaveProduct(default!), Task.FromResult(true), 1);

            var mockIProductService2 = new Mocker<IProductService>()
                .SetupMethod(c => c.GetProduct(), product2, 0)
                .SetupMethod(c => c.GetProduct(), product2, 1)
                .SetupMethod(c => c.SaveProduct(default!), Task.FromResult(true), 0);

            //var mockILogger = new Mocker<ICustomLogger>()
            //    .SetupMethod(c => c.Log(), null, 1);

            var checkService = new CheckService(mockIProductService.Object);
            var isValid = await checkService.IsValidProduct();

            var checkService2 = new CheckService(mockIProductService2.Object);
            var isValid2 = await checkService2.IsValidProduct();

            mockIProductService.CheckMethodCalls();
            mockIProductService2.CheckMethodCalls();
            // mockILogger.CheckMethodCalls();

            Assert.True(isValid);
        }
    }
}