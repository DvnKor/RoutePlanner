using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Contracts;
using Infrastructure.Common;
using Infrastructure.Rights;

namespace Entities.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public string Picture { get; set; }
        
        public string MobilePhone { get; set; }
        
        public string Telegram { get; set; }
        
        public Coordinate Coordinate { get; set; }
        
        public List<UserRight> UserRights { get; set; }
        
        public List<ManagerSchedule> ManagerSchedules { get; set; }

        public bool HasRight(Right right)
        {
            return HasRights(new[] {right});
        }
        
        public bool HasRights(Right[] rights)
        {
            if (UserRights == null) return false;
            var userRightsValues = UserRights.Select(userRight => userRight.Right);
            return !rights.Except(userRightsValues).Any();
        }

        public UserDto ToDto()
        {
            var userDto = new UserDto
            {
                Id = Id,
                Name = Name,
                Email = Email,
                Picture = Picture,
                MobilePhone = MobilePhone,
                Telegram = Telegram,
                Coordinate = Coordinate,
                Rights = UserRights?.Select(userRight => userRight.Right).ToArray(),
            };
            
            if (UserRights?.Count > 0)
            {
                var maxUserRight = UserRights
                    .OrderByDescending(userRight => userRight.Right)
                    .FirstOrDefault();
                userDto.Position = maxUserRight?.RightInfo.Description;
            }
            return userDto;
        }
    }
}