using System;
using System.Collections.Generic;

namespace RoutePlanner
{
    public class Organization
    {
        public int Id;
        public List<Manager> Managers;
        public List<Customer> Customers;
        public List<Admin> Admins;

        public Organization(int id, List<Manager> managers, List<Customer> customers, List<Admin> admins)
        {
            Id = id;
            Managers = managers;
            Customers = customers;
            Admins = admins;
        }
    }
}