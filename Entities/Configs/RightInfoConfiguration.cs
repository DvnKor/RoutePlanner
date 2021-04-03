using Entities.Models;
using Infrastructure.Rights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Entities.Configs
{
    public class RightInfoConfiguration : IEntityTypeConfiguration<RightInfo>
    {
        public void Configure(EntityTypeBuilder<RightInfo> builder)
        {
            builder.HasKey(rightInfo => rightInfo.Right);

            builder.Property(rightInfo => rightInfo.Right)
                .HasConversion(new EnumToNumberConverter<Right, int>());
            
            builder
                .HasMany(rightInfo => rightInfo.UserRights)
                .WithOne()
                .HasForeignKey(userRight => userRight.Right);
        }
    }
}