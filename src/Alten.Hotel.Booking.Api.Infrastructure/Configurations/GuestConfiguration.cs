//using Alten.Hotel.Booking.Api.Domain.Aggregates;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Alten.Hotel.Booking.Api.Infrastructure.Configurations
//{
//    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
//    {
//        public static BookingConfiguration New()
//        {
//            return new BookingConfiguration();
//        }

//        public void Configure(EntityTypeBuilder<Guest> builder)
//        {
//            builder.HasKey(x => x.Id);

//            builder.Property(x => x.FirstName)
//                .HasMaxLength(100)
//                .IsRequired();

//            builder.Property(x => x.LastName)
//                .HasMaxLength(200)
//                .IsRequired();

//            builder.Property(x => x.Email)
//                .HasMaxLength(255)
//                .IsRequired();

//            builder.Property(x => x.Phone)
//                .HasMaxLength(15)
//                .IsRequired();
//        }
//    }
//}