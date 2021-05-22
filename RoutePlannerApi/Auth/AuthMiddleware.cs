using System;
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
            var accessToken = context.Request.Query["access_token"].ToString();
            var authorizationHeader = context.Request.Headers["Authorization"].ToArray();
            if (authorizationHeader.Length == 0 && string.IsNullOrEmpty(accessToken))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token is invalid");
                return;
            }

            var token = !string.IsNullOrEmpty(accessToken) ? accessToken : authorizationHeader[0].Split(' ')[1];
            try
            {
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(token);
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
            catch (InvalidJwtException ex)
            {
                Console.WriteLine(ex);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("JWT has expired");
            }
        }
    }
}