using Alten.Hotel.Booking.Api.Application.Mappers;
using Alten.Hotel.Booking.Api.Domain.Dto;
using AutoMapper;
using System;
using Xunit;

namespace Alten.Hotel.Booking.Api.Application.Test.Mappers
{
    public class BookingProfileTest
    {
        private static IMapper _mapper;

        public BookingProfileTest()
        {
            _mapper = GetMapper();
        }

        [Fact]
        public void Should_Convert_To_Booking()
        {
            var checkIn = DateTime.Now.AddDays(2);
            var checkOut = DateTime.Now.AddDays(4);
            var bookingDto = BookingDto.New(checkIn, checkOut);
            var booking = _mapper.Map<Domain.Aggregates.BookingAggregate.Booking>(bookingDto);

            Assert.Equal(checkIn, booking.CheckIn);
            Assert.Equal(checkOut, booking.CheckOut);
        }

        [Fact]
        public void Should_Convert_To_BookingDto()
        {
            var checkIn = DateTime.Now.AddDays(2);
            var checkOut = DateTime.Now.AddDays(4);
            var booking = Domain.Aggregates.BookingAggregate.Booking.New(checkIn, checkOut);
            var bookingDto = _mapper.Map<BookingDto>(booking);

            Assert.Equal(checkIn, bookingDto.CheckIn);
            Assert.Equal(checkOut, bookingDto.CheckOut);
        }

        [Fact]
        public void Should_Convert_To_BookingResponseDto()
        {
            var checkIn = DateTime.Now.AddDays(2);
            var checkOut = DateTime.Now.AddDays(4);
            var booking = Domain.Aggregates.BookingAggregate.Booking.New(checkIn, checkOut);
            var bookingDto = _mapper.Map<BookingResponseDto>(booking);

            Assert.Equal(checkIn, bookingDto.CheckIn);
            Assert.Equal(checkOut, bookingDto.CheckOut);
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x =>
            {
                x.AddProfile(new BookingProfile());
            }).CreateMapper();
        }
    }
}
