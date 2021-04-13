using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;

namespace Entities.Models
{
    [Table("Client")]
    public class Client
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Picture { get; set; }
        
        public string MobilePhone { get; set; }
        
        public string Telegram { get; set; }
        
        public Coordinate Coordinate { get; set; }
        
        public List<ClientSchedule> ClientSchedules { get; set; }
    }
}