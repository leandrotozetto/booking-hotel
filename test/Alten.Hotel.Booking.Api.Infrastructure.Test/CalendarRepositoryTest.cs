using Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate;
using Alten.Hotel.Booking.Api.Infrastructure.Repositories;
using Dapper;
using Moq;
using Moq.Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Alten.Hotel.Booking.Api.Infrastructure.Test
{
    public class CalendarRepositoryTest
    {
        private CalendarRepository _calendarRepository;

        public CalendarRepositoryTest()
        {
            _calendarRepository = new CalendarRepository(GetConnection());
        }

        [Fact]
        public async Task Should_Be_Return_Available_Dates_When_CurrentBookingId_Is_Null()
        {
            var entities = await _calendarRepository.GetAvailableDatesAsync(DateTime.Now, DateTime.Now);

            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task Should_Be_Return_Available_Dates_When_CurrentBookingId_Is_Not_Null()
        {
            var entities = await _calendarRepository.GetAvailableDatesAsync(DateTime.Now, DateTime.Now, Guid.NewGuid());

            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task Should_Be_Return_Empty_List()
        {
            var dbConnection = new Mock<IDbConnection>();

            dbConnection.SetupDapperAsync(x => x.QueryAsync<AvailableDate>(It.IsAny<string>(), null, null, null, null))
                      .ReturnsAsync(Enumerable.Empty<AvailableDate>());

            _calendarRepository = new CalendarRepository(dbConnection.Object);

            var entities = await _calendarRepository.GetAvailableDatesAsync(DateTime.Now, DateTime.Now);

            Assert.Empty(entities);
        }

        private static IDbConnection GetConnection()
        {
            var dbConnection = new Mock<IDbConnection>();

            dbConnection.SetupDapperAsync(x => x.QueryAsync<AvailableDate>(It.IsAny<string>(), null, null, null, null))
                      .ReturnsAsync(() =>
                      {
                          return new Collection<AvailableDate>
                          {
                              AvailableDate.New(DateTime.Now),
                              AvailableDate.New(DateTime.Now),
                              AvailableDate.New(DateTime.Now),
                              AvailableDate.New(DateTime.Now)
                          };
                      });

            return dbConnection.Object;
        }
    }
}
