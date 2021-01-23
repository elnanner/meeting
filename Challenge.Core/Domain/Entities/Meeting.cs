using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Challenge.Core.Domain.Entities
{
    public class Meeting
    {
        public int MeetingId { get; set; }

        public string Description { get; set; }

        public int AdminId { get; set; }

        public DateTime Date { get; set; }

        public int MaxPeople { get; set; }// cantidad maxima de personas

        public City City { get; set; }
    }
}
