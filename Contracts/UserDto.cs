using Infrastructure.Rights;

namespace Contracts
{
    public class UserDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Picture { get; set; }

        public string MobilePhone { get; set; }
        
        public string Telegram { get; set; }
        
        public string Position { get; set; }
        
        public Right[] Rights { get; set; }
    }
}