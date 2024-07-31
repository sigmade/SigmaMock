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
            var mockProductService = new Mocker<IProductService>()
                .SetupMethod(c => c.GetProduct(), product, 2)
                .SetupMethod(c => c.SaveProduct(default), Task.FromResult(true), 1);

            var checkService = new CheckService(mockProductService.CreateInstance());


            var result = await checkService.IsValidProduct();

            mockProductService.Verify();
            Assert.True(result);
        }
    }
}