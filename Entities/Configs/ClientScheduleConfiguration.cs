using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configs
{
    public class ClientScheduleConfiguration : IEntityTypeConfiguration<ClientSchedule>
    {
        public void Configure(EntityTypeBuilder<ClientSchedule> builder)
        {
        }
    }
}