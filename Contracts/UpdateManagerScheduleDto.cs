using System;
using Infrastructure.Common;

namespace Contracts
{
    public class UpdateManagerScheduleDto
    {
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public Coordinate StartCoordinate { get; set; }
        
        public Coordinate EndCoordinate { get; set; }
    }
}