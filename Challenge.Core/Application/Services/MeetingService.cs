using Challenge.Core.Application.DTOs;
using Challenge.Core.Application.Exceptions;
using Challenge.Core.CustomEntities;
using Challenge.Core.Domain.Entities;
using Challenge.Core.Domain.Interfaces;
using Challenge.Core.QueryFilter;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Core.Application.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IWeatherService _weatherService;
        private readonly PaginationOptions _paginationOptions;

        public MeetingService(IMeetingRepository repository, IWeatherService weatherService, IOptions<PaginationOptions> options)
        {
            _meetingRepository = repository;
            _weatherService = weatherService;
            _paginationOptions = options.Value;
        }
        public async Task<PagedList<MeetingDto>> GetMeetings(MeetingQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;

            try
            {
                var meetings = await _meetingRepository.GetMeetings(filters);

                var meetingsDto = meetings.Select(m =>
                    new MeetingDto
                    {
                        MeetingId = m.MeetingId,
                        Description = m.Description,
                        AdminId = m.AdminId,
                        Date = m.Date,
                        MaxPeople = m.MaxPeople,
                        City = new CityDto
                        {
                            CityId = m.City.CityId,
                            Name = m.City.Name,
                            Latitude = m.City.Latitude,
                            Longitude = m.City.Longitude
                        }
                    }).AsEnumerable();

                return PagedList<MeetingDto>.Create(meetingsDto, filters.PageNumber, filters.PageSize);
            }
            catch (Exception ex)
            {

                throw new ChallengeException(ex.Message);
            }

        }

        public async Task<MeetingDto> GetMeeting(int id)
        {
            var meeting = await _meetingRepository.GetMeeting(id);

            if (meeting == null)
            {
                return null;
            }

            var meetingDto = new MeetingDto
            {
                MeetingId = meeting.MeetingId,
                Description = meeting.Description,
                AdminId = meeting.AdminId,
                Date = meeting.Date,
                MaxPeople = meeting.MaxPeople,
                City = new CityDto
                {
                    CityId = meeting.City.CityId,
                    Name = meeting.City.Name,
                    Latitude = meeting.City.Latitude,
                    Longitude = meeting.City.Longitude
                }
            };

            return meetingDto;
        }

        public async Task<MeetingDto> InsertMeeting(MeetingDto meetingDto)
        {
            var meeting = new Meeting
            {
                MeetingId = meetingDto.MeetingId,
                Description = meetingDto.Description,
                AdminId = meetingDto.AdminId,
                Date = meetingDto.Date,
                MaxPeople = meetingDto.MaxPeople,
                City = new City
                {
                    CityId = meetingDto.City.CityId,
                    Name = meetingDto.City.Name,
                    Latitude = meetingDto.City.Latitude,
                    Longitude = meetingDto.City.Longitude
                }
            };
            try
            {
                // valido que por lo menos sea con un dia de diferncia la meeting
                if ((meeting.Date - DateTime.Now).TotalDays < 1)
                {
                    throw new ChallengeException("La fecha ingresada debe ser mayor a hoy.");
                }

                var inserted = await _meetingRepository.InsertMeeting(meeting);
                meetingDto.MeetingId = inserted.MeetingId;
                meetingDto.City = new CityDto
                {
                    CityId = inserted.City.CityId,
                    Name = inserted.City.Name,
                    Latitude = inserted.City.Latitude,
                    Longitude = inserted.City.Longitude
                };
                return meetingDto;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Ya existe"))
                {
                    throw new ChallengeException(ex.Message);
                }

                if (ex.Message.Contains("ciudad"))
                {
                    throw new ChallengeException(ex.Message);
                }
                throw;
            }

        }

        public async Task<bool> UpdateMeeting(MeetingDto meetingDto, int adminId)
        {
            try
            {
                var meeting = new Meeting
                {
                    MeetingId = meetingDto.MeetingId,
                    Description = meetingDto.Description,
                    AdminId = meetingDto.AdminId,
                    Date = meetingDto.Date,
                    MaxPeople = meetingDto.MaxPeople,
                    City = new City
                    {
                        CityId = meetingDto.City.CityId,
                        Name = meetingDto.City.Name,
                        Latitude = meetingDto.City.Latitude,
                        Longitude = meetingDto.City.Longitude
                    }
                };

                if ((meeting.Date - DateTime.Now).TotalDays < 1)
                {
                    throw new ChallengeException("La fecha ingresada tiene que ser por lo menos un dia antes de la meeting.");
                }

                if (meeting.AdminId == adminId)
                {
                    throw new ChallengeException("Usted no es el admin de esta meeting, no puede por ende, realizar modificaciones a la misma.");
                }

                // no esta contemplado el cambio de admin para una meeting

                return await _meetingRepository.UpdateMeeting(meeting);
            }
            catch (Exception ex)
            {
                throw new ChallengeException(ex.Message);
            }

        }

        public async Task<bool> DeleteMeeting(int id)
        {
            try
            {
                if (id < 1)
                {
                    throw new ChallengeException("El id de la meeting es invalido.");
                }
                return await _meetingRepository.DeleteMeeting(id);
            }
            catch (Exception e)
            {
                throw new ChallengeException(e.Message);
            }
        }

        public async Task<bool> SignOn(int meetingId, int userId)
        {
            // buscamos la meeting y validamos cuestiones antes de llamar al repo
            // se puede inscribir con un dia como mínimo, no esta estipulado, pero lo valido así
            try
            {
                var meeting = await GetMeeting(meetingId);

                if (meeting == null)
                {
                    throw new ChallengeException("La meeting no existe.");
                }

                var totalDays = (meeting.Date - DateTime.Now).TotalDays;

                if (totalDays < 0)
                {
                    throw new ChallengeException("La meeting ya pasó. Lo sentimos.");
                }

                if (totalDays < 1)
                {
                    throw new ChallengeException("Ya no es posible inscribirse a esta meeting.");
                }

                return await _meetingRepository.SignOn(meetingId, userId);
            }
            catch (Exception e)
            {
                throw new ChallengeException(e.Message);
            }
        }

        public async Task<bool> CheckIn(int meetingId, int userId)
        {
            try
            {
                var meeting = await GetMeeting(meetingId);

                if (meeting == null)
                {
                    throw new ChallengeException("La meeting no existe.");
                }

                var totalDays = (meeting.Date - DateTime.Now).TotalDays;

                // La siguiente validacion la quite prque calculo las cervezas(packs de 6) en base a los checkins
                //if (totalDays > 0)
                //{
                //    throw new ChallengeException("No se puede avisar que se asistió a una meeting que no ha ocurrido.");
                //}

                return await _meetingRepository.CheckIn(meetingId, userId);
            }
            catch (Exception e)
            {

                throw new ChallengeException(e.Message);
            }

        }

        public async Task<int> GetAttendedCount(int meetingId)
        {
            try
            {
                return await _meetingRepository.GetAttendedCount(meetingId);
            }
            catch (Exception e)
            {

                throw new ChallengeException(e.Message);
            }
        }

        public async Task<int> GetBeerPacks(int meetingId)
        {
            try
            {
                var meeting = await _meetingRepository.GetMeeting(meetingId);

                var totalDays = (meeting.Date - DateTime.Now).TotalDays;// TODO migrate to private function

                if (totalDays < 0)
                {
                    throw new ChallengeException("La meeting ya pasó. Lo sentimos.");
                }

                // Valido que hasta 1 hora del inicio de la meeting se pueda calcular las birras.
                if (totalDays < 1)
                {
                    throw new ChallengeException("No es posible ya calcular las birras para la meeting. Sos un colgado!.");
                }

                // si faltan mas de 7 dias tampoco le permito, el api del clima me da info de hasta 7-8 dias
                if (totalDays > 7)
                {
                    throw new ChallengeException("Intentá en una fecha mas cercana a la meeting.");
                }

                // podria haber mas validaciones, no especifica

                var attendes = await _meetingRepository.GetAttendedCount(meetingId);

                // calculo de la temperatura en base al horario de la meeting
                var temps = await _weatherService.GetWeatherByLocation(meeting.City.Latitude, meeting.City.Longitude);

                return CalculateBeers(meeting.Date, temps, attendes, totalDays);

            }
            catch (Exception e)
            {

                throw new ChallengeException(e.Message);
            }

        }

        // se pone publico para poder testearlo
        public int CalculateBeers(DateTime date, WeatherDto temps, int attendes, double totalDays)
        {
            // Calcula hasta aprox 2 dias de diferencia, y en base al horario de la meeting
            DatumDto tempInfo = new DatumDto();
            double necessaryBeers = 0d;

            if (totalDays < 2)
            {
                // Hourly find
                tempInfo = temps.Hourly.data.Where(t => DateTimeOffset.FromUnixTimeSeconds(t.time).ToLocalTime().Hour == date.Hour).FirstOrDefault();
            }

            if (totalDays > 2)
            {
                throw new ChallengeException("Solo esta permitido el calculo hasta 2 dias antes de la reunión y en base a la hora,\n" +
                                             "el calculo semanal no esta hecho por cuestiones de tiempo");
            }

            // se toma una birra por persona
            if (tempInfo.temperature > 20 && tempInfo.temperature < 24)
            {
                necessaryBeers = attendes;
            }

            // se toma 3/4 bira por persona
            if (tempInfo.temperature < 20)
            {
                necessaryBeers = attendes * 0.75d;
            }

            // se toman 2 birras per capita
            if (tempInfo.temperature > 24)
            {
                necessaryBeers = attendes * 2;
            }
            return (int)Math.Ceiling(necessaryBeers / 6);
        }

        public async Task<bool> Invite(int meetingId, int[] guests)
        {
            // Send guest invitation

            return Task.CompletedTask.IsCompleted;
        }
    }
}
