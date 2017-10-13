using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IEncrypter _encrypter;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

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

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task RemoveAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new Exception("Invalid parameter");
            }

            await _userRepository.RemoveAsync(id);
        }
    }
}