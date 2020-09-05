using System.Collections.Generic;
using RoutePlannerApi.Domain;

namespace RoutePlannerApi.Repositories
{
    public class ManagerRepository
    {
        private int _currentManagerId = 0;
        private List<Manager> _managers;

        private Manager GetRandomManager()
        {
            _currentManagerId++;
            return new Manager(new Coordinate(), new Coordinate(), _currentManagerId, 8 * 60);
        }

        public List<Manager> GetAllManagers()
        {
            return _managers ??= GetRandomManagers(5);
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