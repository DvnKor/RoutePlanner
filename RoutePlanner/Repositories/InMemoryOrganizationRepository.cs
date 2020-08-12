using System;
using System.Collections.Generic;

namespace RoutePlanner.Repositories
{
    public class InMemoryOrganizationRepository
    {
        private readonly int maxX = 100;
        private readonly int maxY = 100;
        private readonly Random random = new Random();
        private int managersCount = 10;
        private int customersCount = 50;

        public List<Organization> GetAllOrganizations()
        {
            return new List<Organization>() {GetRandomOrganization()};
        }

        private Organization GetRandomOrganization()
        {
            var managers = new List<Manager>();
            var customers = new List<Customer>();
            for (var i = 0; i < managersCount; i++)
            {
                managers.Add(GetRandomManager());
            }

            for (var i = 0; i < customersCount; i++)
            {
                customers.Add(GetRandomCustomer());
            }

            return new Organization(Guid.NewGuid(), managers, customers, null);
        }

        private Manager GetRandomManager()
        {
            return new Manager(GetRandomSimpleCoordinate(), GetRandomSimpleCoordinate(), Guid.NewGuid(), 8 * 60);
        }

        private Customer GetRandomCustomer()
        {
            return new Customer(GetRandomSimpleCoordinate(), Guid.NewGuid(), 60);
        }

        private SimpleCoordinate GetRandomSimpleCoordinate()
        {
            return new SimpleCoordinate(random.Next(0, maxX), random.Next(0, maxY));
        }
    }
}