using System.Collections.Generic;

namespace RoutePlanner.Repositories
{
    public class CustomerRepository
    {
        private int currentCustomerId = 0;
        private List<Customer> customers = null;

        private Customer GetRandomCustomer()
        {
            currentCustomerId++;
            return new Customer(currentCustomerId, new SimpleCoordinate(), 60);
        }

        public List<Customer> GetAllCustomers()
        {
            return customers ??= GetRandomCustomers(50);
        }

        private List<Customer> GetRandomCustomers(int count)
        {
            var result = new List<Customer>();
            for (var i = 0; i < count; i++)
            {
                result.Add(GetRandomCustomer());
            }
            return result;
        }
    }
}