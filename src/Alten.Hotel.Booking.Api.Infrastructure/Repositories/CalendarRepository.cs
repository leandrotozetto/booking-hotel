using Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Infrastructure.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        /// <summary>
        /// The database connection
        /// </summary>
        private readonly IDbConnection _dbConnection;

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRepository"/> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        public CalendarRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Gets the available dates asynchronous.
        /// </summary>
        /// <param name="StartDate">The start date.</param>
        /// <param name="EndDate">The end date.</param>
        /// <returns>Return list of available dates</returns>
        public async Task<IEnumerable<AvailableDate>> GetAvailableDatesAsync(DateTime StartDate, DateTime EndDate, Guid? currentBookingId = null)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@START_DATE", StartDate, DbType.Date);
            parameters.Add("@END_DATE", EndDate, DbType.Date);

            if (currentBookingId is not null)
            {
                parameters.Add("@EXCLUDE_BOOKING_ID", currentBookingId.ToString(), DbType.String);
            }

            var procName = "GET_AVAILABLE_DATES";

            return await _dbConnection.QueryAsync<AvailableDate>(procName, param: parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_dbConnection.State == ConnectionState.Open)
                    {
                        _dbConnection.Close();
                        _dbConnection.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
