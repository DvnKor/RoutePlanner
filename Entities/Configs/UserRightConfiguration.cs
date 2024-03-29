using Entities.Models;
using Infrastructure.Rights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Entities.Configs
{
    public class UserRightConfiguration : IEntityTypeConfiguration<UserRight>
    {
        public void Configure(EntityTypeBuilder<UserRight> builder)
        {
            builder.HasKey(userRight => new {userRight.UserId, userRight.Right});

            builder.Property(userRight => userRight.Right)
                .HasConversion(new EnumToNumberConverter<Right, int>());
        }
    }
}