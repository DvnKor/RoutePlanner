using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configs
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder
                .HasOne(route => route.ManagerSchedule)
                .WithMany()
                .HasForeignKey(route => route.ManagerScheduleId);

            builder
                .HasMany(route => route.Meetings)
                .WithOne(meeting => meeting.Route)
                .HasForeignKey(meeting => meeting.RouteId);
        }
    }
}