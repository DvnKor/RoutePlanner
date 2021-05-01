using System.Collections.Generic;
using Entities.Models;
using GeneticAlgorithm.Contracts.Models;

namespace GeneticAlgorithm.Contracts
{
    public interface IPopulationGenerator
    {
        /// <summary>
        /// Возвращает ранжированную популяцию
        /// </summary>
        /// <param name="managerSchedules">Расписания менеджеров</param>
        /// <param name="meetings">Все встречи</param>
        /// <param name="populationSize">Размер популяции</param>
        List<Genotype> CreatePopulation(
            List<ManagerSchedule> managerSchedules,
            List<Meeting> meetings,
            int populationSize);
    }
}