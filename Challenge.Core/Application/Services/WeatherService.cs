using Challenge.Core.Application.DTOs;
using Challenge.Core.Domain.Entities.Weather;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Challenge.Core.Application.Services
{
    public class WeatherService : IWeatherService
    {
        public async Task<WeatherDto> GetWeatherByLocation(double latitude, double longitude)
        {
            return await Get(latitude, longitude);
        }


        public async Task<WeatherDto> Get(double latitude, double longitude)
        {
            Weather weather;
                var client = new HttpClient();
            //string url = "https://dark-sky.p.rapidapi.com/-34.9214,-57.9544?lang=es&units=auto";

            var formmatedLatitude = latitude.ToString().Replace(',', '.');
            var formmatedLongitude = longitude.ToString().Replace(',', '.');

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://dark-sky.p.rapidapi.com/" + formmatedLatitude + "," + formmatedLongitude + "?lang=es&units=auto"),
                // RequestUri = new Uri(url),
                Headers =
                {
                    { "x-rapidapi-key", "d0d898501bmshba4194ec7493dedp154ee3jsn0911cdb1f55e" },
                    { "x-rapidapi-host", "dark-sky.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                weather = JsonConvert.DeserializeObject<Weather>(body); ;
            }

            // las propiedades en minuscula son porque al obtner el modelo de la api de clima,
            // use una app para mapear el json a c#  y me devilvió eso,
            // eran muchas propiedades y preferí no gastar tiempo en cambiar por mayusculas.


            var weatherDto = new WeatherDto
            {
                Latitude = weather.latitude,
                Longitude = weather.longitude,
                Currently = new CurrentlyDto
                {
                    time = weather.currently.time,
                    summary = weather.currently.summary,
                    icon = weather.currently.icon,
                    temperature = weather.currently.temperature,
                },
                Daily = new DailyDto
                {
                    summary = weather.daily.summary,
                    icon = weather.daily.icon,
                    data = weather.daily.data.Select(x => new DatumDto
                    {
                        time = x.time,
                        summary = x.summary,
                        icon = x.icon,
                        temperature = x.temperature
                    }).ToList()
                },
                Hourly = new HourlyDto
                {
                    summary = weather.hourly.summary,
                    icon = weather.hourly.icon,
                    data = weather.hourly.data.Select(x => new DatumDto
                    {
                        time = x.time,
                        summary = x.summary,
                        icon = x.icon,
                        temperature = x.temperature
                    }).ToList()
                },
                Timezone = weather.timezone
            };

            return weatherDto;
        }
    }
}
