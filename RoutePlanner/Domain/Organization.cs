using System;
using System.Collections.Generic;

namespace RoutePlanner
{
    public class Organization
    {
        public Guid Id;
        public List<Manager> Managers;
        public List<Customer> Customers;
        public List<Admin> Admins;

        public Organization(Guid id, List<Manager> managers, List<Customer> customers, List<Admin> admins)
        {
            Id = id;
            Managers = managers;
            Customers = customers;
            Admins = admins;
        }
    }
}