using ProxyTests.TestServices;
using SigmaMock;

namespace ProxyTests
{
    public class CheckServiceTests
    {
        private readonly Mocker<IProductService> _mockProductService = new();
        private readonly Mocker<ICustomLogger> _customLogger = new();


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

        [Fact]
        public async void ShouldNotValidProduct()
        {
            var product = new Product { Id = 0 };

            _mockProductService.SetupMethod(c => c.GetProduct(), product, 2);
            _mockProductService.SetupMethod(c => c.SaveProduct(default!), Task.FromResult(true), 0);

            // Act
            var result = await GetSut().IsValidProduct();

            _mockProductService.CheckMethodCalls();
            _customLogger.CheckMethodCalls();
            Assert.False(result);
        }
        private CheckService GetSut()
        {
            _customLogger.SetupMethod(c => c.Log(), 1);
            return new CheckService(_mockProductService.Implement(), _customLogger.Implement());
        }
    }
}