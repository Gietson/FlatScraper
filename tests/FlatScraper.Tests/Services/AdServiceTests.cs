using System.Threading.Tasks;
using AutoMapper;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services;
using FlatScraper.Infrastructure.Services.Scrapers;
using Moq;
using Xunit;

namespace FlatScraper.Tests.Services
{
    public class AdServiceTests
    {
        private readonly Mock<IAdRepository> _adRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IScraper> _scraperMock;

        public AdServiceTests()
        {
            _adRepositoryMock = new Mock<IAdRepository>();
            _mapperMock = new Mock<IMapper>();
            _scraperMock = new Mock<IScraper>();
        }

        /*[Fact]
        public async Task add_async_should_invoke_add_async_on_repository()
        {
            // Arrange
            var adService = new AdService(_adRepositoryMock.Object, _scraperMock.Object, _mapperMock.Object);
            ScanPageDto page = new ScanPageDto() { UrlAddress = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/page-2/v1c9073l3200008p2", Active = true, Page = "Gumtree" };

            // Act
            await adService.AddAsync();

            // Assert
            _scanPageRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ScanPage>()), Times.Once);
        }*/

        [Fact]
        public async Task get_all_async_should_invoke_get_all_async_on_repository()
        {
            // Arrange
            var adService = new AdService(_adRepositoryMock.Object, _scraperMock.Object, _mapperMock.Object);

            // Act
            await adService.GetAllAsync();

            // Assert
            _adRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        /*
        [Fact]
        public async Task update_async_should_invoke_update_async_on_repository()
        {
            // Arrange
            var adService = new AdService(_adRepositoryMock.Object, _scraperMock.Object, _mapperMock.Object);
            AdDto ad = new AdDto() { UrlAddress = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/page-2/v1c9073l3200008p2", Active = true, Page = "Gumtree" };

            // Act
            await adService.UpdateAsync(page);


            // Assert
            _scanPageRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ScanPage>()), Times.Once);
        }*/
    }
}