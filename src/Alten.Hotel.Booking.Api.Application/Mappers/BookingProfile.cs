using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Aggregates;
using AutoMapper;

namespace Alten.Hotel.Booking.Api.Application.Mappers
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingDto, IBooking>()
                .ConstructUsing((dto, entity) =>
                {
                    var booking = Domain.Aggregates.BookingAggregate.Booking.New(dto.CheckIn, dto.CheckOut);

                    return booking;
                });

            CreateMap<IBooking, BookingDto>()
                .ConstructUsing((entity, dto) =>
                {
                    return BookingDto.New(entity.CheckIn, entity.CheckOut);
                });

            CreateMap<IBooking, BookingResponseDto>()
                .ConstructUsing((entity, dto) =>
                {
                    return new BookingResponseDto
                    {
                        CheckOut = entity.CheckOut,
                        CheckIn = entity.CheckIn,
                        Id = entity.Id
                    };
                });
        }
    }
}
