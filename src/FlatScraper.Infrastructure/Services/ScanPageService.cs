using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
    public class ScanPageService : IScanPageService
    {
        private readonly IMapper _mapper;
        private readonly IScanPageRepository _scanPageRepository;

        public ScanPageService(IScanPageRepository scanPageRepository, IMapper mapper)
        {
            _scanPageRepository = scanPageRepository;
            _mapper = mapper;
        }

        public async Task<ScanPageDto> GetAsync(Guid id)
        {
            var page = await _scanPageRepository.GetAsync(id);
            if (page == null)
            {
                throw new Exception($"ScanPage with id='{id}' was not found.");
            }

            return _mapper.Map<ScanPageDto>(page);
        }

        public async Task<IEnumerable<ScanPageDto>> GetAllAsync()
        {
            var pages = await _scanPageRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ScanPageDto>>(pages);
        }

        public async Task AddAsync(ScanPageDto page)
        {
            var scanPage = ScanPage.Create(Guid.NewGuid(), page.UrlAddress, page.Page, page.Active);
            await _scanPageRepository.AddAsync(scanPage);
        }

        public async Task RemoveAsync(Guid id)
        {
            var page = await _scanPageRepository.GetAsync(id);
            if (page == null)
            {
                throw new Exception($"ScanPage with id='{id}' was not found.");
            }
            await _scanPageRepository.RemoveAsync(page);
        }

        public async Task UpdateAsync(ScanPageDto page)
        {
            var scanPage = await _scanPageRepository.GetAsync(page.Id);
            if (scanPage == null)
            {
                throw new Exception($"ScanPage with id='{page.Id}' was not found.");
            }

            await _scanPageRepository.UpdateAsync(_mapper.Map<ScanPage>(page));
        }
    }
}