namespace RoutePlannerOld.Domain
{
    public class Customer
    {
        public readonly int Id;
        public readonly Coordinate Coordinate;
        public readonly int MeetingDuration;
        public bool IsVisited = false;
        public Customer(int id, Coordinate coordinate, int meetingDuration)
        {
            Coordinate = coordinate;
            Id = id;
            MeetingDuration = meetingDuration;
        }
    }
}