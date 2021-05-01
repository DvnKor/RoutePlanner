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
        
        public string Name { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public Coordinate Coordinate { get; set; }
        
        public Client Client { get; set; }
    }
}