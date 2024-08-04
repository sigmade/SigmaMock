using ProxyTests.TestServices;
using SigmaMock;

namespace ProxyTests
{
    public class CheckServiceTests
    {
        private readonly Mocker<IProductService> _mockProductService = new();
        private readonly Mocker<ICustomLogger> _customLogger = new();

        private CheckService GetSut()
        {
            _customLogger.SetupMethod(c => c.Log(), 1);
            return new CheckService(_mockProductService.CreateInstance(), _customLogger.CreateInstance());
        }

        [Fact]
        public async void ShouldValidProduct()
        {
            // Arrange
            var product = new Product { Id = 22 };

            _mockProductService.SetupMethod(c => c.GetProduct(), product, 2);
            _mockProductService.SetupMethod(c => c.SaveProduct(default!), Task.FromResult(true), 1);

            // Act
            var result = await GetSut().IsValidProduct();

            // Assert
            _mockProductService.Verify();
            _customLogger.Verify();
            Assert.True(result);
        }

        [Fact]
        public async void ShouldNotValidProduct()
        {
            // Arrange
            var product = new Product { Id = 0 };

            _mockProductService.SetupMethod(c => c.GetProduct(), product, 2);
            _mockProductService.SetupMethod(c => c.SaveProduct(default!), Task.FromResult(true), 0);

            // Act
            var result = await GetSut().IsValidProduct();

            // Assert
            _mockProductService.Verify();
            _customLogger.Verify();
            Assert.False(result);
        }
    }
}