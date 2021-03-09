using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using RoutePlannerApi.Areas.Identity.Data;

namespace RoutePlannerApi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<RoutePlannerAppUser> _claimsFactory;
        private readonly UserManager<RoutePlannerAppUser> _userManager;

        public ProfileService(UserManager<RoutePlannerAppUser> userManager,
            IUserClaimsPrincipalFactory<RoutePlannerAppUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // Add custom claims in token here based on user properties or any other source
            //claims.Add(new Claim(CustomClaimTypes.PhotoUrl, user.PhotoUrl ?? string.Empty));
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}