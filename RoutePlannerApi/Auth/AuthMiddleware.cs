using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Storages;

namespace RoutePlannerApi.Auth
{
    public class AuthMiddleware 
    {
        private readonly RequestDelegate _next;
        
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].ToArray();
            if (authorizationHeader.Length == 0)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token is invalid");
                return;
            }
            var token = authorizationHeader[0].Split(' ')[1];
            var validPayload = await GoogleJsonWebSignature.ValidateAsync(token);

            if (validPayload == null)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token is invalid");
                return;
            }
            
            var userStorage = context.RequestServices.GetRequiredService<IUserStorage>();
            var currentUser = await userStorage.GetByEmail(validPayload.Email);
            if (currentUser == null)
            {
                currentUser = validPayload.ToUser();
                currentUser.Id = await userStorage.AddUser(currentUser);
            }

            var userContext = context.RequestServices.GetRequiredService<IUserContext>();
            userContext.SetUser(currentUser);
            
            await _next.Invoke(context);
        }
    }
}