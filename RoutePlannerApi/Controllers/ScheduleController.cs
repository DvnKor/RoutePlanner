using System;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Infrastructure.Rights;
using Microsoft.AspNetCore.Mvc;
using RoutePlannerApi.Auth;
using Storages;

namespace RoutePlannerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RightsAuthorize(Right.Manager)]
    public class ScheduleController : Controller
    {
        private readonly IManagerScheduleStorage _managerScheduleStorage;
        private readonly IUserContext _userContext;

        public ScheduleController(IManagerScheduleStorage managerScheduleStorage, IUserContext userContext)
        {
            _managerScheduleStorage = managerScheduleStorage;
            _userContext = userContext;
        }
        
        /// <summary>
        /// Получение графика менеджера на неделю
        /// </summary>
        [HttpGet("")]
        public async Task<ActionResult> GetManagerScheduleForWeek([FromQuery] int managerId, [FromQuery] DateTime weekDate)
        {
            var currentUser = _userContext.User;
            var convertedTime = TimeZoneInfo.ConvertTime(weekDate,
                TimeZoneInfo.FindSystemTimeZoneById("Ekaterinburg Standard Time"));
            if (!currentUser.HasRight(Right.Admin) && currentUser.Id != managerId)
            {
                return Forbid();
            }
            
            var managerScheduleForWeek = await _managerScheduleStorage
                .GetManagerScheduleForWeek(managerId, convertedTime);
            return Ok(managerScheduleForWeek);
        }
        
        /// <summary>
        /// Создание смены
        /// </summary>
        [HttpPost("")]
        public async Task<ActionResult> CreateManagerSchedule([FromBody] ManagerSchedule managerSchedule)
        {
            var managerScheduleId = await _managerScheduleStorage.CreateManagerSchedule(managerSchedule);
            return Ok(managerSchedule);
        }
        
        /// <summary>
        /// Обновление смены
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateManagerSchedule(
            int id, [FromBody] UpdateManagerScheduleDto updateManagerScheduleDto)
        {
            var updatedManagerSchedule = await _managerScheduleStorage.UpdateManagerSchedule(id, updateManagerScheduleDto);
            if (updatedManagerSchedule == null)
            {
                return NotFound(id);
            }

            return Ok(updatedManagerSchedule);
        }
        
        /// <summary>
        /// Удаление смены
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteManagerSchedule(int id)
        {
            var deleted = await _managerScheduleStorage.DeleteManagerSchedule(id);
            return Ok(id);
        }
    }
}