using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configs
{
    public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder.Property(meeting => meeting.Coordinate)
                .HasSimpleJsonConversion();
        }
    }
}