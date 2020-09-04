namespace RoutePlannerApi.Models
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public CoordinateDto Coordinate { get; set; }
        public int MeetingDuration { get; set; }
    }
}