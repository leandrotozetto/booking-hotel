using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alten.Hotel.Booking.Api.Infrastructure.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Domain.Aggregates.BookingAggregate.Booking>
    {
        public static BookingConfiguration New()
        {
            return new BookingConfiguration();
        }

        public void Configure(EntityTypeBuilder<Domain.Aggregates.BookingAggregate.Booking> builder)
        {
            builder.HasKey(x => x.Id);

            //builder.HasOne<Payment>()
            //    .WithMany()
            //    .HasForeignKey(x => x.Id);

            //builder.HasOne<Guest>()
            //    .WithMany()
            //    .HasForeignKey(x => x.Id);

            builder.Property(x => x.CheckIn)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(x => x.CheckOut)
                .HasColumnType("date")
                .IsRequired();

            //builder.Property(x => x.RoomNumber)
            //    .IsRequired();


            builder.Ignore(x => x.Errors);

            builder.HasIndex(x => new { x.CheckIn, x.CheckOut })
                .IsUnique();
        }
    }
}