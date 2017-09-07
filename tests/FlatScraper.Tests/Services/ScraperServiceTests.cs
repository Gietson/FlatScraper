using System.Threading.Tasks;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlatScraper.Tests.Services
{
    public class ScraperServiceTests
    {
        private readonly Mock<IAdRepository> _adRepository;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IScanPageService> _scanPageServiceMock;

        public ScraperServiceTests()
        {
            _scanPageServiceMock = new Mock<IScanPageService>();
            _adRepository = new Mock<IAdRepository>();
            _logger = new Mock<ILogger>();
        }

        [Fact]
        public async Task add_async_should_invoke_add_async_on_repository()
        {
            // Arrange
            var scrapService = new ScraperService(_scanPageServiceMock.Object, _adRepository.Object);

            // Act
            await scrapService.ScrapAsync(_logger.Object);

            // Assert
            _scanPageServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }
}