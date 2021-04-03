using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Rights;

namespace Entities.Models
{
    [Table("RightInfo")]
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
            new RightInfo() {Right = Right.Manager, Description = "Менеджер"},
            new RightInfo() {Right = Right.Admin, Description = "Администратор"}
        };
    }
}