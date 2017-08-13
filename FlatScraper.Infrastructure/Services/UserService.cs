using AutoMapper;
using FlatScraper.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using System.Security.Authentication;
using FlatScraper.Core.Domain;

namespace FlatScraper.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IEncrypter encrypter, IMapper mapper)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var user = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(user);
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if(user == null)
            {
                throw new AuthenticationException("Invalid credentials");
            }

            var hash = _encrypter.GetHash(password, user.Salt);
            if (user.Password == hash)
            {
                return;
            }

            throw new AuthenticationException("Invalid credentials");
        }

        public async Task RegisterAsync(Guid id, string email, string username, string password, string role)
        {
            var user = await _userRepository.GetAsync(email);
            if(user != null)
            {
                throw new Exception($"User with email: '{email}' already exists.");
            }

            var salt = _encrypter.GetSalt(password);
            var hash = _encrypter.GetHash(password, salt);
            user = new User(id, email, username, role, hash, salt);
            await _userRepository.AddAsync(user);
        }
    }
}
