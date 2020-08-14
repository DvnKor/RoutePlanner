using System;

namespace RoutePlanner
{
    public class Customer
    {
        public readonly int Id;
        public readonly ICoordinate Coordinate;
        public readonly int MeetingDuration;
        public bool IsVisited = false;
        public Customer(int id, ICoordinate coordinate, int meetingDuration)
        {
            Coordinate = coordinate;
            Id = id;
            MeetingDuration = meetingDuration;
        }
    }
}