using System.Collections.Generic;
using Infrastructure.Common;

namespace Entities.Models
{
    public class RightInfo
    {
        public Right Right { get; set; }
        
        public string Description { get; set; }
        
        public List<UserRight> UserRights { get; set; }
    }

    public static class RightInfoHelpers
    {
        public static RightInfo[] DefaultRights = new[]
        {
            new RightInfo() {Right = Right.Manager, Description = "Менеджер банка"},
            new RightInfo() {Right = Right.Admin, Description = "Админ"}
        };
    }
}