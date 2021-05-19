using System.Linq;
using Entities.Models;

namespace Storages.Extensions
{
    public static class ClientQueryExtensions
    {
        public static IQueryable<Client> Search(this IQueryable<Client> clients, string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return clients;
            }
            var lowerQuery = query.ToLower();
            return clients.Where(client =>
                client.Name.ToLower().Contains(lowerQuery) ||
                (client.Email != null && client.Email.ToLower().Contains(lowerQuery)) ||
                (client.MobilePhone != null && client.MobilePhone.ToLower().Contains(lowerQuery)) ||
                (client.Telegram != null && client.Telegram.ToLower().Contains(lowerQuery)));
        }
    }
}