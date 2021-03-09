using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RoutePlannerApi.Areas.Identity.Data;

namespace RoutePlannerApi.Areas.Identity.Data
{
    public class UsersDbContext : ApiAuthorizationDbContext<RoutePlannerAppUser>
    {
        public UsersDbContext(
            DbContextOptions options, 
            IOptions<OperationalStoreOptions> operationalStoreOptions) 
                : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
