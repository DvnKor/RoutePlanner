using Infrastructure.Common;

namespace Entities.Models
{
    public class UserRight
    {
        public int UserId { get; set; }
        
        public Right Right { get; set; }
        
        public User User { get; set; }
        
        public RightInfo RightInfo { get; set; }
    }
}