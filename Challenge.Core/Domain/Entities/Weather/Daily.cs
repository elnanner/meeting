using System.Collections.Generic;

namespace Challenge.Core.Domain.Entities.Weather
{
    public class Daily
    {
        public string summary { get; set; }
        public string icon { get; set; }
        public List<Datum> data { get; set; }
    }
}
