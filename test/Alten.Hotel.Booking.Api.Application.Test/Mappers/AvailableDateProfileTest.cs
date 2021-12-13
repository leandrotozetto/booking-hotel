using Alten.Hotel.Booking.Api.Application.Mappers;
using Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate;
using Alten.Hotel.Booking.Api.Domain.Dto;
using AutoMapper;
using System;
using Xunit;

namespace Alten.Hotel.Booking.Api.Application.Test.Mappers
{
    public class AvailableDateProfileTest
    {
        private static IMapper _mapper;

        public AvailableDateProfileTest()
        {
            _mapper = GetMapper();
        }

        [Fact]
        public void Should_Convert_When_UnAvailable()
        {
            var id = Guid.NewGuid();
            var date = DateTime.Now;

            var availableDate = AvailableDate.New(date, id);
            var availableDateDto = _mapper.Map<AvailableDateDto>(availableDate);

            Assert.False(availableDateDto.IsAvailable);
            Assert.Equal(date, availableDateDto.Date);
        }

        [Fact]
        public void Should_Convert_When_Is_Available()
        {
            var date = DateTime.Now;

            var availableDate = AvailableDate.New(date, Guid.Empty);
            var availableDateDto = _mapper.Map<AvailableDateDto>(availableDate);

            Assert.True(availableDateDto.IsAvailable);
            Assert.Equal(date, availableDateDto.Date);
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x =>
            {
                x.AddProfile(new AvailableDateProfile());
            }).CreateMapper();
        }
    }
}
