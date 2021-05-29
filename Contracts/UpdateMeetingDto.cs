using System;
using Infrastructure.Common;

namespace Contracts
{
    public class UpdateMeetingDto
    {
        public DateTime AvailableTimeStart { get; set; }
        
        public DateTime AvailableTimeEnd { get; set; }
        
        public int Duration { get; set; }
        
        public Coordinate Coordinate { get; set; }
    }
}