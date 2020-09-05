using System.Collections.Generic;
using RoutePlannerApi.Domain;

namespace RoutePlannerApi.Repositories
{
    public class CustomerRepository
    {
        private int _currentCustomerId;
        private readonly List<Customer> _customers;

        public CustomerRepository()
        {
            _customers = GetRandomCustomers(50);
        }
        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public void AddCustomer(Customer customer)
        {
            _currentCustomerId++;
            var newCustomer = new Customer(_currentCustomerId, customer.Coordinate, customer.MeetingDuration);
            _customers.Add(newCustomer);
        }

        public void DeleteCustomer(int customerId)
        {
            _customers.Remove(FindCustomerById(customerId));
        }

        public Customer FindCustomerById(int customerId)
        {
            return _customers.Find(c => c.Id == customerId);
        }

        private Customer GetRandomCustomer()
        {
            _currentCustomerId++;
            return new Customer(_currentCustomerId, new Coordinate(), 60);
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