using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configs
{
    public class ManagerScheduleConfiguration : IEntityTypeConfiguration<ManagerSchedule>
    {
        public void Configure(EntityTypeBuilder<ManagerSchedule> builder)
        {
            builder.Property(managerSchedule => managerSchedule.StartCoordinate)
                .HasSimpleJsonConversion();
            
            builder.Property(managerSchedule => managerSchedule.EndCoordinate)
                .HasSimpleJsonConversion();
        }
    }
}