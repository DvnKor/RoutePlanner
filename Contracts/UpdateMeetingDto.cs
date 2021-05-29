using System;
using Infrastructure.Common;

namespace Contracts
{
    public class UpdateMeetingDto
    {
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public Coordinate Coordinate { get; set; }
    }
}