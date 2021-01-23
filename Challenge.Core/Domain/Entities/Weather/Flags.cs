using Newtonsoft.Json;
using System.Collections.Generic;

namespace Challenge.Core.Domain.Entities.Weather
{
    public class Flags
    {
        public List<string> sources { get; set; }
        [JsonProperty("nearest-station")]
        public double NearestStation { get; set; }
        public string units { get; set; }
    }
}
