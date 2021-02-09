using System.Threading.Tasks;
using Entities;
using Entities.Contracts;

namespace Storages
{
    public interface IOrganizationStorage
    {
        Task<Organization> GetOrganizationAsync(int id);
        
        Task<Organization> CreateOrganizationAsync(string name);

        Task<Organization> UpdateOrganizationAsync(int id, string name);

        Task DeleteOrganizationAsync(int id);
    }
    
    public class OrganizationStorage : IOrganizationStorage
    {
        private readonly IRoutePlannerContextFactory _contextFactory;

        public OrganizationStorage(IRoutePlannerContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Organization> GetOrganizationAsync(int id)
        {
            await using var ctx = _contextFactory.Create();
            return await ctx.Organizations.FindAsync(id);
        }

        public async Task<Organization> CreateOrganizationAsync(string name)
        {
            await using var ctx = _contextFactory.Create();
            var organization = new Organization()
            {
                Name = name
            };
            ctx.Organizations.Add(organization);
            await ctx.SaveChangesAsync();
            // toDO проверить, что id проставился корректно
            return organization;
        }

        public async Task<Organization> UpdateOrganizationAsync(int id, string name)
        {
            await using var ctx = _contextFactory.Create();
            var organization = new Organization()
            {
                Id = id,
                Name = name
            };
            var entry = ctx.Organizations.Update(organization);
            await ctx.SaveChangesAsync();
            // toDo проверить, что если организации с id не существовало, то вернулся null
            return entry.Entity;
        }

        public async Task DeleteOrganizationAsync(int id)
        {
            await using var ctx = _contextFactory.Create();
            var organization = new Organization()
            {
                Id = id
            };
            ctx.Organizations.Remove(organization);
            await ctx.SaveChangesAsync();
        }
    }
}