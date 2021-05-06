using Entities.Models;
using Google.Apis.Auth;

namespace RoutePlannerApi.Auth
{
    public static class PayloadExtensions
    {
        public static User ToUser(this GoogleJsonWebSignature.Payload payload)
        {
            return new User
            {
                Name = payload.Name,
                Email = payload.Email,
                Picture = payload.Picture,
            };
        }
    }
}