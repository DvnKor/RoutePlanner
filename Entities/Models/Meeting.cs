using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Meeting")]
    public class Meeting
    {
        public int Id { get; set; }
        
        public int ClientId { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public Client Client { get; set; }
    }
}