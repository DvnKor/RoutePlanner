using System;

namespace RoutePlanner
{
    public class Customer
    {
        public readonly ICoordinate Coordinate;
        public readonly Guid Id;
        public readonly int MeetingDuration;
        public bool IsVisited = false;
        public Customer(ICoordinate coordinate, Guid id, int meetingDuration)
        {
            Coordinate = coordinate;
            Id = id;
            MeetingDuration = meetingDuration;
        }
    }
}