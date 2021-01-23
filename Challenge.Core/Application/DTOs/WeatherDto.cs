using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge.Core.Application.DTOs
{
    public class WeatherDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timezone { get; set; }
        public CurrentlyDto Currently { get; set; }
        public HourlyDto Hourly { get; set; }
        public DailyDto Daily { get; set; }
    }
}
