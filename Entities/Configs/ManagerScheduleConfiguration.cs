using System;
using Entities.Common;
using Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Entities.Configs
{
    public class ManagerScheduleConfiguration : IEntityTypeConfiguration<ManagerSchedule>
    {
        public void Configure(EntityTypeBuilder<ManagerSchedule> builder)
        {
            builder.HasKey(managerSchedule => new {managerSchedule.ManagerId, managerSchedule.Day});

            builder.Property(managerSchedule => managerSchedule.Day)
                .HasConversion(new EnumToNumberConverter<DayOfWeek, int>());
            
            builder.Property(managerSchedule => managerSchedule.StartCoordinate).HasSimpleJsonConversion();
            
            builder.Property(managerSchedule => managerSchedule.EndCoordinate).HasSimpleJsonConversion();
        }
    }
}