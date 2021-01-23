using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge.Core.Application.DTOs
{
    public class HourlyDto
    {
        public string summary { get; set; }
        public string icon { get; set; }
        public List<DatumDto> data { get; set; }
    }
}
