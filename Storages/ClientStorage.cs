using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Storages.Extensions;

namespace Storages
{
    public interface IClientStorage
    {
        Task<int> CreateClient(Client client);

        Task<Client[]> GetClients(int offset, int limit, string query);

        Task<Client> UpdateClient(int id, UpdateClientDto updateClientDto);

        Task<bool> DeleteClient(int id);
    }

    public class ClientStorage : IClientStorage
    {
        private readonly IRoutePlannerContextFactory _contextFactory;

        public ClientStorage(IRoutePlannerContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> CreateClient(Client client)
        {
            await using var ctx = _contextFactory.Create();
            ctx.Clients.Add(client);
            await ctx.SaveChangesAsync();
            return client.Id;
        }

        public async Task<Client[]> GetClients(int offset, int limit, string query)
        {
            await using var ctx = _contextFactory.Create();
            var clients = await ctx.Clients
                .Search(query)
                .OrderBy(client => client.Id)
                .LimitByOffset(offset, limit)
                .ToArrayAsync();
            return clients;
        }

        public async Task<Client> UpdateClient(int id, UpdateClientDto updateClientDto)
        {
            await using var ctx = _contextFactory.Create();
            var clientToUpdate = await ctx.Clients
                .FirstOrDefaultAsync(client => client.Id == id);
            if (clientToUpdate == null)
            {
                return null;
            }

            clientToUpdate.Name = updateClientDto.Name;
            clientToUpdate.Email = updateClientDto.Email;
            clientToUpdate.MobilePhone = updateClientDto.MobilePhone;
            clientToUpdate.Telegram = updateClientDto.Telegram;
            clientToUpdate.Coordinate = updateClientDto.Coordinate;
            
            ctx.Clients.Update(clientToUpdate);
            await ctx.SaveChangesAsync();
            
            return clientToUpdate;
        }

        public async Task<bool> DeleteClient(int id)
        {
            await using var ctx = _contextFactory.Create();
            var client = await ctx.Clients.FindAsync(id);
            if (client == null)
            {
                return false;
            }
            ctx.Clients.Remove(client);
            return await ctx.SaveChangesAsync() > 0;
        }
    }
}