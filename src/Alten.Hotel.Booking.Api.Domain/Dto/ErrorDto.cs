using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alten.Hotel.Booking.Api.Domain.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ErrorDto
    {
        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IEnumerable<string> Errors { get; set; }

        /// <summary>
        /// News the specified errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static ErrorDto New(IEnumerable<string> errors)
        {
            return new ErrorDto
            {
                Errors = errors
            };
        }
    }
}
