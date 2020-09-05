﻿using System;
using System.Collections.Generic;

namespace RoutePlannerApi.Domain
{
    public class Route
    {
        private const int CustomerVisitedReward = 500;
        private const int RouteStartSameAsPreferredReward = 200;
        private const int RouteFinishSameAsPreferredReward = 200;

        public Manager Manager { get; }

        public List<Customer> Customers;

        private List<Customer> route = null;

        private int _fitness;

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
            if (route != null)
                return _fitness;
            _fitness = 0;
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
                        _fitness += RouteStartSameAsPreferredReward;
                
                    isFirstVisited = true;

                    prevCustomer = customer;
                    continue;
                }


                var travelTime =  (int)customer.Coordinate.GetTravelTime(prevCustomer.Coordinate).TotalMinutes;
                var nextTime = currentWorkTimeMinutes + travelTime + customer.MeetingDuration;
                if (nextTime < Manager.WorkTimeMinutes)
                {
                    meetingTimes.Add(new Tuple<int, int>(currentWorkTimeMinutes + travelTime,
                        currentWorkTimeMinutes + travelTime + customer.MeetingDuration));
                    currentWorkTimeMinutes = nextTime;
                    route.Add(customer);
                    _fitness += CustomerVisitedReward;
                    prevCustomer = customer;
                }
                else
                {
                    if (Manager.PreferredFinish.GetTravelTime(prevCustomer.Coordinate) < TimeSpan.FromMinutes(10))
                        _fitness += RouteFinishSameAsPreferredReward;
                    return _fitness;
                }

            }

            return _fitness;
        }
    }
}