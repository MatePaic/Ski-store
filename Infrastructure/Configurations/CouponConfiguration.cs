using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<AppCoupon>
    {
        public void Configure(EntityTypeBuilder<AppCoupon> builder)
        {
            builder.Property(x => x.PercentOff).HasColumnType("decimal(18,2)");
            builder.Property(x => x.AmountOff).HasColumnType("decimal(18,2)");
        }
    }
}
