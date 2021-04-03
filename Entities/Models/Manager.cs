using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Manager")]
    public class Manager
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public List<ManagerSchedule> ManagerSchedules { get; set; }
    }
}