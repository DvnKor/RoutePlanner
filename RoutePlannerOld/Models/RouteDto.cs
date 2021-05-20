using System.Collections.Generic;

namespace RoutePlannerOld.Models
{
    public class RouteDto
    {
        public RouteDto(int managerId, List<CustomerDto> route)
        {
            ManagerId = managerId;
            Route = route;
        }

        public int ManagerId { get; set; }
        public List<CustomerDto> Route { get; set; }
    }
}