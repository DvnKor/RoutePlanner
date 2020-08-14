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

        private int currentManagerId = 0;
        private int currentOrganizationId = 0;
        private int currentCustomerId = 0;

        public List<Organization> GetAllOrganizations()
        {
            return new List<Organization> {GetRandomOrganization()};
        }

        private Organization GetRandomOrganization()
        {
            var managers = new List<Manager>();
            var customers = new List<Customer>();
            currentOrganizationId++;
            for (var i = 0; i < managersCount; i++)
            {
                managers.Add(GetRandomManager());
            }

            for (var i = 0; i < customersCount; i++)
            {
                customers.Add(GetRandomCustomer());
            }

            return new Organization(currentOrganizationId, managers, customers, null);
        }

        private Manager GetRandomManager()
        {
            currentManagerId++;
            return new Manager(GetRandomSimpleCoordinate(), GetRandomSimpleCoordinate(), currentManagerId, 8 * 60);
        }

        private Customer GetRandomCustomer()
        {
            currentCustomerId++;
            return new Customer(currentCustomerId, GetRandomSimpleCoordinate(), 60);
        }

        private SimpleCoordinate GetRandomSimpleCoordinate()
        {
            return new SimpleCoordinate(random.Next(0, maxX), random.Next(0, maxY));
        }
    }
}