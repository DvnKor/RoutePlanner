using System;
using System.Net;
using System.Threading.Tasks;
using Infrastructure.Rights;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace RoutePlannerApi.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RightsAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly Right[] _rights;

        public RightsAuthorizeAttribute(params Right[] rights)
        {
            _rights = rights;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userContext = context.HttpContext.RequestServices.GetService<IUserContext>();
            if (userContext?.User?.UserRights == null)
            {
                throw new InvalidOperationException("Пользователь не авторизован");
            }

            var hasRight = userContext.User.HasRights(_rights);

            if (!hasRight)
            {
                await context.HttpContext.ForbidAsync();
            }
        }
    }
}