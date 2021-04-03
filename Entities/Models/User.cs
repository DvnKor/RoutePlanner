using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Infrastructure.Rights;

namespace Entities.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Picture { get; set; }
        
        public List<UserRight> Rights { get; set; }

        public bool HasRights(Right[] rights)
        {
            if (Rights == null) return false;
            var userRightsValues = Rights.Select(userRight => userRight.Right);
            return !rights.Except(userRightsValues).Any();
        }
    }
}