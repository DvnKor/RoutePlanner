using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.VisualBasic;

namespace RoutePlanner
{
    public class Route
    {
        private const int CustomerVisitedReward = 500;
        private const int RouteStartSameAsPreferredReward = 200;
        private const int RouteFinishSameAsPreferredReward = 200;
        private const int PreferredManagerReward = 200;

        private Manager Manager;

        public List<Customer> Customers;

        private List<Customer> route = null;

        private List<Tuple<int, int>> meetingTimes = new List<Tuple<int, int>>();

        public Route(List<Customer> customers, Manager manager)
        {
            Customers = customers;
            Manager = manager;
        }

        public List<Customer> GetRoute()
        {
            if (route == null)
            {
                GetFitness();
            }

            return route;
        }

        public int GetFitness()
        {
            var fitness = 0;
            var currentWorkTimeMinutes = 0;

            route = new List<Customer>();
            var isFirstVisited = false;

            var prevCustomer = Customers[0];

            foreach (var customer in Customers)
            {
                if (customer.IsVisited)
                    continue;
                if (!isFirstVisited)
                {
                    meetingTimes.Add(new Tuple<int, int>(currentWorkTimeMinutes,
                        currentWorkTimeMinutes + customer.MeetingDuration));
                    currentWorkTimeMinutes += customer.MeetingDuration;
                    route.Add(customer);

                    if (Manager.PreferredStart.GetTravelTime(customer.Coordinate) < TimeSpan.FromMinutes(10))
                        fitness += RouteStartSameAsPreferredReward;
                
                    isFirstVisited = true;

                    prevCustomer = customer;
                    continue;
                }


                var travelTime =  customer.Coordinate.GetTravelTime(prevCustomer.Coordinate).Minutes;

                if (currentWorkTimeMinutes + travelTime + customer.MeetingDuration < Manager.WorkTimeMinutes)
                {
                    meetingTimes.Add(new Tuple<int, int>(currentWorkTimeMinutes + travelTime,
                        currentWorkTimeMinutes + travelTime + customer.MeetingDuration));
                    currentWorkTimeMinutes = currentWorkTimeMinutes + travelTime + customer.MeetingDuration;
                    route.Add(customer);
                    fitness += CustomerVisitedReward;
                    prevCustomer = customer;
                }
                else
                {
                    if (Manager.PreferredFinish.GetTravelTime(prevCustomer.Coordinate) < TimeSpan.FromMinutes(10))
                        fitness += RouteFinishSameAsPreferredReward;
                    return fitness;
                }

            }

            return fitness;
        }
    }
}