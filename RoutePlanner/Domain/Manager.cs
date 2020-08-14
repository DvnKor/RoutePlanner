using System;

namespace RoutePlanner
{
    public class Manager
    {
        public readonly int Id;
        public readonly ICoordinate PreferredStart;
        public readonly ICoordinate PreferredFinish;
        public readonly int WorkTimeMinutes;

        public Manager(ICoordinate preferredStart, ICoordinate preferredFinish, int id, int workTimeMinutes)
        {
            PreferredStart = preferredStart;
            PreferredFinish = preferredFinish;
            Id = id;
            WorkTimeMinutes = workTimeMinutes;
        }
    }
}