using Infrastructure.Common;

namespace Contracts
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        
        public string MobilePhone { get; set; }
        
        public string Telegram { get; set; }
        
        public Coordinate Coordinate { get; set; }
    }
}