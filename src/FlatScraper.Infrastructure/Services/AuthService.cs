using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEncrypter _encrypter;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository, IEncrypter encrypter, IMapper mapper)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
            _mapper = mapper;
        }

        public async Task LoginAsync(LoginUserDto newUser)
        {
            if (newUser?.Password == null || newUser.Email == null)
            {
                throw new AuthenticationException("Invalid credentials");
            }

            var user = await _userRepository.GetAsync(newUser.Email);
            if (user == null)
            {
                throw new AuthenticationException("Invalid credentials");
            }

            var hash = _encrypter.GetHash(newUser.Password, user.Salt);
            if (user.Password == hash)
            {
                return;
            }

            throw new AuthenticationException("Invalid credentials");
        }

        public async Task RegisterAsync(CreateUserDto newUser)
        {
            var user = await _userRepository.GetAsync(newUser.Email);
            if (user != null)
            {
                throw new Exception($"User with email: '{newUser.Email}' already exists.");
            }

            Guid id = Guid.NewGuid();
            string role = "user";
            var salt = _encrypter.GetSalt(newUser.Password);
            var hash = _encrypter.GetHash(newUser.Password, salt);
            user = new User(id, newUser.Email, newUser.Username, role, hash, salt);
            await _userRepository.AddAsync(user);
        }
    }
}