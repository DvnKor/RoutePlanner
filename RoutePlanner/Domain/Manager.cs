using System;

namespace RoutePlanner
{
    public class Manager
    {
        public readonly ICoordinate PreferredStart;
        public readonly ICoordinate PreferredFinish;
        public readonly Guid Id;
        public readonly int WorkTimeMinutes;

        public Manager(ICoordinate preferredStart, ICoordinate preferredFinish, Guid id, int workTimeMinutes)
        {
            PreferredStart = preferredStart;
            PreferredFinish = preferredFinish;
            Id = id;
            WorkTimeMinutes = workTimeMinutes;
        }
    }
}