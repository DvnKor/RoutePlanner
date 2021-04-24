using System.Linq;
using System.Text;
using Entities.Models;

namespace GeneticAlgorithm.Contracts.Models
{
    public class Genotype
    {
        public Route[] Routes { get; }

        public double Fitness => Routes.Select(route => route.Fitness).Sum();

        public double Distance => Routes.Select(route => route.Distance).Sum();

        public double WaitingTime => Routes.Select(route => route.WaitingTime).Sum();

        public int SuitableMeetingsCount => Routes.SelectMany(route => route.SuitableMeetings).Count();

        public Genotype(Route[] routes)
        {
            Routes = routes;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var route in Routes)
            {
                builder.AppendLine($"Менеджер {route.ManagerSchedule.UserId}").AppendLine();
                builder.AppendLine($"Встречи:").AppendLine();
                foreach (var meeting in route.SuitableMeetings)
                {
                    builder.Append($"{meeting.StartTime:HH:mm} - {meeting.EndTime:HH:mm}. ");
                    builder.Append($"Клиент: {meeting.ClientId}. ");
                    builder.Append($"Широта: {meeting.Coordinate.Latitude}. Долгота: {meeting.Coordinate.Longitude}.\n");
                }

                builder.AppendLine().AppendLine($"Фитнес: {route.Fitness}");
                builder.AppendLine($"Расстояние в метрах: {route.Distance}");
                builder.AppendLine($"Время ожидания в минутах: {route.WaitingTime}");
                builder.AppendLine($"Количество встреч: {route.SuitableMeetings.Count}");
                builder.AppendLine();
            }
            
            builder.AppendLine().AppendLine($"Общий фитнес: {Fitness}");
            builder.AppendLine($"Общее расстояние в метрах: {Distance}");
            builder.AppendLine($"Общее время ожидания в минутах: {WaitingTime}");
            builder.AppendLine($"Общее количество встреч: {SuitableMeetingsCount}");
            builder.AppendLine();

            return builder.ToString();
        }
    }
}