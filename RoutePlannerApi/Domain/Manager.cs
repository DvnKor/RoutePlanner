namespace RoutePlannerApi.Domain
{
    public class Manager
    {
        public readonly int Id;
        public readonly Coordinate PreferredStart;
        public readonly Coordinate PreferredFinish;
        public readonly int WorkTimeMinutes;

        public Manager(Coordinate preferredStart, Coordinate preferredFinish, int id, int workTimeMinutes)
        {
            PreferredStart = preferredStart;
            PreferredFinish = preferredFinish;
            Id = id;
            WorkTimeMinutes = workTimeMinutes;
        }
    }
}