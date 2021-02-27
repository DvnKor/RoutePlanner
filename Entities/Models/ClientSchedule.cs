using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("ClientSchedule")]
    public class ClientSchedule
    {
        public int ClientId { get; set; }
        
        public DayOfWeek Day { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public Client Client { get; set; }
    }
}