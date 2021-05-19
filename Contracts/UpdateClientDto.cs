using Infrastructure.Common;

namespace Contracts
{
    public class UpdateClientDto
    {
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string MobilePhone { get; set; }
        
        public string Telegram { get; set; }
        
        public Coordinate Coordinate { get; set; }
    }
}