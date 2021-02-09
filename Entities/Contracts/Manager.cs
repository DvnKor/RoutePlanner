using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Contracts
{
    [Table("Manager")]
    public class Manager
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int OrganizationId { get; set; }
        
        public Organization Organization { get; set; }
        
        public List<ManagerSchedule> ManagerSchedules { get; set; }
    }
}