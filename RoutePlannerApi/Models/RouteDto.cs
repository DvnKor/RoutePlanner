using System.Collections.Generic;

namespace RoutePlannerApi.Models
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