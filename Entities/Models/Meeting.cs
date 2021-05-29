using System;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;

namespace Entities.Models
{
    [Table("Meeting")]
    public class Meeting
    {
        public int Id { get; set; }
        
        public int ClientId { get; set; }
        
        public Client Client { get; set; }

        public DateTime AvailableTimeStart { get; set; }
        
        public DateTime AvailableTimeEnd { get; set; }
        
        public int DurationInMinutes { get; set; }

        public Coordinate Coordinate { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        [NotMapped]
        public double DistanceFromPrevious { get; set; }
        
        [NotMapped]
        public double WaitingTime { get; set; }
    }
}