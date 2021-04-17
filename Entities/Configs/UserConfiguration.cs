using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasIndex(user => user.Email)
                .IsUnique();
            
            builder
                .Property(user => user.Coordinate)
                .HasSimpleJsonConversion();
        }
    }
}