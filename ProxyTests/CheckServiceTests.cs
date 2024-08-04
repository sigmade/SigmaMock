using ProxyTests.TestServices;
using SigmaMock;

namespace ProxyTests
{
    public class CheckServiceTests
    {
        [Fact]
        public async void ShouldValidProduct()
        {
            // Arrange
            var product = new Product { Id = 22 };
            var mockProductService = new Mocker<IProductService>()
                .SetupMethod(c => c.GetProduct(), product, 2)
                .SetupMethod(c => c.SaveProduct(default!), Task.FromResult(true), 1);

            var checkService = new CheckService(mockProductService.CreateInstance());

            // Act
            var result = await checkService.IsValidProduct();

            // Assert
            mockProductService.Verify();
            Assert.True(result);
        }
    }
}