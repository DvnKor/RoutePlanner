using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;

namespace Entities.Contracts
{
    [Table("Client")]
    public class Client
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Coordinate Coordinate { get; set; }
        
        public List<ClientSchedule> ClientSchedules { get; set; }
    }
}