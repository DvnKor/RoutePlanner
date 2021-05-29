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
    [RightsAuthorize(Right.Admin)]
    [ApiController]
    public class MeetingsController : Controller
    {
        private readonly IMeetingStorage _meetingsStorage;

        public MeetingsController(IMeetingStorage meetingsStorage)
        {
            _meetingsStorage = meetingsStorage;
        }
        
        /// <summary>
        /// Создание встречи
        /// </summary>
        [HttpPost("")]
        public async Task<ActionResult> CreateMeeting([FromBody] Meeting meeting)
        {
            var meetingId = await _meetingsStorage.CreateMeeting(meeting);
            return Ok(meeting);
        }
        
        /// <summary>
        /// Обновление встречи
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateMeeting(int id, [FromBody] UpdateMeetingDto updateMeetingDto)
        {
            var updatedMeeting = await _meetingsStorage.UpdateMeeting(id, updateMeetingDto);
            if (updatedMeeting == null)
            {
                return NotFound(id);
            }

            return Ok(updatedMeeting);
        }
        
        /// <summary>
        /// Обновление времени окончания встречи
        /// </summary>
        [HttpPut("{id:int}/end-time")]
        public async Task<ActionResult> UpdateMeetingEndTime(int id, [FromBody] DateTime endTime)
        {
            var updatedMeeting = await _meetingsStorage.UpdateEndTime(id, endTime);
            if (updatedMeeting == null)
            {
                return NotFound(id);
            }

            return Ok(updatedMeeting);
        }
        
        /// <summary>
        /// Получение встреч
        /// </summary>
        [HttpGet("")]
        public async Task<ActionResult> GetMeetings(
            [FromQuery] int offset, 
            [FromQuery] int limit,
            [FromQuery] string query,
            [FromQuery] DateTime date)
        {
            var meetings = await _meetingsStorage.GetMeetings(offset, limit, query, date);
            return Ok(meetings);
        }
        
        /// <summary>
        /// Удаление встречи
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMeeting(int id)
        {
            var deleted = await _meetingsStorage.DeleteMeeting(id);
            return Ok(id);
        }
    }
}