using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatScraper.Common.Mongo;
using FlatScraper.Core.Domain;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.Infrastructure.Services
{
	public class AdService : IAdService
	{
		private readonly IAdRepository _adRepository;
		private readonly IMapper _mapper;

		public AdService(IAdRepository adRepository, IMapper mapper)
		{
			_adRepository = adRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<AdDto>> GetAllAsync()
		{
			var ad = await _adRepository.GetAllAsync();

			return _mapper.Map<IEnumerable<AdDto>>(ad);
		}

	    public async Task<PagedResult<AdDto>> BrowseAsync()
	    {
	        var ad = await _adRepository.BrowseAsync();

	        return _mapper.Map<PagedResult<AdDto>>(ad);
	    }

	    public async Task<AdDto> GetAsync(Guid id)
		{
			var ad = await _adRepository.GetAsync(id);

			return _mapper.Map<AdDto>(ad);
		}

		public async Task AddAsync(AdDto adDto)
		{
			var ad = _mapper.Map<Ad>(adDto);

			await _adRepository.AddAsync(ad);
		}
	}
}