using System.Collections.Generic;

namespace RoutePlanner.Repositories
{
    public class ManagerRepository
    {
        private int currentManagerId = 0;

        private Manager GetRandomManager()
        {
            currentManagerId++;
            return new Manager(new SimpleCoordinate(), new SimpleCoordinate(), currentManagerId, 8 * 60);
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