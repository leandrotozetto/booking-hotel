using Microsoft.EntityFrameworkCore.Migrations;

namespace Alten.Hotel.Booking.Api.Infrastructure.Migrations
{
    public partial class procedure_get_available_dates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcSql = @"CREATE PROCEDURE GET_AVAILABLE_DATES
								(
									@START_DATE DATE,
									@END_DATE DATE,
									@EXCLUDE_BOOKING_ID UNIQUEIDENTIFIER = NULL
								)
								AS
								BEGIN
									;WITH CTE_BOOKING_DATES_AVAILABLE([Index], [Date]) AS
									(
										SELECT 
											1 AS [Index], 
											DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS [Date]
										UNION ALL
										SELECT 
											[Index] + 1 AS [Index], 
											DATEADD(DAY, [Index], @START_DATE) AS [Date]
										FROM CTE_BOOKING_DATES_AVAILABLE
										WHERE [Index] < 32
									),
									CTE_BOOKED_DATES([Index], [Date], EndDate, BookingId) AS
									(
										SELECT
											0 AS [Index], 
											CheckIn AS [Date], 
											CheckOut AS EndDate,
											Id AS BookingId
										FROM CTE_BOOKING_DATES_AVAILABLE CB
											JOIN Booking B ON B.CheckIn = CB.[Date]
										UNION ALL
										SELECT
											[Index] + 1 AS [Index], 
											DATEADD(DAY, 1, [Date]) AS [Date], 
											EndDate,
											BookingId
										FROM CTE_BOOKED_DATES B
										WHERE DATEADD(DAY, [Index], [Date]) <= EndDate
											AND EndDate > [Date]
									)
									SELECT
										DA.[Date],
										CASE WHEN @EXCLUDE_BOOKING_ID IS NULL OR BookingId <> @EXCLUDE_BOOKING_ID THEN BookingId ELSE NULL END AS BookingId
									FROM CTE_BOOKING_DATES_AVAILABLE DA
										LEFT JOIN CTE_BOOKED_DATES BD ON BD.[Date] = DA.[Date]
									ORDER BY DA.[Date]
								END
								GO";
            migrationBuilder.Sql(createProcSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			var dropProcSql = @"IF OBJECT_ID('GET_AVAILABLE_DATES', 'P') IS NOT NULL
								BEGIN
									DROP PROCEDURE GET_AVAILABLE_DATES
								END
								GO";
			migrationBuilder.Sql(dropProcSql);
		}
    }
}
