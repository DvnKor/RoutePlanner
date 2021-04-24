using System.Collections.Generic;
using Entities.Models;
using GeneticAlgorithm.Domain.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IPopulationGenerator
    {
        /// <summary>
        /// Возвращает ранжированную популяцию - набор генотипов, отсортированный
        /// по значению функции приспособленности в порядке убывания
        /// </summary>
        /// <param name="managerSchedules">Расписания менеджеров</param>
        /// <param name="meetings">Все встречи</param>
        /// <param name="populationSize">Размер популяции</param>
        Genotype[] CreateRankedPopulation(
            List<ManagerSchedule> managerSchedules,
            List<Meeting> meetings,
            int populationSize);
    }
}