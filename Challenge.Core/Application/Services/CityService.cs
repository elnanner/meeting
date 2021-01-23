using Challenge.Core.Application.DTOs;
using Challenge.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Core.Application.Services
{
    public class CityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public async Task<IEnumerable<CityDto>> GetAll()
        {
            var cities = await _cityRepository.GetAll();

            return cities.Select(c => new CityDto
            {
                CityId = c.CityId,
                Name = c.Name,
                Latitude = c.Latitude,
                Longitude = c.Longitude
            }).AsEnumerable<CityDto>();
        }
    }
}
