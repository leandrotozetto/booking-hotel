using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alten.Hotel.Booking.Api.Interface.Converters
{
    /// <summary>
    /// DateTime converter
    /// </summary>
    /// <seealso cref="JsonConverter{T}" />
    [ExcludeFromCodeCoverage]
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Reads and converts the JSON />.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var valueFormated = value.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            writer.WriteStringValue(valueFormated);
        }
    }
}