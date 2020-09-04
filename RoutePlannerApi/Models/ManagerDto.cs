namespace RoutePlannerApi.Models
{
    public class ManagerDto
    {
        public int Id { get; set; }
        public CoordinateDto PreferredStart { get; set; }
        public CoordinateDto PreferredFinish { get; set; }
        public int WorkTimeMinutes { get; set; }
    }
}