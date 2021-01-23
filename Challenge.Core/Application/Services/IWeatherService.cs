using Challenge.Core.Application.DTOs;
using System.Threading.Tasks;

namespace Challenge.Core.Application.Services
{
    public interface IWeatherService
    {
        Task<WeatherDto> GetWeatherByLocation(double latitude, double longitude);

    }
}
