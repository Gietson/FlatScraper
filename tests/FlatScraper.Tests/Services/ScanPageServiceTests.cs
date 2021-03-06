﻿using System.Threading.Tasks;
using AutoMapper;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.DTO;
using FlatScraper.Infrastructure.Services;
using Moq;
using Xunit;

namespace FlatScraper.Tests.Services
{
    public class ScanPageServiceTests
    {
        public ScanPageServiceTests()
        {
            _scanPageRepositoryMock = new Mock<IScanPageRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IScanPageRepository> _scanPageRepositoryMock;

        [Fact]
        public async Task add_async_should_invoke_add_async_on_repository()
        {
            // Arrange
            var scanPageService = new ScanPageService(_scanPageRepositoryMock.Object, _mapperMock.Object);
            ScanPageDto page = new ScanPageDto()
            {
                UrlAddress =
                    "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/page-2/v1c9073l3200008p2",
                Active = true,
                Host = "Gumtree",
                HostUrl = "https://www.gumtree.pl"
            };

            // Act
            await scanPageService.AddAsync(page);

            // Assert
            _scanPageRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ScanPage>()), Times.Once);
        }

        [Fact]
        public async Task get_all_async_should_invoke_get_all_async_on_repository()
        {
            // Arrange
            var scanPageService = new ScanPageService(_scanPageRepositoryMock.Object, _mapperMock.Object);

            // Act
            await scanPageService.GetAllAsync();

            // Assert
            _scanPageRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        /*
        [Fact]
        public async Task update_async_should_invoke_update_async_on_repository()
        {
            // Arrange
            var scanPageService = new ScanPageService(_scanPageRepositoryMock.Object, _mapperMock.Object);
            ScanPageDto page = new ScanPageDto() { Id = Guid.NewGuid(), UrlAddress = "https://www.gumtree.pl/s-mieszkania-i-domy-sprzedam-i-kupie/warszawa/page-2/v1c9073l3200008p2", Active = true, Page = "Gumtree" };

            // Act
            await scanPageService.UpdateAsync(page);


            // Assert
            _scanPageRepositoryMock.Verify(x=>x.UpdateAsync(It.IsAny<ScanPage>()), Times.Once);
        }*/
    }
}