using System.Collections.Generic;

namespace Entities.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Picture { get; set; }
        
        public List<RightInfo> Rights { get; set; }
    }
}