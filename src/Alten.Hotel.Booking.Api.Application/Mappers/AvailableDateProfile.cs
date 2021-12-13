using Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate;
using Alten.Hotel.Booking.Api.Domain.Dto;
using AutoMapper;

namespace Alten.Hotel.Booking.Api.Application.Mappers
{
    public class AvailableDateProfile : Profile
    {
        public AvailableDateProfile()
        {
            CreateMap<AvailableDate, AvailableDateDto>();
        }
    }
}
