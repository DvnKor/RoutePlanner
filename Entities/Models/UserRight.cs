using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Rights;

namespace Entities.Models
{
    [Table("UserRight")]
    public class UserRight
    {
        public int UserId { get; set; }
        
        public Right Right { get; set; }
        
        public RightInfo RightInfo { get; set; }
    }
}