using System;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Entities.Configs
{
    public class ClientScheduleConfiguration : IEntityTypeConfiguration<ClientSchedule>
    {
        public void Configure(EntityTypeBuilder<ClientSchedule> builder)
        {
            builder.HasKey(clientSchedule => new {clientSchedule.ClientId, clientSchedule.Day});
            
            builder.Property(clientSchedule => clientSchedule.Day)
                .HasConversion(new EnumToNumberConverter<DayOfWeek, int>());
        }
    }
}