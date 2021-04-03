using Infrastructure.Common;

namespace Entities.Models
{
    public class UserRight
    {
        public int UserId { get; set; }
        
        public Right Right { get; set; }
    }
}