using System.Collections.Generic;
using RoutePlannerApi.Domain;

namespace RoutePlannerApi.Repositories
{
    public class ManagerRepository
    {
        private int currentManagerId = 0;
        private List<Manager> managers;

        private Manager GetRandomManager()
        {
            currentManagerId++;
            return new Manager(new SimpleCoordinate(), new SimpleCoordinate(), currentManagerId, 8 * 60);
        }

        public List<Manager> GetAllManagers()
        {
            return managers ??= GetRandomManagers(5);
        }

        public List<Manager> GetRandomManagers(int count)
        {
            var result = new List<Manager>();
            for (var i = 0; i < count; i++)
            {
                result.Add(GetRandomManager());
            }

            return result;
        }
    }
}