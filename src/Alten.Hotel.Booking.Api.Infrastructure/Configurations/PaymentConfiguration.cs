//using Alten.Hotel.Booking.Api.Domain.Aggregates;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Alten.Hotel.Booking.Api.Infrastructure.Configurations
//{
//    class PaymentConfiguration : IEntityTypeConfiguration<Payment>
//    {
//        public static BookingConfiguration New()
//        {
//            return new BookingConfiguration();
//        }

//        public void Configure(EntityTypeBuilder<Payment> builder)
//        {
//            builder.HasKey(x => x.Id);

//            builder.Property(x => x.CardNumber)
//                .HasMaxLength(20)
//                .IsRequired();

//            builder.Property(x => x.CartType)
//                .HasMaxLength(2)
//                .IsRequired();

//            builder.Property(x => x.ExpirationMonth)
//                .IsRequired();

//            builder.Property(x => x.ExpirationYear)
//                .IsRequired();

//            builder.Property(x => x.TotalAmount)
//                .IsRequired();
//        }
//    }
//}