using System;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;

namespace Entities.Models
{
    [Table("ManagerSchedule")]
    public class ManagerSchedule
    {
        public int Id { get; set; } 
            
        public int ManagerId { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public Coordinate StartCoordinate { get; set; }
        
        public Coordinate EndCoordinate { get; set; }
        
        public Manager Manager { get; set; }
    }
}