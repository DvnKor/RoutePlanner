using System;
using System.Net;
using System.Threading.Tasks;
using Infrastructure.Rights;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace RoutePlannerApi.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RightsAuthorize : Attribute, IAsyncAuthorizationFilter
    {
        private readonly Right[] _rights;

        public RightsAuthorize(params Right[] rights)
        {
            _rights = rights;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userContext = context.HttpContext.RequestServices.GetService<IUserContext>();
            if (userContext?.User?.Rights == null)
            {
                throw new InvalidOperationException("Пользователь не авторизован");
            }

            var hasRight = userContext.User.HasRights(_rights);

            if (!hasRight)
            {
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            }
        }
    }
}