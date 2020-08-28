using System.Collections.Generic;

namespace RoutePlanner.Repositories
{
    public class CustomerRepository
    {
        private int currentCustomerId = 0;

        private Customer GetRandomCustomer()
        {
            currentCustomerId++;
            return new Customer(currentCustomerId, new SimpleCoordinate(), 60);
        }

        public List<Customer> GetRandomCustomers(int count)
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