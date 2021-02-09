using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Contracts
{
    [Table("Organization")]
    public class Organization
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public List<Manager> Managers { get; set; }
    }
}