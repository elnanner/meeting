using System.Collections.Generic;

namespace Challenge.Core.Domain.Entities.Weather
{
    public class Hourly
    {
        public string summary { get; set; }
        public string icon { get; set; }
        public List<Datum> data { get; set; }
    }
}
