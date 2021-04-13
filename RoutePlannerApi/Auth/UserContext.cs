using Entities.Models;

namespace RoutePlannerApi.Auth
{
    public interface IUserContext
    {
        User User { get; }
        void SetUser(User user);
    }
    
    public class UserContext : IUserContext
    {
        public User User { get; private set; }
            
        public void SetUser(User user)
        {
            User = user;
        }
    }
}