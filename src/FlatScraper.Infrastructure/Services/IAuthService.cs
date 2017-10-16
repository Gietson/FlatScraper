using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
    public interface IAuthService : IService
    {
        Task LoginAsync(LoginUserDto user);
        Task RegisterAsync(CreateUserDto user);
    }
}