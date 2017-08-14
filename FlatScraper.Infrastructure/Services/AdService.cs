using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using AutoMapper;
using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services.Scrapers;
using FlatScraper.Infrastructure.Extensions;
using HtmlAgilityPack;
using FlatScraper.Core.Domain;

namespace FlatScraper.Infrastructure.Services
{
    public class AdService : IAdService
    {
        private readonly IAdRepository _adRepository;
        private readonly IScraper _scraper;
        private readonly IMapper _mapper;

        public AdService(IAdRepository adRepository, IScraper scraper, IMapper mapper)
        {
            _adRepository = adRepository;
            _scraper = scraper;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AdDto>> GetAllAsync()
        {
            var ad = await _adRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<AdDto>>(ad);
        }

        public async Task<AdDto> GetAsync(Guid id)
        {
            var ad = await _adRepository.GetAsync(id);

            return _mapper.Map<AdDto>(ad);
        }

        public async Task AddAsync(string url)
        {
            HtmlDocument scrapedDoc = await ScrapExtensions.ScrapUrl(url);

            List<Ad> ads = _scraper.ParseHomePage(scrapedDoc);

            foreach (var ad in ads)
            {
                HtmlDocument scrapedSubPage = await ScrapExtensions.ScrapUrl(ad.Url);
                ad.AdDetails = _scraper.ParseDetailsPage(scrapedSubPage);
            }
            
        }
    }
}
