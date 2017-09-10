using System.Threading.Tasks;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services;
using Moq;
using Xunit;

namespace FlatScraper.Tests.Services
{
	public class ScraperServiceTests
	{
		private readonly Mock<IAdRepository> _adRepository;
		private readonly Mock<IScanPageService> _scanPageServiceMock;

		public ScraperServiceTests()
		{
			_scanPageServiceMock = new Mock<IScanPageService>();
			_adRepository = new Mock<IAdRepository>();
		}

		[Fact]
		public async Task add_async_should_invoke_add_async_on_repository()
		{
			// Arrange
			var scrapService = new ScraperService(_scanPageServiceMock.Object, _adRepository.Object);

			// Act
			await scrapService.ScrapAsync();

			// Assert
			_scanPageServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
		}
	}
}