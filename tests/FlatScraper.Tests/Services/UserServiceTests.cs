using System;
using System.Threading.Tasks;
using AutoMapper;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.DTO;
using FlatScraper.Infrastructure.Services;
using Moq;
using Xunit;

namespace FlatScraper.Tests.Services
{
	public class UserServiceTests
	{
		private readonly Mock<IEncrypter> _encrypterMock;
		private readonly Mock<IMapper> _mapperMock;
		private readonly Mock<IUserRepository> _userRepositoryMock;

		public UserServiceTests()
		{
			_userRepositoryMock = new Mock<IUserRepository>();
			_encrypterMock = new Mock<IEncrypter>();
			_mapperMock = new Mock<IMapper>();
		}

		[Fact]
		public async Task register_async_should_invoke_add_async_on_repository()
		{
			_encrypterMock.Setup(x => x.GetSalt(It.IsAny<string>())).Returns("hash");
			_encrypterMock.Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<string>())).Returns("salt");
			var authService = new AuthService(_userRepositoryMock.Object, _encrypterMock.Object, _mapperMock.Object);
		    CreateUserDto user = new CreateUserDto()
		    {
		        Email = "user@email.com",
		        Username = "user1",
		        Password = "secret",
		        Role = "user"
		    };
			await authService.RegisterAsync(user);

			_userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
		}
	}
}