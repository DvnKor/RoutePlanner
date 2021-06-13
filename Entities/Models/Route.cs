using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Route")]
    public class Route
    {
        public int Id { get; set; }
        
        public int ManagerScheduleId { get; set; }
        
        /// <summary>
        /// Информация о расписании менеджера
        /// </summary>
        public ManagerSchedule ManagerSchedule { get; set; }

        /// <summary>
        /// Все возможные встречи
        /// </summary>
        [NotMapped]
        public IList<Meeting> PossibleMeetings { get; set; }
        
        /// <summary>
        /// Встречи, которые менеджер посетит
        /// </summary>
        [NotMapped]
        public List<Meeting> SuitableMeetings { get; set; }
        
        public List<Meeting> Meetings { get; set; }

        /// <summary>
        /// Длина маршрута в метрах
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Суммарное время ожидания (в минутах)
        /// </summary>
        public double WaitingTime { get; set; }

        public bool FinishesAsPreferred { get; set; }
    }
}