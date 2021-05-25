using System.Linq;
using System.Text;
using Entities.Models;

namespace GeneticAlgorithm.Contracts.Models
{
    public class Genotype
    {
        public Route[] Routes { get; }

        public double Fitness { get; set; }

        public double Distance => Routes.Select(route => route.Distance).Sum();

        public double WaitingTime => Routes.Select(route => route.WaitingTime).Sum();

        public int SuitableMeetingsCount => Routes.Select(route => route.SuitableMeetings.Count).Sum();

        public int CountRouteFinishesAsPreferred => Routes.Count(x => x.FinishesAsPreferred);

        public Genotype(Route[] routes)
        {
            Routes = routes;
        }
        
        public string PrintParameters()
        {
            var builder = new StringBuilder();
            builder.AppendLine().AppendLine($"Общий фитнес: {Fitness}");
            builder.AppendLine($"Общее расстояние в метрах: {Distance}");
            builder.AppendLine($"Общее время ожидания в минутах: {WaitingTime}");
            builder.AppendLine($"Общее количество встреч: {SuitableMeetingsCount}");
            builder.AppendLine(
                $"Количество маршрутов заканчивающихся в желаемой точке: {CountRouteFinishesAsPreferred}");
            builder.AppendLine().AppendLine();

            return builder.ToString();
        }

        public string PrintRoutesWithParameters()
        {
            var builder = new StringBuilder();
            foreach (var route in Routes)
            {
                builder.AppendLine($"### Менеджер {route.ManagerSchedule.UserId}.");
                builder.AppendLine(
                    $"Начальная координата: {route.ManagerSchedule.StartCoordinate.Latitude}," +
                    $" {route.ManagerSchedule.StartCoordinate.Longitude}");
                builder.AppendLine($"Начало смены {route.ManagerSchedule.StartTime:g}.");
                builder.AppendLine($"Конец смены {route.ManagerSchedule.EndTime:g}.");
                builder.AppendLine(
                    $"Конечная желаемая координата: {route.ManagerSchedule.StartCoordinate.Latitude}," +
                    $" {route.ManagerSchedule.StartCoordinate.Longitude}");
                builder.AppendLine();
                
                builder.AppendLine($"Встречи:").AppendLine();
                foreach (var meeting in route.SuitableMeetings)
                {
                    builder.Append($"{meeting.StartTime:HH:mm} - {meeting.EndTime:HH:mm}. ");
                    builder.Append($"Клиент: {meeting.ClientId}. ");
                    builder.Append($"Широта: {meeting.Coordinate.Latitude}. Долгота: {meeting.Coordinate.Longitude}.\n");
                }

                if (route.FinishesAsPreferred)
                {
                    builder.AppendLine($"Маршрут заканчивается в желаемой конечной точке менеджера.");
                }

                builder.AppendLine($"Расстояние в метрах: {route.Distance}");
                builder.AppendLine($"Время ожидания в минутах: {route.WaitingTime}");
                builder.AppendLine($"Количество встреч: {route.SuitableMeetings.Count}");
                builder.AppendLine();
            }

            builder.Append(PrintParameters());

            return builder.ToString();
        }
    }
}