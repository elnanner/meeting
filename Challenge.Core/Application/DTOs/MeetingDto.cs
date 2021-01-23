using Challenge.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Challenge.Core.Application.DTOs
{
    public class MeetingDto
    {
        //public MeetingDto(int id, User admin, DateTime date, int count, City city)
        //{
        //    MeetingId = id;
        //    Admin = admin;
        //    Date = date;
        //    BeerCount = count;
        //    City = city;
        //    Checkins = new List<int>();
        //    Guests = new List<User>();

        //}

        public int MeetingId { get; set; }

        public string Description { get; set; }

        public int AdminId { get; set; }

        public DateTime Date { get; set; }

        public int MaxPeople { get; set; }

        public CityDto City { get; set; }

    }
}
